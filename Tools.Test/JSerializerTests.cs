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
            var item = Helper.GetSimpleFake();

            string result = JSerializer.ToJson(item);

            Assert.IsTrue(result.Length > 0);
        }
    }
}
