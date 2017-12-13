using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Tools.Extensions.Conversion;

namespace Tools.Test.Extensions.Conversion
{
    [TestClass]
    public class ConversionExTests
    {
        [TestMethod]
        public void ToEnum_ValidEnum_ConvertsToEnum()
        {
            string strEnum = "SPADE";

            Suits suit = strEnum.ToEnum(typeof(Suits));
            Assert.IsTrue(suit.ToString().Equals(strEnum, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ToEnum_ValidEnum_ConvertsToEnumWithGenericOverload()
        {
            string strEnum = "SPADE";

            Suits suit = strEnum.ToEnum<Suits>(true);

            Assert.IsTrue(suit.ToString().Equals(strEnum, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void ToEnum_InvalidEnumInput_ThrowsInvalidOperationException()
        {
            string strEnum = "SPADE";

            Assert.ThrowsException<InvalidOperationException>(() => strEnum.ToEnum<Suits>(false));
        }

        [TestMethod]
        public void ToEnum_ValidEnum_ConvertsToEnumWithoutIgnoringCase()
        {
            string strEnum = "Club";

            Assert.IsTrue(Suits.Club == strEnum.ToEnum<Suits>(false));
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
    }
}