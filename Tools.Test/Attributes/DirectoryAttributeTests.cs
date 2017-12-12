using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.Attributes;

namespace Tools.Test.Attributes
{
    [TestClass]
    public class DirectoryAttributeTests
    {
        [TestMethod()]
        public void Directory_Test()
        {
            var dirAttribute = new DirectoryAttribute();
            bool valid = dirAttribute.IsValid(@"C:\Test\Test");
            Assert.IsTrue(valid);
        }
    }
}