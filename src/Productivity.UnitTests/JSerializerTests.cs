using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace Productivity.UnitTests
{
    [TestFixture]
    public class JSerializerTests
    {
        [Test]
        public void ToJson_ValidObject_ReturnsJson()
        {
            var item = Helper.GetSimpleFake();

            string result = JSerializer.ToJson(item);

            Assert.IsTrue(result.Length > 0);
        }

        public void ToJson_ValidComplexObject_ReturnsJson()
        {
            var item = Helper.GetComplexFake();

            string result = JSerializer.ToJson(item);

            Assert.IsTrue(result.Length > 0);
        }

        [Test]
        public void ToJson_InvalidObject_Throws()
        {
            SimpleFake fake = null;

            Assert.Throws<ArgumentException>(() => JSerializer.ToJson(fake));
        }

        [Test]
        public void FromJson_ValidJson_ReturnsObject()
        {
            var item = Helper.GetSimpleFake();
            string json = JSerializer.ToJson(item);

            var fromJsonObj = JSerializer.FromJson<SimpleFake>(json);

            Assert.True(fromJsonObj != null);
        }

        [Test]
        public void FromJson_ValidComplexJson_ReturnsObject()
        {
            var item = Helper.GetComplexFake();
            string json = JSerializer.ToJson(item);

            var fromJsonObj = JSerializer.FromJson<ComplexFake>(json);

            Assert.True(fromJsonObj != null);
        }

        [Test]
        public void FromJson_InValidJson_Throws()
        {
            string json = "this is not json";

            Assert.Throws<JsonReaderException>(() => JSerializer.FromJson<SimpleFake>(json));
        }

        [Test]
        public void FromJson_Null_Throws()
        {
            string json = null;

            Assert.Throws<ArgumentException>(() => JSerializer.FromJson<SimpleFake>(json));
        }
    }
}