using Productivity.Exceptions;
using Productivity.Extensions.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Productivity.RandomGenerator
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
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string NextAlphabet(int length = 8)
        {
            Guard.AssertArgs(length > 0, nameof(length));

            string result = string.Empty;

            lock (s_syncLock)
            {
                for (int i = 0; i < length; i++)
                {
                    char c = (char)(s_random.Next(0, 26) + 97); //use ASCII to get lower case letters.
                    if (s_random.Next(0, 2) == 1) c = char.ToUpper(c);

                    result += c;
                }
            }

            return result;
        }

        /// <summary>
        /// Generate a random string of number characters.
        /// </summary>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string NextNumeric(int length = 8)
        {
            Guard.AssertArgs(length > 0, nameof(length));

            string result = string.Empty;

            lock (s_syncLock)
            {
                for (int i = 0; i < length; i++)
                {
                    result += (char)(s_random.Next(0, 10) + 48);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates a string with specified length that meet minumum password requireents.
        /// </summary>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string NextPassword(
            int length = 8,
            int upperCount = 1,
            int numbersCount = 1,
            int specialChars = 1)
        {
            Guard.AssertArgs(length > 0, nameof(length));
            Guard.AssertArgs(upperCount.IsValid(), nameof(upperCount));
            Guard.AssertArgs(numbersCount.IsValid(), nameof(numbersCount));
            Guard.AssertArgs(specialChars.IsValid(), nameof(specialChars));
            Guard.AssertArgs(length > upperCount + numbersCount + specialChars, $"{nameof(length)} cannot be lower than the sum of required characters");

            string password = string.Empty;

            if (specialChars > 0)
            {
                password += GetChars(specialChars, 33, 47 + 1);
            }

            if (upperCount > 0)
            {
                password += GetChars(upperCount, 65, 90 + 1);
            }

            if (numbersCount > 0)
            {
                password += GetChars(numbersCount, 48, 57 + 1);
            }

            lock (s_syncLock)
            {
                int used = specialChars + upperCount + numbersCount;
                int left = length - used;

                for (int i = 0; i < left; i++)
                {
                    char c = (char)s_random.Next(97, 122);
                    password += c;
                }
            }

            return Randomize(password);

            string Randomize(string pw)
            {
                string randomized = string.Empty;
                var indexes = new List<int>(Enumerable.Range(0, pw.Length));

                while (indexes.Any())
                {
                    int index = indexes[s_random.Next(0, indexes.Count())];

                    randomized += password[index];

                    indexes.Remove(index);
                }

                return randomized;
            }

            string GetChars(int len, int min, int max)
            {
                string word = string.Empty;

                for (int i = 0; i < len; i++)
                {
                    word += (char)s_random.Next(min, max);
                }

                return word;
            }
        }

        /// <summary>
        /// Creates a randomized string of characters that includes special characters.
        /// </summary>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string NextRandomized(int length = 8)
        {
            Guard.AssertArgs(length > 0, nameof(length));

            string randomized = string.Empty;

            lock (s_syncLock)
            {
                for (int i = 0; i < length; i++)
                {
                    char c = (char)s_random.Next(0, 128);
                    randomized += c;
                }
            }

            return randomized;
        }
    }
}