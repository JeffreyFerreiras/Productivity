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
            var assemblyInstance = AssemblyHelper.LoadNETAssembly(assemblyName, "Tools.InstanceClass");

            string s = assemblyInstance.Next(10);

            Assert.IsTrue(s.Length > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadNETAssembly_NullArgs()
        {
            var assemblyInstance = AssemblyHelper.LoadNETAssembly(null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadNETAssembly_EmptyArgs()
        {
            var assemblyInstance = AssemblyHelper.LoadNETAssembly("", "");
        }
    }
}
