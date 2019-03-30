using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Helper
{
    public class PluginViewModelHelper
    {
        public static PluginInfoModelItem GetPluginAssemblyInfo(Assembly assembly)
        {
            return new PluginInfoModelItem() { TargetAssembly = assembly };
        }
    }


    public class PluginInfoModelItem
    {
        public string AssemblyName { get { return TargetAssembly.FullName; } }
        public string AssemblyVersion { get { return TargetAssembly.ImageRuntimeVersion; } }
        public string AssemblyPath { get { return TargetAssembly.Location; } }
        public IEnumerable<Module> AssemblyModules { get { return TargetAssembly.Modules; } }
        public IEnumerable<Type> AssemblyTypes { get { return TargetAssembly.DefinedTypes; } }
        public Assembly TargetAssembly{ get; set; }
    }
}
