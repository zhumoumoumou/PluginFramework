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
            var scanRes = manager.WorkDirectoryPluginScan();

            foreach (var item in scanRes)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void DllLoadingChainTest()
        {
            var dllPaths = manager.WorkDirectoryPluginScan();
            var assemblies = manager.LoadAssembliesFromDlls(dllPaths);
            manager.AppendPluginManagedItems(assemblies);

            Console.ReadKey();
        }

        [TestMethod]
        public void GetComponentsTest()
        {
            
        }
    }
}
