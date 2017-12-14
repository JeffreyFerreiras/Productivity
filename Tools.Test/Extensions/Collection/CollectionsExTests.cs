using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions.Validation;

namespace Tools.Test.Extensions
{
    using System.Collections;
    using Tools.Extensions.Collection;

    [TestClass]
    public class CollectionsExTests
    {
        [TestMethod]
        public void GetRandomElement_ValidCollection_ReturnsRandomElement()
        {
            string[] stringArry = Helper.GeStringArray(20);
            string elem = stringArry.GetRandomElement();

            Assert.IsTrue(stringArry.Contains(elem));
        }

        [TestMethod]
        public void GetRandomElement_InValidCollection_ReturnsDefaultValue()
        {
            string[] stringArry = null;

            Assert.ThrowsException<ArgumentException>(() => stringArry.GetRandomElement());
        }

        [TestMethod]
        public void FromDictionary_EmptyObject_PopulateObject()
        {
            var item = new FakeTest
            {
                A = null,
                B = null,
                C = null,
                D = null,
                E = null
            };

            IDictionary<string, object> itemDictionary =
                new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
            {
                { "a", "a value" },
                { "b", 5},
                { "c", 11.11},
                { "d", new int[] { 1, 2, 3, 4, 5 }},
                { "e", "Hello world!".ToCharArray()}
            };

            object o = itemDictionary.FromDictionary(item);

            Assert.IsTrue(o.ValidateProperties());
        }

        [TestMethod]
        public void FromDictionary_PopulatedMultipleTypeProperties_PopulatesObject()
        {
            var item = new FakeMultiType
            {
                A = "dddd",
                B = 1.55,
                C = DateTime.Now,
                D = Suits.Club
            };

            IDictionary<string, object> itemDictionary =
                new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
            {
                { "a", "a value" },
                { "b", 5.55},
                { "c", DateTime.Now},
                { "d", Suits.Club }
            };
            
            object o = itemDictionary.FromDictionary(item);

            Assert.IsTrue(item.A.Equals(itemDictionary["a"]));
            Assert.IsTrue(item.B.Equals(itemDictionary["b"]));
            Assert.IsTrue(item.C.Equals(itemDictionary["c"]));
        }

        [TestMethod]
        public void FromDictionary_MismatchedTypes_PopulatesObject()
        {
            var item = new FakeMultiType
            {
                A = "dddd",
                B = 1.55,
                C = DateTime.Now,
                D = Suits.Club
            };

            IDictionary<string, object> itemDictionary =
                new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
            {
                { "a", (object)"a value" },
                { "b", 5.55m},
                { "c", DateTime.Now.ToString()},
                { "d", Suits.Club }
            };

            object o = itemDictionary.FromDictionary(item);

            Assert.IsTrue(item.A == (string)itemDictionary["a"]);
            Assert.IsTrue(item.B == Convert.ToDouble(itemDictionary["b"]));
            Assert.IsTrue(item.C == Convert.ToDateTime(itemDictionary["c"]));
            Assert.IsTrue(item.D == (Suits)itemDictionary["d"]);
        }

        [TestMethod]
        public void TryGetValue_ValidDictANDIndex_IndexValue()
        {
            var dict = new Dictionary<string, string>();

            string key = "key";
            string value = "value";

            dict.Add(key, value);

            Assert.AreEqual(value, dict.TryGetValue(key));
        }

        [TestMethod]
        public void TryGetValue_InvalidKey_ThrowsArgumentException()
        {
            var dict = new Dictionary<string, string>();
            const string key = "key";
            const string value = "value";
            dict.Add(key, value);

            Assert.ThrowsException<ArgumentException>(() => dict.TryGetValue(""));
            Assert.ThrowsException<ArgumentException>(() => dict.TryGetValue(null));
        }

        [TestMethod]
        public void TryGetValue_NonExistingKey_DefaultValue()
        {
            var dict = new Dictionary<string, string>
            {
                { "key", "value" }
            };

            string defaultValue = dict.TryGetValue("DoesnotExist");

            Assert.AreEqual(default(string), defaultValue);
        }
    }
}