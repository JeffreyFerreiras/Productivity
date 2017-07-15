using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Tools.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            IDictionary<string, object> itemDictionary = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase)
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
    }
}
