using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions.Validation;

namespace Tools.Test.Extensions
{
    using Tools.Extensions.Collection;
    using Tools.Extensions.Conversion;

    [TestClass]
    public class CollectionsExTests
    {
        [TestMethod]
        public void ToDictionary_EmptyString_ReturnsEmptyDictionary()
        {
            IDictionary<string, string> dict = "".ToDictionary();

            Assert.IsTrue(dict.Count == 0);
        }

        [TestMethod]
        public void ToDictionary_Null_ReturnsEmptyDictionary()
        {
            string pair = null;

            IDictionary<string, string> dict = pair.ToDictionary();

            Assert.IsTrue(dict.Count == 0);
        }

        [TestMethod]
        public void ToDictionary_KeyValuePairString_ReturnsDictionary()
        {
            string pair = "key=value;key2=value2;key3=value3";

            IDictionary<string, string> dict = pair.ToDictionary();

            Assert.AreEqual(dict["key"], "value");
            Assert.AreEqual(dict["key2"], "value2");
            Assert.AreEqual(dict["key3"], "value3");
        }

        [TestMethod]
        public void SubSequence_HighLength_ThrowsIndexOutOfRangeException()
        {
            var collection = Helper.GeStringArray();
            int len = collection.Count() + 5;

            Action action = () => collection.SubSequence(3, len);
            Assert.ThrowsException<IndexOutOfRangeException>(action);
        }

        [TestMethod]
        public void SubSequence_HighStart_ThrowsIndexOutOfRangeException()
        {
            var collection = Helper.GeStringArray();

            Assert.ThrowsException<IndexOutOfRangeException>(() => collection.SubSequence(collection.Count() + 2));
        }

        [TestMethod]
        public void SubSequence_InvalidRange_ThrowsArgumentException()
        {
            var collection = Helper.GeStringArray();
            int start = 8;
            int len = 7;

            Assert.ThrowsException<ArgumentException>(() => collection.SubSequence(start, len));
        }

        [TestMethod]
        public void SubSequence_Array_ReturnsSubSequence()
        {
            int startIndex = 4;
            var collection = Helper.GeStringArray();
            var sub = collection.SubSequence(startIndex);

            for(int i = 0; i < sub.Count(); i++)
            {
                string colVal = collection.ElementAt(i + startIndex);
                string subVal = sub.ElementAt(i);

                Assert.AreEqual(colVal, subVal);
            }
        }

        [TestMethod]
        public void SubSequence_ReferenceTypeList_ReturnsSubSequence()
        {
            int startIndex = 4;
            var collection = Helper.GetSimpleFakes();
            var sub = collection.SubSequence(startIndex);

            for(int i = 0; i < sub.Count(); i++)
            {
                var colVal = collection.ElementAt(i + startIndex);
                var subVal = sub.ElementAt(i);

                Assert.AreEqual(colVal, subVal);
            }
        }

        [TestMethod]
        public void SubSequence_IEnumerableTypeList_ReturnsSubSequence()
        {
            int startIndex = 4;
            IEnumerable<SimpleFake> collection = Helper.GetSimpleFakes();
            var sub = collection.SubSequence(startIndex);

            for(int i = 0; i < sub.Count(); i++)
            {
                var colVal = collection.ElementAt(i + startIndex);
                var subVal = sub.ElementAt(i);

                Assert.AreEqual(colVal, subVal);
            }
        }

        [TestMethod]
        public void GetUnderlyingType_IEnumerableString_ReturnsStringType()
        {
            IEnumerable<string> collection = new List<string>
            {
                "Hello World!",
                "stuff",
            };

            Type underlyingType = collection.GetUnderlyingType();
            Assert.AreEqual(underlyingType, typeof(string));
        }

        [TestMethod]
        public void GetUnderlyingType_Primitive_ReturnsPrimitiveType()
        {
            IEnumerable<int> collection = new List<int>
            {
                5,
                50,
            };

            Type underlyingType = collection.GetUnderlyingType();
            Assert.AreEqual(underlyingType, typeof(int));
        }

        [TestMethod]
        public void GetUnderlyingType_CustomReferenceType_ReturnsType()
        {
            var collection = new SimpleFake[]
            {
                Helper.GetSimpleFake(),
                Helper.GetSimpleFake()
            };

            Type underlyingType = collection.GetUnderlyingType();
            Assert.AreEqual(underlyingType, typeof(SimpleFake));
        }

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
            var item = new SimpleFake
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
            var item = Helper.GetComplexFake();

            IDictionary<string, object> itemDictionary =
                new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
            {
                { "StringType", "a value" },
                { "DoubleType", 5.55},
                { "DateType", DateTime.Now},
                { "EnumType", Suits.Club }
            };

            object o = itemDictionary.FromDictionary(item);

            Assert.IsTrue(item.StringType.Equals(itemDictionary["StringType"]));
            Assert.IsTrue(item.DoubleType.Equals(itemDictionary["DoubleType"]));
            Assert.IsTrue(item.DateType.Equals(itemDictionary["DateType"]));
            Assert.IsTrue(item.EnumType.Equals(itemDictionary["EnumType"]));
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