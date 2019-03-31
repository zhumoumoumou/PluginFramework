using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginFramework.Interface;

namespace PluginFramework
{
    /// <summary>
    /// IComponent的典型实现。
    /// </summary>
    class Component : MarshalByRefObject, IComponent
    {
        public string Name => null;

        public string Description => null;

        public bool IsEnabled { get; set; }

        public IEnumerable<IComponent> Children => null;

        public void Dispose()
        {

        }
    }
}
