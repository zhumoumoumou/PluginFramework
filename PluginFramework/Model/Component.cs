using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using PluginFramework.Interface;

namespace PluginFramework.Model
{
    /// <summary>
    /// 表示一个部件。它提供了<see cref="IComponent"/>接口的基础实现。
    /// </summary>
    public abstract class Component : MarshalByRefObject, IDisposable, System.ComponentModel.INotifyPropertyChanged, IComponent
    {
        private string name;
        /// <summary>
        /// 插件的友好名称。
        /// <see cref="Name"/>为空时在GUI插件树界面将很难识别到该插件
        /// ，因此不建议令<see cref="Name"/>为空。
        /// </summary>
        public string Name
        {
            get { return name; }
            protected set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        private string description;
        /// <summary>
        /// 插件的详细描述。
        /// </summary>
        public string Description
        {
            get { return description; }
            protected set
            {
                if (description != value)
                {
                    description = value; RaisePropertyChanged("Description");
                }
            }
        }
        
        private bool isEnabled = false;
        /// <summary>
        /// 插件是否处于启用状态。此项务必赋以初值，因为它决定
        /// <see cref="Extension"/>接口在用户进行激活/解除激活操作时
        /// 调用Detach还是Attach。
        /// 它同时还与界面显示有关。
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            protected set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    RaisePropertyChanged("IsEnabled");
                }
            }
        }
        
        private ObservableCollection<IComponent> children;
        /// <summary>
        /// 公开的子模块清单。
        /// </summary>
        public ObservableCollection<IComponent> Children { get { return children; } protected set { children = value; RaisePropertyChanged("Children"); } }

        /// <summary>
        /// 采用部件名称字符串初始化部件。
        /// </summary>
        /// <param name="name">插件名称</param>
        public Component(string name)
        {
            this.Name = name;
            Children = new ObservableCollection<IComponent>();
        }

        /// <summary>
        /// 跳过对<see cref="Name"/>的初始化，直接初始化一个<see cref="Component"/>类。
        /// </summary>
        protected Component()
        {
            Children = new ObservableCollection<IComponent>();
        }

        /// <summary>
        /// <see cref="System.ComponentModel.INotifyPropertyChanged"/>接口定义的事件。
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发<see cref="PropertyChanged"/>事件，通知客户端属性已修改。
        /// </summary>
        /// <param name="prop"></param>
        protected virtual void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// 释放<see cref="Component"/>的非托管资源。
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// 获取插件名称与描述的组合字符串。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + " " + Description;
        }
    }
}
