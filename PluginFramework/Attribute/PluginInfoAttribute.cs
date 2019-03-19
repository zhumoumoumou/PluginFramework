using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 插件相关的属性。
/// </summary>
namespace PluginFramework.Attribute
{
    /// <summary>
    /// 插件基础信息。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class PluginInfoAttribute : System.Attribute
    {
        public PluginInfoAttribute(string friendlyName)
        {
            this.FriendlyName = friendlyName;
        }

        /// <summary>
        /// 该插件的友好名称。
        /// </summary>
        public string FriendlyName { get; }

        /// <summary>
        /// 该插件的描述。
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 插件的类型信息，
    /// 该类的所有子类都会拥有该属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    sealed class PluginCategoryAttribute : System.Attribute
    {
        public PluginCategoryAttribute(string category)
        {
            this.Category = category;
        }
        
        public string Category { get; }

        /// <summary>
        /// 代表该类目是否为互斥类目，
        /// 即是否仅允许声明该类的某一个实例。
        /// </summary>
        public bool IsMutex { get; set; } = false;
    }
}
