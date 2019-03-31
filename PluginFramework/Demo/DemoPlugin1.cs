using PluginFramework.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginFramework.Demo
{

    class Counter :  IExtension
    {
        public EventHandler TargetHandler { get; set; }

        public event EventHandler OnCounterFire;

        public int Count { get; private set; }
        public bool IsEnabled { get; set; }

        public string Name { get; } = "示例计数器";

        public string Description { get; } = "这是一个示例计数器。";

        public IEnumerable<IComponent> Chlidren => null;

        public bool Attach()
        {
            return true;
        }

        public void ResetCounter() { Count = 0; }

        public void AssignToNewEventHandler(EventHandler handler)
        {
            this.TargetHandler -= handler;
            ResetCounter();
            handler += OnCounterFire;
            this.TargetHandler = handler;
        }

        public bool Detach()
        {
            TargetHandler -= OnCounterFire;
            return true;
        }

        public void Dispose()
        {

        }

        public Counter()
        {
            OnCounterFire += (obj, e) => { this.Count += 1; };
        }
    }
}
