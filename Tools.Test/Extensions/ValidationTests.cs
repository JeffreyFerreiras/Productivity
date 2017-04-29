using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.Extensions;

namespace Tools.Test.Extensions
{
    [TestClass]
    public class ValidationTests
    {
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
