using Productivity.Extensions.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Productivity.RandomGenerator
{
    public class RandomChar
    {
        private static readonly Random SRandom = new Random();
        private static readonly object SSyncLock = new object();
        private static readonly List<char> SSpecialChars;

        static RandomChar()
        {
            int[] c3247 = Enumerable.Range(32, 48 - 32).ToArray();
            int[] c5864 = Enumerable.Range(58, 65 - 58).ToArray();
            int[] c9164 = Enumerable.Range(91, 97 - 91).ToArray();
            int[] c123126 = Enumerable.Range(123, 127 - 123).ToArray();

            var specialChars = new List<char>();

            specialChars.AddRange(c3247.ToCharArray());
            specialChars.AddRange(c5864.ToCharArray());
            specialChars.AddRange(c9164.ToCharArray());
            specialChars.AddRange(c123126.ToCharArray());

            SSpecialChars = specialChars;
        }

        public static char Next()
        {
            lock (SSyncLock)
            {
                return (char)SRandom.Next(0, 128);
            }
        }

        public static char NextAlphabet()
        {
            lock (SSyncLock)
            {
                char result = (char)(SRandom.Next(0, 26) + 'a');

                if (SRandom.Next(0, 2) == 1)
                    result = char.ToUpper(result);

                return result;
            }
        }

        public static char NextNumber()
        {
            lock (SSyncLock)
            {
                char result = (char)SRandom.Next(48, 58);

                return result;
            }
        }

        public static char NextSpecialCharacter()
        {
            lock (SSyncLock)
            {
                int index = SRandom.Next(0, SSpecialChars.Count);

                return SSpecialChars[index];
            }
        }
    }
}