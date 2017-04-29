using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tools.Test
{
    using Tools.Extensions;

    [TestClass]
    public class RandomStringTests
    {
        [TestMethod]
        public void Next_Test()
        {
            string randLetters = RandomString.Next(6);

            Assert.IsTrue(randLetters.Length == 6);
            Assert.IsTrue(randLetters.OnlyLetters());
        }

        [TestMethod]
        public void NextPassword_Test()
        {
            string pw = RandomString.NextPassword(8);
            Assert.IsTrue(pw.Length == 8);
        }
    }
}