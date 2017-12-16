using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Tools.Extensions.Conversion;

namespace Tools.Test.Extensions.Conversion
{
    [TestClass]
    public class ConversionExTests
    {
        #region ToEnum
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
        #endregion

        [TestMethod]
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

            IDictionary<string, object> itemDictionary;
            itemDictionary = item.ToDictionary();

            Assert.IsTrue(itemDictionary.Count > 0);
        }

        [TestMethod]
        public void DeepCopy_ValidObject_Clones()
        {
            var obj = Helper.GetFake();
            var clone = obj.DeepClone();
            
            foreach(var prop in obj.GetType().GetTypeInfo().GetProperties())
            {
                var originalValue = prop.GetValue(obj);
                var cloneValue = prop.GetValue(clone);

                Assert.IsTrue(cloneValue.Equals(originalValue));
            }
        }

        [TestMethod]
        public void DeepCopy_ValidClomplexObject_Clones()
        {
            var obj = Helper.GetFakeMultiType();
            var clone = obj.DeepClone();

            foreach(var prop in obj.GetType().GetTypeInfo().GetProperties())
            {
                var originalValue = prop.GetValue(obj);
                var cloneValue = prop.GetValue(clone);

                if(originalValue == null) continue;

                if(originalValue is List<FakeTest>)
                {
                    var orig = (List<FakeTest>)originalValue;
                    var clon = (List<FakeTest>)cloneValue;

                    Assert.IsFalse(cloneValue == originalValue);
                    orig.SequenceEqual(clon);
                }
                else if(originalValue is string[])
                {
                    var orig = (string[])originalValue;
                    var clon = (string[])cloneValue;

                    Assert.IsFalse(cloneValue == originalValue);
                    Assert.IsTrue(orig.SequenceEqual(clon));
                }
                else
                {
                    Assert.IsTrue(cloneValue.Equals(originalValue)); //Deep comparison
                    Assert.IsFalse(cloneValue == originalValue); // Reference comparison

                    originalValue = null;

                    Assert.IsFalse(cloneValue.Equals(originalValue)); //Make sure references were not simply copied.
                }
            }
        }

    }
}