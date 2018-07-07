using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Configuration;

namespace Tools.Test.Configuration
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
            Assert.Throws<InvalidOperationException>(()=>ConfigReader.GetConnectionString("SLKDJHJGLKH"));
        }
    }
}
