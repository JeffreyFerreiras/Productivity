using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions.Conversion;

namespace Tools.Test.Extensions.Conversion
{
    [TestFixture]
    public class ConversionExTests
    {
        #region ToEnum

        [Test]
        public void ToEnum_ValidEnum_ConvertsToEnum()
        {
            string strEnum = "SPADE";

            Suits suit = strEnum.ToEnum(typeof(Suits));
            Assert.IsTrue(suit.ToString().Equals(strEnum, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ToEnum_ValidEnum_ConvertsToEnumWithGenericOverload()
        {
            string strEnum = "SPADE";

            Suits suit = strEnum.ToEnum<Suits>(true);

            Assert.IsTrue(suit.ToString().Equals(strEnum, StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ToEnum_InvalidEnumInput_ThrowsInvalidOperationException()
        {
            string strEnum = "SPADE";

            Assert.Throws<InvalidOperationException>(() => strEnum.ToEnum<Suits>(false));
        }

        [Test]
        public void ToEnum_ValidEnum_ConvertsToEnumWithoutIgnoringCase()
        {
            string strEnum = "Club";

            Assert.IsTrue(Suits.Club == strEnum.ToEnum<Suits>(false));
        }

        #endregion ToEnum

        [Test]
        public void ToDictionary_Object_IDictionary()
        {
            var item = new
            {
                a = "Hellon world!",
                b = 5,
                c = 11.11,
                d = new int[] { 1, 2, 3, 4, 5 },
                e = "Hello world!".ToCharArray(),
                f = Suits.Diamond,
                g = DateTime.Now,
            };

            var itemDictionary = item.ToDictionary();

            Assert.IsTrue(itemDictionary.Count > 0);
        }

        [Test]
        public void ToByteArray_ValidInput_ReturnsByteArray()
        {
            string s = "Hello World!";
            byte[] bin = s.ToByteArray();

            Assert.IsTrue(bin.Length > 0);
        }

        [Test]
        public void ToByteArray_InValidInput_Throws()
        {
            string s = null;

            Assert.Throws<ArgumentNullException>(()=>s.ToByteArray());
        }

        [Test]
        public void FromYN_TruthString_RetursTrue()
        {
            Assert.IsTrue("Y".FromYN());
            Assert.IsTrue("Yes".FromYN());
            Assert.IsTrue("T".FromYN());
            Assert.IsTrue("true".FromYN());
        }

        [Test]
        public void FromYN_FalseString_RetursFalse()
        {
            Assert.IsFalse("".FromYN());
            Assert.IsFalse("F".FromYN());
            Assert.IsFalse("false".FromYN());
            Assert.IsFalse("N".FromYN());
        }

        [Test]
        public void FromYN_NullString_RetursFalse()
        {
            string s = null;
            Assert.IsFalse(s.FromYN());
        }
    }
}