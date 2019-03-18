using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Interface
{
    /// <summary>
    /// 表示某个插件可以卸载的接口。
    /// </summary>
    interface IUnloadable
    {
        bool Unload();
    }
}
