using NUnit.Framework;
using Productivity.Attributes;
using System;

namespace Productivity.UnitTests.Attributes
{
    [TestFixture]
    public class PathAttributeTests
    {
        [Test]
        public void Path_ValidDirectory_ReturnsTrue()
        {
            var attr = new PathAttribute();
            bool valid = attr.IsValid(@"C:\Test\Test");

            Assert.IsTrue(valid);
        }

        [Test]
        public void Path_ValidNetworkDirectory_ReturnsTrue()
        {
            var attr = new PathAttribute();
            bool valid = attr.IsValid(@"\\D076CTS0037235\Test\Test");

            Assert.IsTrue(valid);
        }

        [Test]
        public void Path_ValidIPDirectory_ReturnsTrue()
        {
            var attr = new PathAttribute();
            bool valid = attr.IsValid(@"\\10.7.52.124\Test\Test");

            Assert.IsTrue(valid);
        }

        [Test]
        public void Path_InvalidArgs_Returnsfalse()
        {
            var attr = new PathAttribute();

            Assert.IsFalse(attr.IsValid(string.Empty));
            Assert.IsFalse(attr.IsValid(null));
        }

        [Test]
        public void Path_InvalidType_ThrowsException()
        {
            var attr = new PathAttribute();

            Assert.Throws<ArgumentException>(() => attr.IsValid(1));
        }
    }
}