using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;

namespace Tools.Test
{
    [TestFixture]
    public class JSerializerTests
    {
        [Test]
        public void ToJson_ValidInput_ReturnsJson()
        {
            object item = new
            {
                Name = "Jeffrey",
                Last = "Ferreiras",
                Age =  28
            };


            string result = JSerializer.ToJson(item);

            Assert.IsTrue(result.Length > 0);
        }
    }
}
