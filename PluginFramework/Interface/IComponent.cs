using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Interface
{
    /// <summary>
    /// 表示一个部件的接口。一般情况下不直接实现该接口而是继承<see cref="Model.Component"/>类。
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// 插件的友好名称。
        /// </summary>
        string Name { get;}

        /// <summary>
        /// 插件的详细描述。
        /// </summary>
        string Description{ get; }

        /// <summary>
        /// 插件是否处于启用状态。此项务必赋以初值，因为它决定
        /// <see cref="IExtension"/>接口在用户进行激活/解除激活操作时
        /// 调用Detach还是Attach。
        /// 它同时还与界面显示有关。
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// 公开的子模块清单。
        /// </summary>
        System.Collections.ObjectModel.ObservableCollection<IComponent> Children
        {
            get;
        }
    }
}
