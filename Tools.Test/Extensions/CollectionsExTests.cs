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
        [TestMethod]
        public void ToDictionary_ConvertObject_IDictionary()
        {
            var item = new
            {
                a = "Hellon world!",
                b = 5,
                c = 11.11,
                d = new int [] {1,2,3,4,5},
                e = "Hello world!".ToCharArray()
            };

            IDictionary<string, object> itemDictionary;
            itemDictionary = item.ToDictionary();

            Assert.IsTrue(itemDictionary.Count > 0);
        }
    }
}
