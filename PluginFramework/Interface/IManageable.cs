using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Interface
{
    /// <summary>
    /// 表示插件是否受管理的接口。
    /// </summary>
    public interface IManageable : INotifyPropertyChanged, ISite
    {
        /// <summary>
        /// 表明插件当前是否已启用。
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
