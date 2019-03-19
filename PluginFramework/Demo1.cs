using PluginFramework.Attribute;
using PluginFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PluginFramework
{
    /// <summary>
    /// 插件类声明互斥属性。
    /// </summary>
    class Demo1
    {
        public void RunInherit()
        {
            IEnumerable<MutexPluginCategoryAttribute> attrubutes =
                from data in typeof(DemoInheritMutexPluginA).GetCustomAttributes()
                where data is MutexPluginCategoryAttribute
                select (MutexPluginCategoryAttribute)data;

            foreach (var item in attrubutes)
            {
                Console.WriteLine(item.Category);
            }
        }
    }
    
    [MutexPluginCategoryAttribute("互斥类1")]
    class DemoMutexPluginA : ILoadable, IUnloadable
    {
        public bool Load()
        {
            return true;
        }

        public bool Unload()
        {
            return true;
        }
    }
    
    class DemoInheritMutexPluginA : DemoMutexPluginA
    {

    }
}
