using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Lucene.Net.Store.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using ServiceBusHub;
using core.Business;

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

        public override bool OnStart()
        {
            var rootPath = RoleEnvironment.GetLocalResource("ReadStorage").RootPath;

            var subscriptions = new[] { "lucene" };
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("Blob"));
            var hub = new Hub(RoleEnvironment.GetConfigurationSettingValue("ServiceBus"));

            var tasks = subscriptions.Select(s => Task.Run(() =>
            {
                var localDirectory = new Lucene.Net.Store.SimpleFSDirectory(new DirectoryInfo(Path.Combine(rootPath, s)));
                var masterDirectory = new AzureDirectory(account, s);
                var irs = new IntermediateReaderService(masterDirectory, hub, localDirectory);

                masterDirectory.Dispose();

                return new IndexInfo { IRS = irs as IDisposable, Directory = localDirectory };
            }))
                                     .ToArray();

            var criticalToWait = Task.WhenAll(tasks)
                .ContinueWith(_ => tasks.Where(t => !t.IsFaulted && t.IsCompleted).Select(t => t.Result).ToArray())
                .ContinueWith(t => _localIndexes = t.Result);

            criticalToWait.Wait();

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
            base.OnStop();
        }
    }
}