using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using PluginFramework.Interface;
using PluginFramework.Attribute;

namespace Tests
{
    [PluginInfo("示例计数器", Description = "示例计数器，其将计算指定事件的发生次数。")]
    [PluginCategory("示例", IsMutex = false)]
    [PluginCategory("测试用")]
    [PluginCategory("测试类目A独占", IsMutex = true)]
    class TestPluginClass1 : Component, IExtension
    {
        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler Disposed;

        public bool Attach()
        {
            throw new NotImplementedException();
        }

        public bool Detach()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [PluginInfo("示例计数器", Description = "示例计数器，其将计算指定事件的发生次数。")]
    [PluginCategory("示例", IsMutex = false)]
    [PluginCategory("测试用")]
    [PluginCategory("测试类目B独占", IsMutex = true)]
    class TestPluginClass2 : IComponent, IExtension
    {
        public ISite Site { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler Disposed;

        public bool Attach()
        {
            throw new NotImplementedException();
        }

        public bool Detach()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
