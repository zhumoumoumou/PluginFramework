using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Interface
{
    /// <summary>
    /// 表示插件加载入口的接口。
    /// </summary>
    public interface IExtension : IComponent
    {
        /// <summary>
        /// 插件加载方法。
        /// </summary>
        /// <returns>若执行成功返回true，否则返回false。</returns>
        bool Attach();

        /// <summary>
        /// 插件卸载方法。
        /// </summary>
        /// <returns>若执行成功返回true，否则返回false。</returns>
        bool Detach();
    }
}
