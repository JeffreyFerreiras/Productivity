using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform;
using Tools.Extensions;

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
                Collection = new List<int> { 1,2,3,4,5},
                IsCom = false,
            };

            Assert.IsTrue(o.ValidateProperties());
        }

        [TestMethod]
        public void ValidateProperties_InvalidProps()
        {
            object o = new
            {
                Cost = -1,
                Name = "",
                Collection = new List<int> (),
                IsCom = false,
            };

            Assert.IsFalse(o.ValidateProperties());
        }

        [TestMethod]
        public void ValidateProperties_NullArgs()
        {
            object o = null;
            Assert.IsFalse(o.ValidateProperties());
        }

        [TestMethod]
        public void IsNullOrWhiteSpace_Test()
        {
            Assert.IsTrue(!"lasdkjfh".IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void IsNullOrWhiteSpace_Empty()
        {
            Assert.IsTrue("".IsNullOrWhiteSpace());
        }

        [TestMethod]
        public void IsNumber_Number()
        {
            object num = 5;
            Assert.IsTrue(num.IsNumber());
        } 

        [TestMethod]
        public void IsNumber_NotNumber()
        {
            object num = "";
            Assert.IsFalse(num.IsNumber());
        }

        [TestMethod]
        public void IsValidNumber_Test()
        {
            string nums = "12365486";
            Assert.IsTrue(nums.IsValidNumber());
        }

        [TestMethod]
        public void IsValidNumber_NotValid()
        {
            string nums = "la564dfad5";
            Assert.IsFalse(nums.IsValidNumber());
        }

        [TestMethod]
        public void IsValidNumber_Empty()
        {
            string nums = "";
            Assert.IsFalse(nums.IsValidNumber());
        }

        [TestMethod]
        public void HasUpper_Test()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasUpper(3));
        }

        [TestMethod]
        public void HasLower_Test()
        {
             string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasLower(3));
        }

        [TestMethod]
        public void HasNumber_Test()
        {
            string s = "AaBbCc12345!@#";
            Assert.IsTrue(s.HasNumber(3));
        }
    }
}
