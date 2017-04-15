using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tools.Test
{
    [TestClass]
    public class AssemblyHelperTests
    { 
        [TestMethod]
        public void LoadNETAssembly_Test()
        {
            string assemblyName = @"C:\Projects\Git\Tools.NET - Core\Tools\bin\Debug\netstandard1.6\Tools.dll";
            var assemblyInstance = AssemblyHelper.LoadNETAssembly(assemblyName, "Tools.InstanceClass");

            string s = assemblyInstance.Next(10);

            Assert.IsTrue(s.Length > 0);
        }
    }
}
