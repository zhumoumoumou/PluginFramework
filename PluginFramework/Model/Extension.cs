using PluginFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Model
{
    /// <summary>
    /// 表示插件加载入口的接口。
    /// </summary>
    public abstract class Extension : Component, IExtension
    {
        /// <summary>
        /// 插件加载方法。请注意修改<see cref="Component.IsEnabled"/>，否则将导致插件反复加载。
        /// </summary>
        /// <returns>若执行成功返回true，否则返回false。</returns>
        public abstract bool Attach();

        /// <summary>
        /// 插件卸载方法。请注意修改<see cref="Component.IsEnabled"/>。
        /// </summary>
        /// <returns>若执行成功返回true，否则返回false。</returns>
        public abstract bool Detach();

        /// <summary>
        /// 采用部件名称字符串初始化部件。
        /// </summary>
        /// <param name="name">插件名称</param>
        public Extension(string name) : base(name) { }

        /// <summary>
        /// 非托管资源释放。当<see cref="Component.IsEnabled"/>为真将首先调用一次<see cref="Detach"/>。
        /// </summary>
        public override void Dispose()
        {
            if (IsEnabled)
            {
                Detach();
            }
        }
    }
}
