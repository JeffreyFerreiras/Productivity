﻿using System;
using System.Linq;

namespace Tools
{
    using Extensions;

    public class RandomString
    {
        /// <summary>
        /// Generate a random string of alphabet characters.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Next(int length = 8)
        {
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
        public static string NextPassword(int length = 8, int upperCount = 1, int numbersCount=1)
        {
            var random = new Random();
            string password = string.Empty;

            for (int i = 0; i < length; i++)
            {
                char c = (char)(random.Next(0, 94) + 33);
                password += c;
            }
            return password;
        }
    }
}
