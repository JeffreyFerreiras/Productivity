using System;
using System.Linq;

namespace Tools
{
    using Extensions;
    using Tools.Exceptions;

    public class RandomString
    {
        /// <summary>
        /// Generate a random string of alphabet characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NextAlphabet(int length = 8)
        {
            Guard.ThrowIfInvalidArgs(length);

            var random = new Random();
            string result = string.Empty;

            for (int i = 0; i < length; i++)
            {
                char c = (char)(random.Next(0, 26) + 97); //use ASCII to get lower case letters.
                if (random.Next(0, 2) == 1) c = char.ToUpper(c);
                result += c;
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
            Guard.ThrowIfInvalidArgs(length, upperCount, numbersCount);

            var random = new Random();
            string password = string.Empty;

            for (int i = 0; i < length; i++)
            {
                char c = (char)(random.Next(0, 94) + 33);
                password += c;
            }

            return password;
        }

        /// <summary>
        /// Creates a randomized string of characters that includes special characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string NextRandomized(int length =8)
        {
            Guard.ThrowIfInvalidArgs(length);

            string randomized = string.Empty;
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                char c = (char)random.Next(0, 128);
                randomized += c;
            }
    
            return randomized;
        }
    }
}
