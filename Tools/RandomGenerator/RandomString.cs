using System;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.RandomGenerator
{
    /// <summary>
    /// Provides thread safe random string generator
    /// </summary>
    public class RandomString
    {
        private static readonly Random s_random = new Random();
        private static readonly object s_syncLock = new object();

        /// <summary>
        /// Generate a random string of alphabet characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NextAlphabet(int length = 8)
        {
            Guard.AssertArgs(length > 0, nameof(length));

            string result = string.Empty;

            lock(s_syncLock)
            {
                for(int i = 0; i < length; i++)
                {
                    char c = (char)(s_random.Next(0, 26) + 97); //use ASCII to get lower case letters.
                    if(s_random.Next(0, 2) == 1) c = char.ToUpper(c);
                    result += c;
                }
            }

            return result;
        }

        /// <summary>
        /// Generates a string with specified length that meet minumum password requireents.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NextPassword(int length = 8, int upperCount = 1, int numbersCount = 1)
        {
            Guard.AssertArgs(length > 0, nameof(length));
            Guard.AssertArgs(upperCount.IsValid(), nameof(upperCount));
            Guard.AssertArgs(numbersCount.IsValid(), nameof(numbersCount));

            string password = string.Empty;

            lock(s_syncLock)
            {
                for(int i = 0; i < length; i++)
                {
                    char c = (char)(s_random.Next(0, 94) + 33);
                    password += c;
                }
            }

            return password;
        }

        /// <summary>
        /// Creates a randomized string of characters that includes special characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NextRandomized(int length = 8)
        {
            Guard.AssertArgs(length > 0, nameof(length));

            string randomized = string.Empty;

            lock(s_syncLock)
            {
                for(int i = 0; i < length; i++)
                {
                    char c = (char)s_random.Next(0, 128);
                    randomized += c;
                }
            }

            return randomized;
        }
    }
}