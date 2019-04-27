using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Extensions.Conversion;

namespace Tools.RandomGenerator
{
    public class RandomChar
    {
        private static readonly Random s_random = new Random();
        private static readonly object s_syncLock = new object();
        private static readonly List<char> s_specialChars;

        static RandomChar()
        {
            int[] c32_47 = Enumerable.Range(32, (48 - 32)).ToArray();
            int[] c58_64 = Enumerable.Range(58, (65 - 58)).ToArray();
            int[] c91_64 = Enumerable.Range(91, (97 - 91)).ToArray();
            int[] c123_126 = Enumerable.Range(123, (127 - 123)).ToArray();

            List<char> specialChars = new List<char>();

            specialChars.AddRange(c32_47.ToCharArray());
            specialChars.AddRange(c58_64.ToCharArray());
            specialChars.AddRange(c91_64.ToCharArray());
            specialChars.AddRange(c123_126.ToCharArray());

            s_specialChars = specialChars;
        }

        public static char Next()
        {
            lock (s_syncLock)
            {
                return (char)s_random.Next(0, 128);
            }
        }

        public static char NextAlphabet()
        {
            lock (s_syncLock)
            {
                char result = (char)(s_random.Next(0, 26) + 'a');

                if (s_random.Next(0, 2) == 1)
                    result = char.ToUpper(result);

                return result;
            }
        }

        public static char NextNumber()
        {
            lock (s_syncLock)
            {
                char result = (char)s_random.Next(48, 58);

                return result;
            }
        }

        public static char NextSpecialCharacter()
        {
            lock (s_syncLock)
            {
                int index = s_random.Next(0, s_specialChars.Count);

                return s_specialChars[index];
            }
        }
    }
}