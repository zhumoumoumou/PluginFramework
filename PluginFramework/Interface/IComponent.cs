using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Interface
{
    /// <summary>
    /// 表示一个部件。
    /// </summary>
    public interface IComponent : IDisposable
    {
        /// <summary>
        /// 插件的友好名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 插件的详细描述。
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 插件是否处于启用状态。
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 公开的子模块清单。
        /// </summary>
        IEnumerable<IComponent> Chlidren { get; }
    }

}
