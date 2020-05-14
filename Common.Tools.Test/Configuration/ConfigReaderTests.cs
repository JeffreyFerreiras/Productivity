using Common.Tools.Configuration;
using NUnit.Framework;
using System;

namespace Common.Tools.Test.Configuration
{
    [TestFixture]
    public class ConfigReaderTests
    {
        [Test]
        public void GetConnectionString_ValidConnString_ReturnsConnString()
        {
            string connString = ConfigReader.GetConnectionString("OfficeGames");

            Assert.IsNotNull(connString);
        }

        [Test]
        public void GetConnectionString_InvalidConnString_ThrowsInvalidOperation()
        {
            Assert.Throws<InvalidOperationException>(() => ConfigReader.GetConnectionString("SLKDJHJGLKH"));
        }
    }
}