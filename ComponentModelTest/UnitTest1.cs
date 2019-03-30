using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComponentModelTest
{
    [TestClass]
    public class UnitTest1
    {
        Component component;
        Component childComponent1, childComponent2;
        Container container;

        [TestInitialize]
        public void TestInit()
        {

        }
        
        [TestMethod]
        public void TestMethod1()
        {
            component = new Component();
            childComponent1 = new Component();
            childComponent2 = new Component();
            container = new Container();
            container.Add(childComponent1);
        }
    }
}
