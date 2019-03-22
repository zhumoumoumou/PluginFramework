﻿using PluginFramework.Attribute;
using PluginFramework.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin2
{
    [PluginInfo("TestPluginClass3", Description = "示例计数器，其将计算指定事件的发生次数。")]
    [PluginCategory("示例", IsMutex = false)]
    [PluginCategory("测试用")]
    [PluginCategory("测试类目A独占", IsMutex = true)]
    public class TestPluginClass3 : IManageable, ILoadable, IUnloadable
    {
        public EventHandler TargetHandler { get; set; }

        public event EventHandler OnCounterFire;

        public int Count { get; private set; }
        public bool IsEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Load()
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

        public bool Unload()
        {
            TargetHandler -= OnCounterFire;
            return true;
        }

        public TestPluginClass3()
        {
            OnCounterFire += (obj, e) => { this.Count += 1; };
        }
    }

    [PluginInfo("TestPluginClass4", Description = "示例计数器，其将计算指定事件的发生次数。")]
    [PluginCategory("示例", IsMutex = false)]
    [PluginCategory("测试用")]
    [PluginCategory("测试类目B独占", IsMutex = true)]
    public class TestPluginClass4 : IManageable, ILoadable, IUnloadable
    {
        public EventHandler TargetHandler { get; set; }

        public event EventHandler OnCounterFire;

        public int Count { get; private set; }
        public bool IsEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Load()
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

        public bool Unload()
        {
            TargetHandler -= OnCounterFire;
            return true;
        }

        public TestPluginClass4()
        {
            OnCounterFire += (obj, e) => { this.Count += 1; };
        }
    }
}