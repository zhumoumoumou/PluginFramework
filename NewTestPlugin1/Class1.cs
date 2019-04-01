using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PluginFramework.Interface;

namespace NewTestPlugin1
{
    public class Concat : IExtension
    {
        public string Name => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public bool IsEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObservableCollection<IComponent> Children => throw new NotImplementedException();

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

    public class LogAppender : IComponent
    {
        public string Name { get; } = "日志追加模块";

        public string Description { get; } = "用于当前插件的日志追加。";

        public bool IsEnabled { get; private set; }

        public ObservableCollection<IComponent> Children => null;

        

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
