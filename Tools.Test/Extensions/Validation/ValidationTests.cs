using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tools.Extensions.Conversion;
using Tools.Extensions.Validation;

namespace Tools.Test.Extensions
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void ValidateProperties_NullArgs()
        {
            object o = null;
            Assert.IsFalse(o.HasValidProperties());
        }

        [TestMethod]
        public void IsNullOrWhiteSpace_Test()
        {
            Assert.IsFalse("lasdkjfh".IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void IsNullOrWhiteSpace_Empty()
        {
            Assert.IsTrue("".IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void IsNumber_Number_ReturnsTrue()
        {
            object num = 5;
            Assert.IsTrue(num.IsNumber());
        }

        [TestMethod]
        public void IsNumber_EmptyString_ReturnsFalse()
        {
            object num = "";
            Assert.IsFalse(num.IsNumber());
        }

        [TestMethod]
        public void IsValidNumber_ValidStringNumber_ReturnsTrue()
        {
            string nums = "12365486";
            Assert.IsTrue(nums.IsValidNumber());
        }

        [TestMethod]
        public void IsValidNumber_InvalidStringNumber_ReturnsFalse()
        {
            string nums = "la564dfad5";
            Assert.IsFalse(nums.IsValidNumber());
        }

        [TestMethod]
        public void IsValidNumber_EmptyString_ReturnsFalse()
        {
            string nums = "";
            Assert.IsFalse(nums.IsValidNumber());
        }

        [TestMethod]
        public void IsValidNumber_NegativeNumber_ReturnsTrue()
        {
            Assert.IsTrue("-354".IsValidNumber());
            Assert.IsTrue((-999).ToString().IsValidNumber());
        }

        [TestMethod]
        public void HasUpper_MixedString_ReturnsTrue()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasUpper(3));
        }

        [TestMethod]
        public void HasLower_MixedString_ReturnsTrue()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasLower(3));
        }

        [TestMethod]
        public void HasNumber_MixedString_ReturnsTrue()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasNumber(3));
        }

        [TestMethod]
        public void HasOnlyLetters_LowerCaseLetters_ReturnsTrue()
        {
            string abc = "abc";

            Assert.IsTrue(abc.HasOnlyLetters());
        }

        [TestMethod]
        public void HasOnlyLetters_UpperCaseLetters_ReturnsTrue()
        {
            string abc = "ABC";

            Assert.IsTrue(abc.HasOnlyLetters());
        }

        [TestMethod]
        public void HasOnlyLetters_Numbers_ReturnsTrue()
        {
            string nums = "123456";

            Assert.IsFalse(nums.HasOnlyLetters());
        }

        [TestMethod]
        public void EqualsIgnoreCase_EqualStrings_ReturnsTrue()
        {
            string first = "Hello World!";
            string second = "HellO World!";

            Assert.IsTrue(first.EqualsIgnoreCase(second));
        }

        [TestMethod]
        public void EqualsIgnoreCase_Null_ReturnsFalse()
        {
            string first = null;
            string second = null;

            Assert.IsFalse(first.EqualsIgnoreCase(second));
        }

        [TestMethod]
        public void EqualsIgnoreCase_Different_ReturnsFalse()
        {
            string first = "Hello World!";
            string second = "HellO!";

            Assert.IsFalse(first.EqualsIgnoreCase(second));
        }

        [TestMethod]
        public void EqualsIgnoreCase_Empty_ReturnsTrue()
        {
            string first = "";
            string second = "";

            Assert.IsTrue(first.EqualsIgnoreCase(second));
        }

        [TestMethod]
        public void EqualsIgnoreCase_NullEmpty_ReturnsFalse()
        {
            string first = null;
            string second = "";

            Assert.IsFalse(first.EqualsIgnoreCase(second));
            Assert.IsFalse(second.EqualsIgnoreCase(first));
        }
        
    }
}