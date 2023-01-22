using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Productivity.Extensions.Text
{
    public static class TextEx
    {
        public static string Tab(this string text) => text.Tab(1);

        public static string Tab(this string text, int count)
        {
            if (text == null || count <= 0)
                return text;

            string tabs = new string('\t', count);

            return text + tabs;
        }

        public static string Left(this string text, int length)
        {
            text = text ?? string.Empty;
            return text.Substring(0, Math.Min(length, text.Length));
        }

        public static string Right(this string text, int length)
        {
            text = text ?? string.Empty;
            return text.Length >= length
                ? text.Substring(text.Length - length, length)
                : text;
        }

        /// <summary>
        /// Converts a enum's name if in camel case and adds a space between each capital letter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReadableName<T>(this T value) where T : Enum
        {
            if (value.ToString().All(c => char.IsUpper(c)))
            {
                return value.ToString();
            }

            string result = string.Empty;

            foreach (char c in value.ToString())
            {
                if (char.IsUpper(c))
                {
                    result += ' ';
                }

                result += c;
            }

            return result.Trim();
        }

        /// <summary>
        /// Removes all non-numeric characters from a string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveNonNumeric(this string source)
        {
            if (source is null)
            {
                return null;
            }

            return Regex.Replace(source, @"[^0-9]", "");
        }
    }
}