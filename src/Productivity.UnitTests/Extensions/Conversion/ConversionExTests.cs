using NUnit.Framework;
using System;
using Productivity.Extensions.Conversion;
using Productivity.Extensions.Validation;

namespace Productivity.UnitTests.Extensions.Conversion
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

            Assert.Throws<ArgumentNullException>(() => s.ToByteArray());
        }

        [Test]
        public void FromYN_TruthString_RetursTrue()
        {
            Assert.IsTrue("Y".FromYn());
            Assert.IsTrue("Yes".FromYn());
            Assert.IsTrue("T".FromYn());
            Assert.IsTrue("true".FromYn());
        }

        [Test]
        public void FromYN_FalseString_RetursFalse()
        {
            Assert.IsFalse("".FromYn());
            Assert.IsFalse("F".FromYn());
            Assert.IsFalse("false".FromYn());
            Assert.IsFalse("N".FromYn());
        }

        [Test]
        public void FromYN_NullString_RetursFalse()
        {
            string s = null;
            Assert.IsFalse(s.FromYn());
        }

        [Test]
        public void ToDateTime_ValidInt_ReturnsDateTime()
        {
            var dateTime = 20180425.ToDateTime();

            Assert.IsFalse(default == dateTime);
        }

        [Test]
        public void ToDateTime_InvalidFormat_ReturnsNull()
        {
            Assert.Throws<InvalidOperationException>(() => 112233.ToDateTime());
        }

        [Test]
        public void PopulateWith_ComplexToSimple_ConvertsObjects()
        {
            var complex = new ComplexMan();
            var simple = new SimpleMan();

            simple.PopulateWith(complex);

            Assert.IsTrue(simple.HasValidProperties());
        }

        [Test]
        public void PopulateWith_SimpleToComplex_ConvertsObjects()
        {
            var complex = new ComplexMan()
            {
                Dob = default,
                Name = "",
                Age = -5,
                Suit = Suits.Club,
            };
            var simple = new SimpleMan()
            {
                Dob = DateTime.Now.ToShortDateString(),
                Name = "Billy",
                Age = 160,
                Suit = "Spade"
            };

            complex.PopulateWith(simple);

            Assert.IsTrue(simple.HasValidProperties());
        }

        [Test]
        public void PopulateWith_InvalidConversion_SilentlyFail()
        {
            var complex = new ComplexMan()
            {
                Dob = default,
                Name = "",
                Age = -5,
                Suit = Suits.Club,
            };
            var simple = new SimpleMan()
            {
                Dob = DateTime.Now.ToShortDateString(),
                Name = "Billy",
                Age = 160,
                Suit = "SOME RANDOMNESS"
            };

            complex.PopulateWith(simple);

            Assert.IsTrue(simple.HasValidProperties());
        }

        [Test]
        public void CleanCurrency_MultipleScenarios_ReturnsOnlyNumbers()
        {
            Assert.AreEqual("1523.00", "$1,523.00".CleanMoney());
            Assert.AreEqual("1523.00", "1,523.00".CleanMoney());
            Assert.AreEqual("1523", "$1,523".CleanMoney());
            Assert.AreEqual("100454523.99", "$100,454,523.99".CleanMoney());
            Assert.AreEqual("100454523.99", "!100,454,523.99".CleanMoney());
            Assert.AreEqual("N/A", "".CleanMoney());
        }

        [Test]
        public void ToByteArray_Object_ReturnsByteArray()
        {
            var model = Helper.GetComplexFake();

            var arry = model.ToByteArray();

            Assert.True(arry.Length > 0);
        }

        [Test]
        public void FromByteArray_Object_ReturnsByteArray()
        {
            var model = Helper.GetComplexFake();
            var arry = model.ToByteArray();

            var model2 = arry.FromByteArray<ComplexFake>();

            Assert.True(model.DeepEquals(model2));
        }

        [Test]
        public void GetEnumValues_Enum_ReturnsAllEnumValues()
        {
            Suits suit = Suits.Club;
            var suits = Enum.GetValues(typeof(Suits));

            Assert.True(suits.Length == suit.GetEnumValues().Length);
        }
    }
}