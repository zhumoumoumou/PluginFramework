using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework
{
    class BasePlugin : IRegistable
    {
        #region Metadata
        public Version Version { get; set; }
        public bool IsEnabled { get; set; }
        #endregion





        public bool AfterRetist()
        {
            throw new NotImplementedException();
        }

        public bool BeginRegist()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
