using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.RandomGenerator;

namespace Tools.Test.RandomGenerator
{
    [TestClass]
    public class RandomCharTests
    {
        [TestMethod]
        public void Next_ValidChar()
        {
            for(int i = 0; i < 1000; i++)
            {
                char randomChar = RandomChar.NextAlphabet();
                int asciiNum = randomChar;

                Assert.IsTrue(asciiNum < 128 && asciiNum >=0);
            }
        }

        [TestMethod]
        public void NextAlphabet_ReturnsRandomAlphabetChar()
        {
            for(int i = 0; i < 1000; i++)
            {
                char randomChar = RandomChar.NextAlphabet();

                Assert.IsTrue(char.IsLetter(randomChar));
            }
        }

        [TestMethod]
        public void NextNumber_ReturnsNumberChar()
        {
            for(int i = 0; i < 1000; i++)
            {
                char num = RandomChar.NextNumber();

                Assert.IsTrue(char.IsDigit(num));
            }
        }

        [TestMethod]
        public void NextNumber_ReturnsSpecialChar()
        {
            for(int i = 0; i < 1000; i++)
            {
                char special = RandomChar.NextSpecialCharacter();

                Assert.IsFalse(char.IsDigit(special));
                Assert.IsFalse(char.IsNumber(special));
            }
        }
    }
}