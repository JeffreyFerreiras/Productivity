using Common.Tools.RandomGenerator;
using NUnit.Framework;

namespace Common.Tools.Test.RandomGenerator
{
    [TestFixture]
    public class RandomCharTests
    {
        [Test]
        public void Next_ValidChar()
        {
            for (int i = 0; i < 1000; i++)
            {
                char randomChar = RandomChar.NextAlphabet();
                int asciiNum = randomChar;

                Assert.IsTrue(asciiNum < 128 && asciiNum >= 0);
            }
        }

        [Test]
        public void NextAlphabet_ReturnsRandomAlphabetChar()
        {
            for (int i = 0; i < 1000; i++)
            {
                char randomChar = RandomChar.NextAlphabet();

                Assert.IsTrue(char.IsLetter(randomChar));
            }
        }

        [Test]
        public void NextNumber_ReturnsNumberChar()
        {
            for (int i = 0; i < 1000; i++)
            {
                char num = RandomChar.NextNumber();

                Assert.IsTrue(char.IsDigit(num));
            }
        }

        [Test]
        public void NextNumber_ReturnsSpecialChar()
        {
            for (int i = 0; i < 1000; i++)
            {
                char special = RandomChar.NextSpecialCharacter();

                Assert.IsFalse(char.IsDigit(special));
                Assert.IsFalse(char.IsNumber(special));
            }
        }
    }
}