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
            string randLetters = RandomString.NextAlphabet(6);

            Assert.IsTrue(randLetters.Length == 6);
            Assert.IsTrue(randLetters.OnlyLetters());
        }

        [TestMethod]
        public void NextPassword_Test()
        {
            string pw = RandomString.NextPassword(8);
            Assert.IsTrue(pw.Length == 8);
        }

        [TestMethod]
        public void NextRandomized_Test()
        {
            int count = 15;

            while(count > 0)
            {
                string randomized = RandomString.NextRandomized(8);
                Assert.IsNotNull(randomized);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(randomized));
                count--;
            }
        }
    }
}