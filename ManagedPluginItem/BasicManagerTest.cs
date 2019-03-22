using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginFramework.Attribute;
using PluginFramework.Interface;
using PluginFramework.Manager;

namespace Tests
{
    [TestClass]
    public class BasicManagerTest
    {
        BasicManager manager = new BasicManager()
        {
            WorkDirectory = @"D:\zlkWorkspace\CSharpProj\PluginFramework\TestPlugins"
        };

        [TestMethod]
        public void WorkDirectoryPluginScanTest()
        {
            var scanRes = manager.WorkDirectoryDllFilesScan();

            foreach (var item in scanRes)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void DllLoadingChainTest()
        {
            var dllPaths = manager.WorkDirectoryDllFilesScan();
            var assemblies = manager.LoadAssembliesFromDlls(dllPaths);

            foreach (var ass in assemblies)
            {
                var types = ass.GetTypes();
                foreach (var type in types)
                {
                    manager.AppendPluginManagedItem(type);
                }
            }
        }

        [TestMethod]
        public void GetComponentsTest()
        {
            manager.DllLoadingChain();
            foreach (var item in manager.LoadList)
            {
                ((ILoadable)item.Plugin).Load();
            }
        }
    }
}
