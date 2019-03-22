using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginFramework.Manager;

namespace Tests
{
    [TestClass]
    public class ManagedPluginItemTest
    {
        ManagedPluginItem item1 = ManagedPluginItem.CreateManagedPluginItem(typeof(TestPluginClass1));
        ManagedPluginItem item2 = ManagedPluginItem.CreateManagedPluginItem(typeof(TestPluginClass1));
        ManagedPluginItem item3 = ManagedPluginItem.CreateManagedPluginItem(typeof(TestPluginClass2));

        /// <summary>
        /// 对静态方法CreateManagedPluginItem的测试。
        /// </summary>
        [TestMethod]
        public void CreateManagedPluginItemTest()
        {
            Console.WriteLine(item1.FirendlyName);
            Console.WriteLine(item1.Description);
            foreach (var category in item1.Category)
            {
                Console.WriteLine(category);
            }
            foreach (var category in item1.MutexCategory)
            {
                Console.WriteLine(category);
            }
            Console.WriteLine();
        }

        [TestMethod]
        public void CanPluginInsertTest()
        {
            IEnumerable<string> regMutex = new List<string>();
            regMutex = item1.MutexCategory;

            Console.WriteLine(ManagedPluginItem.CanPluginInsert(regMutex, item2));
            Console.WriteLine(ManagedPluginItem.CanPluginInsert(regMutex, item3));

            Console.WriteLine();
        }

        [TestMethod]
        public void IsTwoItemMutexTest()
        {
            Console.WriteLine(ManagedPluginItem.IsTwoItemMutex(item1, item2));
            Console.WriteLine(ManagedPluginItem.IsTwoItemMutex(item1, item3));

            Console.WriteLine();
        }
    }
}
