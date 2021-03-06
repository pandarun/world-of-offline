﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lucene.Net.Store;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using ServiceBusHub;
using core.Business;
using StructureMap.Diagnostics;

namespace web
{
    public class WebRole : RoleEntryPoint
    {
        private class IndexInfo
        {
            public Lucene.Net.Store.Directory Directory { get; set; }
            public IDisposable IRS { get; set; }
        }
        private IndexInfo[] _localIndexes;

        private SearchWriterService _luceneWriter;
        private IDisposable _luceneTempInstanceSubscription;
        private IDisposable _luceneSearchSubscription;

        public override bool OnStart()
        {
            var readPath = RoleEnvironment.GetLocalResource("ReadStorage").RootPath;
            var writePath = RoleEnvironment.GetLocalResource("WriteStorage").RootPath;

            var subscription = "lucene" ;
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("Blob"));
            var hub = new Hub(RoleEnvironment.GetConfigurationSettingValue("ServiceBus"));
            
            var criticalWait = Task.Run(() =>
            {
                string writeFolder = Path.Combine(writePath, "lucene");
                if (!System.IO.Directory.Exists(writeFolder))
                {
                    System.IO.Directory.CreateDirectory(writeFolder);
                }
                string combine = Path.Combine(readPath, subscription);
                if (!System.IO.Directory.Exists(combine))
                {
                    System.IO.Directory.CreateDirectory(combine);
                }

                var localDirectory = new Lucene.Net.Store.SimpleFSDirectory(new DirectoryInfo(combine));
                var masterDirectory = new AzureDirectory(account, subscription);
                var irs = new IntermediateReaderService(masterDirectory, hub, localDirectory, true);

                masterDirectory.Dispose();

                return new IndexInfo {IRS = irs as IDisposable, Directory = localDirectory};
            })
                .ContinueWith(t => _localIndexes = new[] {t.Result});

            _luceneWriter = new SearchWriterService(new AzureDirectory(account, "lucene", new Lucene.Net.Store.SimpleFSDirectory(new DirectoryInfo(writePath))), true);
            var subscriptionName = string.Format("{0:yyyyMMddHHmmss}_{1}", DateTime.UtcNow, Guid.NewGuid().ToString().Replace("-", string.Empty));
            hub.CreateSubscription("lucene", subscriptionName)
                .ContinueWith(t =>
                {
                    _luceneTempInstanceSubscription = t.Result;
                    return hub.SubscribeWithRoutingKey("lucene", subscriptionName, OnSearchMessage)
                        .ContinueWith(t2 => _luceneSearchSubscription = t.Result);
                })
                .Unwrap();

            return base.OnStart();
        }

        public override void OnStop()
        {
            if (_localIndexes != null)
            {
                foreach (var localIndex in _localIndexes)
                {
                    localIndex.IRS.Dispose();
                    localIndex.Directory.Dispose();
                }
            }

            _luceneSearchSubscription.Dispose();
            _luceneWriter.Dispose();
            _luceneTempInstanceSubscription.Dispose();

            base.OnStop();
        }

        private BrokeredMessageResults OnSearchMessage(GenericPayloadDeliverInfo arg)
        {
            var updateMessage = arg.Payload.GetBody<GenericSearchItem>();

            var body = updateMessage.GetBody();
            _luceneWriter.Update(body);

            return BrokeredMessageResults.Ack;
        }
    }
}