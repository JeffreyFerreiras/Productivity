using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Tools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Tools.Test.Extensions
{
    [TestClass]
    public class CollectionsExTests
    {
        public class FakeTest
        {
            public object a { get; set; }
            public object b { get; set; }
            public object c { get; set; }
            public object d { get; set; }
            public object e { get; set; }
        }

        [TestMethod]
        public void GetRandomElement_ValidCollection_ReturnsRandomElement()
        {
            string[] stringArry = GetFakeStringArray(20);

            string elem = stringArry.GetRandomElement();

            Assert.IsTrue(stringArry.Contains(elem));
        }

        private static string[] GetFakeStringArray(int length = 10)
        {
            string[] stringArray = new string[length];

            for (int i = 0; i < length; i++)
            {
                stringArray[i] = RandomString.NextAlphabet();
            }

            return stringArray;
        }

        [TestMethod]
        public void ToDictionary_ConvertObject_IDictionary()
        {
            var item = new
            {
                a = "Hellon world!",
                b = 5,
                c = 11.11,
                d = new int[] { 1, 2, 3, 4, 5 },
                e = "Hello world!".ToCharArray()
            };

            IDictionary<string, object> itemDictionary;
            itemDictionary = item.ToDictionary();

            Assert.IsTrue(itemDictionary.Count > 0);
        }

        [TestMethod]
        public void FromDictionary_EmptyObject_PopulateObject()
        {
            var item = new FakeTest
            {
                a = null,
                b = null,
                c = null,
                d = null,
                e = null
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
        public void TryGetValue_ValidDictANDIndex_IndexValue()
        {
            var dict = new Dictionary<string, string>();
            const string key = "key";
            const string value = "value";
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

           
            Assert.ThrowsException<ArgumentException>(()=> dict.TryGetValue(""));
            Assert.ThrowsException<ArgumentException>(() => dict.TryGetValue(null));
        }

        [TestMethod]
        public void TryGetValue_NonExistingKey_DefaultValue()
        {
            var dict = new Dictionary<string, string>();
            const string key = "key";
            const string value = "value";
            dict.Add(key, value);

            string defaultValue = dict.TryGetValue("DoesnotExist");

            Assert.AreEqual(default(string), defaultValue);
        }
    }
}
