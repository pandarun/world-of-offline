// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using core.Business;
using Lucene.Net.Store;
using Microsoft.WindowsAzure.ServiceRuntime;
using ServiceBusHub;
using StructureMap;
namespace web.DependencyResolution {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });

                            x.For<Lucene.Net.Store.Directory>()
                             .Singleton()
                             .Use(() => new SimpleFSDirectory(new System.IO.DirectoryInfo(System.IO.Path.Combine(RoleEnvironment.GetLocalResource("ReadStorage").RootPath, "lucene"))));


                            x.For<core.Business.ISearchReaderService>()
                             .HttpContextScoped()
                             .Use<SearchReaderService>();

                            string sb;
                            try { sb = RoleEnvironment.GetConfigurationSettingValue("ServiceBus"); }
                            catch { sb = "error"; }
                            x.For<ServiceBusHub.IHub>()
                             .Singleton()
                             .Use<ServiceBusHub.Hub>()
                             .Ctor<string>("connectionString")
                             .Is(sb);
                        });
            return ObjectFactory.Container;
        }
    }
}