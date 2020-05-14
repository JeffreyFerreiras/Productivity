using Common.Tools.Attributes;
using NUnit.Framework;

namespace Common.Tools.Test.Attributes
{
    [TestFixture]
    public class IpAddressAttributeTests
    {
        [Test]
        public void IsValid_ValidIp_ReturnsTrue()
        {
            var attr = new IpAddressAttribute();

            Assert.True(attr.IsValid("127.0.0.1"));
        }

        [Test]
        public void IsValid_InValidIp_ReturnsFalse()
        {
            var attr = new IpAddressAttribute();

            Assert.False(attr.IsValid("127..0.1"));
        }
    }
}