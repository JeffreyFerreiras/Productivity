using NUnit.Framework;
using System;
using Tools.RandomGenerator;

namespace Tools.Test
{
    [TestFixture]
    public class RandomStringTests
    {
        [Test]
        public void NextAlphabet_ValidStringLength_RandomString()
        {
            string randLetters = RandomString.NextAlphabet(6);

            foreach(char c in randLetters)
            {
                Assert.IsTrue(char.IsLetter(c));
            }
        }

        [Test]
        public void NextAlphabet_NegativeLength()
        {
            Assert.Throws<ArgumentException>(() => RandomString.NextAlphabet(-6));
        }

        [Test]
        public void NextPassword_Test()
        {
            string pw = RandomString.NextPassword(8);
            Assert.IsTrue(pw.Length == 8);
        }

        [Test]
        public void NextPassword_NegativeLength()
        {
            Assert.Throws<ArgumentException>(()=>RandomString.NextPassword(-6));
        }

        [Test]   
        public void NextPassword_NegativeLength_Upper()
        {
            Assert.Throws<ArgumentException>(()=> RandomString.NextPassword(6, -1));
        }

        [Test]
        public void NextPassword_NegativeLength_Numbers()
        {
            Assert.Throws<ArgumentException>(() => RandomString.NextPassword(6, 1, -7));
        }

        [Test]
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

        [Test]
        public void NextRandomized_NegativeLength()
        {
            Assert.Throws<ArgumentException>(() => RandomString.NextRandomized(-7));
        }
    }
}