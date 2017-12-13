using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tools.Test
{
    using Tools.Extensions.Validation;

    [TestClass]
    public class RandomStringTests
    {
        [TestMethod]
        public void NextAlphabet_ValidStringLength_RandomString()
        {
            string randLetters = RandomString.NextAlphabet(6);

            Assert.IsTrue(randLetters.Length == 6);
            Assert.IsTrue(randLetters.HasOnlyLetters());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NextAlphabet_NegativeLength()
        {
            string randLetters = RandomString.NextAlphabet(-6);
        }

        [TestMethod]
        public void NextPassword_Test()
        {
            string pw = RandomString.NextPassword(8);
            Assert.IsTrue(pw.Length == 8);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NextPassword_NegativeLength()
        {
            string randLetters = RandomString.NextPassword(-6);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NextPassword_NegativeLength_Upper()
        {
            string randLetters = RandomString.NextPassword(6, -1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NextPassword_NegativeLength_Numbers()
        {
            string randLetters = RandomString.NextPassword(6, 1, -7);
        }

        [TestMethod]
        public void NextRandomized_Test()
        {
            int count = 15;

            while (count > 0)
            {
                string randomized = RandomString.NextRandomized(8);
                Assert.IsNotNull(randomized);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(randomized));
                count--;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NextRandomized_NegativeLength()
        {
            string randLetters = RandomString.NextRandomized(-7);
        }
    }
}