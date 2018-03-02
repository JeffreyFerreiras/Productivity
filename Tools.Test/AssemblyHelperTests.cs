using NUnit.Framework;
using System;
using System.Reflection;

namespace Tools.Test
{
    [TestFixture]
    public class AssemblyHelperTests
    {
        [Test]
        public void LoadNETAssembly_ValidArgs_CreatesDynamicObject()
        {
            string assemblyName = this.GetType().GetTypeInfo().Assembly.Location;
            dynamic assemblyInstance = AssemblyHelper.LoadNETAssembly(assemblyName, "Tools.InstanceClass");

            string s = assemblyInstance.Next(10);

            Assert.IsTrue(s.Length > 0);
        }

        [Test]
        public void LoadNETAssembly_NullArgs_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AssemblyHelper.LoadNETAssembly(null, null));
        }

        [Test]
        public void LoadNETAssembly_EmptyStringArgs_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => AssemblyHelper.LoadNETAssembly("", ""));
        }
    }
}