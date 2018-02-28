using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.Attributes;

namespace Tools.Test.Attributes
{
    [TestClass]
    public class DirectoryAttributeTests
    {
        [TestMethod()]
        public void Directory_ValidDirectory_ReturnsTrue()
        {
            var attr = new DirectoryAttribute();
            bool valid = attr.IsValid(@"C:\Test\Test");

            Assert.IsTrue(valid);
        }

        [TestMethod()]
        public void Directory_ValidNetworkDirectory_ReturnsTrue()
        {
            var attr = new DirectoryAttribute();
            bool valid = attr.IsValid(@"\\D076CTS0037235\Test\Test");

            Assert.IsTrue(valid);
        }

        [TestMethod()]
        public void Directory_ValidIPDirectory_ReturnsTrue()
        {
            var attr = new DirectoryAttribute();
            bool valid = attr.IsValid(@"\\10.7.52.124\Test\Test");

            Assert.IsTrue(valid);
        }

        [TestMethod()]
        public void Directory_InvalidArgs_Returnsfalse()
        {
            var attr = new DirectoryAttribute();
            
            Assert.IsFalse(attr.IsValid(string.Empty));
            Assert.IsFalse(attr.IsValid(null));
        }
    }
}