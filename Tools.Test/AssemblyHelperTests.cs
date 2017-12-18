using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tools.Test
{
    [TestClass]
    public class AssemblyHelperTests
    {
        [TestMethod]
        public void LoadNETAssembly_ValidArgs_CreatesDynamicObject()
        {
            string assemblyName = this.GetType().GetTypeInfo().Assembly.Location;
            dynamic assemblyInstance = AssemblyHelper.LoadNETAssembly(assemblyName, "Tools.InstanceClass");

            string s = assemblyInstance.Next(10);

            Assert.IsTrue(s.Length > 0);
        }

        [TestMethod]
        public void LoadNETAssembly_NullArgs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => AssemblyHelper.LoadNETAssembly(null, null));
        }

        [TestMethod]
        public void LoadNETAssembly_EmptyStringArgs_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => AssemblyHelper.LoadNETAssembly("", ""));
        }
    }
}