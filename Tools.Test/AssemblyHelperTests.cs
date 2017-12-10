using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Tools.Test
{
    [TestClass]
    public class AssemblyHelperTests
    { 
        [TestMethod]
        public void LoadNETAssembly_Test()
        {
            string assemblyName = this.GetType().GetTypeInfo().Assembly.Location;
            dynamic assemblyInstance = AssemblyHelper.LoadNETAssembly(assemblyName, "Tools.InstanceClass");

            string s = assemblyInstance.Next(10);

            Assert.IsTrue(s.Length > 0);
        }

        [TestMethod]
        public void LoadNETAssembly_NullArgs()
        {
            Assert.ThrowsException<ArgumentException> (()=>AssemblyHelper.LoadNETAssembly(null, null));
        }

        [TestMethod]
        public void LoadNETAssembly_EmptyArgs()
        {
            Assert.ThrowsException<ArgumentException>(() => AssemblyHelper.LoadNETAssembly("", ""));
        }
    }
}
