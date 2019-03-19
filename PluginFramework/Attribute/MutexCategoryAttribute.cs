using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Attribute
{
    /// <summary>
    /// 表示互斥的插件类型。
    /// 同一个类可拥有多个该属性。
    /// 该类的所有子类都会拥有该属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class MutexPluginCategoryAttribute : System.Attribute
    {
        public MutexPluginCategoryAttribute(string category)
        {
            this.Category = category;

            // TODO: Implement code here
            
        }

        /// <summary>
        /// 作为互斥的键值。
        /// </summary>
        public string Category { get; }
    }

    /// <summary>
    /// 插件基础信息。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class PluginInfoAttribute : System.Attribute
    {
        // This is a positional argument
        public PluginInfoAttribute(string friendlyName)
        {
            this.FriendlyName = friendlyName;
        }

        public string FriendlyName { get; }

        public Version Version { get; set; }

        public string Description { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    sealed class PluginCategoryAttribute : System.Attribute
    {
        public PluginCategoryAttribute(string category)
        {
            this.Category = category;
        }
        
        public string Category { get; }
    }
}
