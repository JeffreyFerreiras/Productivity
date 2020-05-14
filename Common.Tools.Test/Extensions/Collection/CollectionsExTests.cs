using Common.Tools.Extensions.Collection;
using Common.Tools.Extensions.Conversion;
using Common.Tools.Extensions.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Common.Tools.Test.Extensions
{
    [TestFixture]
    public class CollectionsExTests
    {
        [Test]
        public void ToDictionary_EmptyString_ReturnsEmptyDictionary()
        {
            IDictionary<string, string> dict = "".ToDictionary();

            Assert.IsTrue(dict.Count == 0);
        }

        [Test]
        public void ToDictionary_Null_ReturnsEmptyDictionary()
        {
            string pair = null;

            IDictionary<string, string> dict = pair.ToDictionary();

            Assert.IsTrue(dict.Count == 0);
        }

        [Test]
        public void ToDictionary_KeyValuePairString_ReturnsDictionary()
        {
            string pair = "key=value;key2=value2;key3=value3";

            IDictionary<string, string> dict = pair.ToDictionary();

            Assert.AreEqual(dict["key"], "value");
            Assert.AreEqual(dict["key2"], "value2");
            Assert.AreEqual(dict["key3"], "value3");
        }

        [Test]
        public void SubSequence_HighLength_ThrowsIndexOutOfRangeException()
        {
            var collection = Helper.GeStringArray();
            int len = collection.Count() + 5;

            Assert.Throws<IndexOutOfRangeException>(() => collection.SubSequence(3, len));
        }

        [Test]
        public void SubSequence_HighStart_ThrowsIndexOutOfRangeException()
        {
            var collection = Helper.GeStringArray();

            Assert.Throws<IndexOutOfRangeException>(() => collection.SubSequence(collection.Count() + 2));
        }

        [Test]
        public void SubSequence_InvalidRange_ThrowsArgumentException()
        {
            var collection = Helper.GeStringArray();
            int start = 8;
            int len = 7;

            Assert.Throws<ArgumentException>(() => collection.SubSequence(start, len));
        }

        [Test]
        public void SubSequence_Array_ReturnsSubSequence()
        {
            int startIndex = 4;
            var collection = Helper.GeStringArray();
            var sub = collection.SubSequence(startIndex);

            for (int i = 0; i < sub.Count(); i++)
            {
                string colVal = collection.ElementAt(i + startIndex);
                string subVal = sub.ElementAt(i);

                Assert.AreEqual(colVal, subVal);
            }
        }

        [Test]
        public void SubSequence_ReferenceTypeList_ReturnsSubSequence()
        {
            int startIndex = 4;
            var collection = Helper.GetSimpleFakes();
            var sub = collection.SubSequence(startIndex);

            for (int i = 0; i < sub.Count(); i++)
            {
                var colVal = collection.ElementAt(i + startIndex);
                var subVal = sub.ElementAt(i);

                Assert.AreEqual(colVal, subVal);
            }
        }

        [Test]
        public void SubSequence_IEnumerableTypeList_ReturnsSubSequence()
        {
            int startIndex = 4;
            IEnumerable<SimpleFake> collection = Helper.GetSimpleFakes();
            var sub = collection.SubSequence(startIndex);

            for (int i = 0; i < sub.Count(); i++)
            {
                var colVal = collection.ElementAt(i + startIndex);
                var subVal = sub.ElementAt(i);

                Assert.AreEqual(colVal, subVal);
            }
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
        public void GetRandomElement_ValidCollection_ReturnsRandomElement()
        {
            string[] stringArry = Helper.GeStringArray(20);
            string elem = stringArry.GetRandomElement();

            Assert.IsTrue(stringArry.Contains(elem));
        }

        [Test]
        public void GetRandomElement_InValidCollection_ReturnsDefaultValue()
        {
            string[] stringArry = null;

            Assert.Throws<ArgumentException>(() => stringArry.GetRandomElement());
        }

        [Test]
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

            Assert.IsTrue(o.HasValidProperties());
        }

        [Test]
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

        [Test]
        public void TryGetValue_ValidDictANDIndex_IndexValue()
        {
            var dict = new Dictionary<string, string>();

            string key = "key";
            string value = "value";

            dict.Add(key, value);

            Assert.AreEqual(value, dict.TryGetValue(key));
        }

        [Test]
        public void TryGetValue_InvalidKey_ThrowsArgumentException()
        {
            var dict = new Dictionary<string, string>();
            const string key = "key";
            const string value = "value";
            dict.Add(key, value);

            Assert.Throws<ArgumentException>(() => dict.TryGetValue(""));
            Assert.Throws<ArgumentException>(() => dict.TryGetValue(null));
        }

        [Test]
        public void TryGetValue_NonExistingKey_DefaultValue()
        {
            var dict = new Dictionary<string, string>
            {
                { "key", "value" }
            };

            string defaultValue = dict.TryGetValue("DoesnotExist");

            Assert.AreEqual(default(string), defaultValue);
        }

        [Test]
        public void Rotate_Ints_Rotates()
        {
            int[] input = { 1, 2, 3, 4, 5, 6 };
            int[] expected = { 4, 5, 6, 1, 2, 3 };

            input.Rotate(3);

            Assert.AreEqual(input, expected);
        }

        [Test]
        public void Rotate2_Ints_Rotates()
        {
            int[] input = { 1, 2, 3, 4, 5, 6 };
            int[] expected = { 4, 5, 6, 1, 2, 3 };

            input = input.Rotate(3).ToArray();

            Assert.AreEqual(input, expected);
        }

        [Test]
        public void Rotate_ListOfInts_Rotates()
        {
            var input = new List<int> { 1, 2, 3, 4, 5, 6 };
            var expected = new List<int> { 4, 5, 6, 1, 2, 3 };

            input = input.Rotate(3).ToList();

            Assert.AreEqual(input, expected);
        }

        [Test]
        public void Rotate_Performace_Test()
        {
            int[] input = Helper.GetRandomArray(10000);

            var sw = Stopwatch.StartNew();
            input.Rotate(5000);
            sw.Stop();
            string result = sw.ElapsedTicks.ToString();
            Debug.WriteLine($"Array: {sw.ElapsedTicks}");

            var input2 = input.ToList();
            sw.Restart();
            input2.Rotate(5000);
            sw.Stop();
            string result2 = sw.ElapsedTicks.ToString();
            Debug.WriteLine($"Linq: {sw.ElapsedTicks}");

            sw.Restart();
            input.Rotate(5000);
            sw.Stop();
            string result3 = sw.ElapsedTicks.ToString();
            Debug.WriteLine($"Array: {sw.ElapsedTicks}");
        }

        [Theory]
        [TestCase(5)]
        [TestCase(-8)]
        public void Rotate_OutofRange_ThrowsIndexOutOfRangeException(int n)
        {
            int[] input = { 1, 2, 3, 4, 5, 6 };

            Assert.Throws<IndexOutOfRangeException>(() => input.Rotate(input.Length + n));
        }

        [Test]
        public void Rotate_null_ThrowsArgumentException()
        {
            int[] input = null;

            Assert.Throws<ArgumentException>(() => input.Rotate(3));
        }

        [Test]
        public void Page_Strings_PagesWithIEnumerableOverload()
        {
            string[] words = Helper.GeStringArray(100);

            for (int i = 0; i < words.Length / 10; i++)
            {
                AssertPageResult(words.Page(i + 1, 10));
            }
        }

        private static void AssertPageResult(IEnumerable<string> result)
        {
            if (result.Count() > 0)
            {
                Assert.True(result.Count() == 10);
            }
            else
            {
                Assert.IsNotNull(result);
            }
        }
    }
}