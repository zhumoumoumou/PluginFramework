using PluginFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework
{
    class DemoPlugin : ILoadable, IUnloadable
    {
        public bool Load()
        {
            throw new NotImplementedException();
        }

        public bool Unload()
        {
            throw new NotImplementedException();
        }
    }

}
