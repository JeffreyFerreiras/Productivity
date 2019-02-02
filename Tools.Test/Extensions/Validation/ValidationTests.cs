using NUnit.Framework;
using System;
using System.Collections.Generic;
using Tools.Extensions.Validation;

namespace Tools.Test.Extensions
{
    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void IsValidUrl_ValidUrl_ReturnsTrue()
        {
            string url = "https://www.google.com/search?q=what";

            Assert.IsTrue(url.IsValidUrl());
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void IsValidUrl_InvalidData_ReturnsTrue(string url)
        {
            Assert.IsFalse(url.IsValidUrl());
        }

        [Test]
        public void ValidateProperties_Test()
        {
            object o = new
            {
                Cost = 88.50,
                Name = "Bob",
                Collection = new List<int> { 1, 2, 3, 4, 5 },
                IsCom = false,
            };

            Assert.IsTrue(o.HasValidProperties());
        }

        [Test]
        public void ValidateProperties_InvalidProps()
        {
            object o = new
            {
                Cost = -1,
                Name = "",
                Collection = new List<int>(),
                IsCom = false,
            };

            Assert.IsFalse(o.HasValidProperties());
        }

        [Test]
        public void ValidateProperties_NullArgs()
        {
            object o = null;
            Assert.IsFalse(o.HasValidProperties());
        }

        [Test]
        public void IsNullOrWhiteSpace_Test()
        {
            Assert.IsFalse("lasdkjfh".IsNullOrWhiteSpace());
        }

        [Test]
        public void IsNullOrWhiteSpace_Empty()
        {
            Assert.IsTrue("".IsNullOrWhiteSpace());
        }

        [Test]
        public void IsNumber_Number_ReturnsTrue()
        {
            object num = 5;
            Assert.IsTrue(num.IsNumber());
        }

        [Test]
        public void IsNumber_EmptyString_ReturnsFalse()
        {
            object num = "";
            Assert.IsFalse(num.IsNumber());
        }

        [Test]
        public void IsValidNumber_ValidStringNumber_ReturnsTrue()
        {
            string nums = "12365486";
            Assert.IsTrue(nums.IsValidNumber());
        }

        [Test]
        public void IsValidNumber_InvalidStringNumber_ReturnsFalse()
        {
            string nums = "la564dfad5";
            Assert.IsFalse(nums.IsValidNumber());
        }

        [Test]
        public void IsValidNumber_EmptyString_ReturnsFalse()
        {
            string nums = "";
            Assert.IsFalse(nums.IsValidNumber());
        }

        [Test]
        public void IsValidNumber_NegativeNumber_ReturnsTrue()
        {
            Assert.IsTrue("-354".IsValidNumber());
            Assert.IsTrue((-999).ToString().IsValidNumber());
        }

        [Test]
        public void HasUpper_MixedString_ReturnsTrue()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasUpper(3));
        }

        [Test]
        public void HasLower_MixedString_ReturnsTrue()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasLower(3));
        }

        [Test]
        public void HasNumber_MixedString_ReturnsTrue()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasNumber(3));
        }

        [Test]
        public void HasOnlyLetters_LowerCaseLetters_ReturnsTrue()
        {
            string abc = "abc";

            Assert.IsTrue(abc.HasOnlyLetters());
        }

        [Test]
        public void HasOnlyLetters_UpperCaseLetters_ReturnsTrue()
        {
            string abc = "ABC";

            Assert.IsTrue(abc.HasOnlyLetters());
        }

        [Test]
        public void HasOnlyLetters_Numbers_ReturnsTrue()
        {
            string nums = "123456";

            Assert.IsFalse(nums.HasOnlyLetters());
        }

        [Test]
        public void EqualsIgnoreCase_EqualStrings_ReturnsTrue()
        {
            string first = "Hello World!";
            string second = "HellO World!";

            Assert.IsTrue(first.EqualsIgnoreCase(second));
        }

        [Test]
        public void EqualsIgnoreCase_Null_ReturnsFalse()
        {
            string first = null;
            string second = null;

            Assert.IsFalse(first.EqualsIgnoreCase(second));
        }

        [Test]
        public void EqualsIgnoreCase_Different_ReturnsFalse()
        {
            string first = "Hello World!";
            string second = "HellO!";

            Assert.IsFalse(first.EqualsIgnoreCase(second));
        }

        [Test]
        public void EqualsIgnoreCase_Empty_ReturnsTrue()
        {
            string first = "";
            string second = "";

            Assert.IsTrue(first.EqualsIgnoreCase(second));
        }

        [Test]
        public void EqualsIgnoreCase_NullEmpty_ReturnsFalse()
        {
            string first = null;
            string second = "";

            Assert.IsFalse(first.EqualsIgnoreCase(second));
            Assert.IsFalse(second.EqualsIgnoreCase(first));
        }


        [Test]
        public void IsSortedAsc_SortedArray_ReturnsTrue()
        {
            var arr = Helper.GetRandomArray(100);

            Array.Sort(arr);

            Assert.IsTrue(arr.IsSortedAsc());
        }

        [Test]
        public void IsSortedAsc_UnsortedArray_ReturnsFalse()
        {
            var arr = Helper.GetRandomArray(100);

            Assert.IsFalse(arr.IsSortedAsc());
        }

        [Test]
        public void IsSortedAsc_EmptyArray_ReturnsTrue()
        {
            var arr = new int[] { };
            Assert.IsTrue(arr.IsSortedAsc());
        }

        [Test]
        public void IsSortedAsc_NullArray_ThrowsArgumentException()
        {
            int[] arr = null;
            Assert.Throws<ArgumentException>(() => arr.IsSortedAsc());
        }

        [Test]
        public void IsValidABA_ValidABA_ReturnsTrue()
        {
            string aba = "021000021";
            Assert.IsTrue(aba.IsValidABA());
        }

        [Test]
        public void IsValidCreditCard_ValidCard_ReturnsTrue()
        {
            string ccnum = "5454545454545454";
            Assert.IsTrue(ccnum.IsValidCard());
        }
    }
}