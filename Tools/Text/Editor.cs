using System.Text;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Text
{
    public class Editor
    {
        /// <summary>
        /// Mask the last 4 places in a given string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Mask(string text)
        {
            return Mask(text, 4);
        }

        /// <summary>
        /// Mask the last positions of a string given a count.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Mask(string text, int count)
        {
            if (text == null || count >= text.Trim().Length || count <= 0) return text;

            string mask = new string('*', text.Length - count);
            string last = text.Substring((text.Length) - count);

            return mask + last;
        }

        /// <summary>
        /// Removes the first occurence of HTML element and its content.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static string RemoveHtmlTag(string text, string tagName)
        {
            Guard.AssertArgs(text.IsValid(), nameof(text));
            Guard.AssertArgs(tagName.IsValid(), nameof(tagName));

            string closingTag = "</" + tagName.Substring(1);
            tagName = tagName.Substring(0, tagName.Length - 1);

            int startIndex = text.IndexOf(tagName);

            if (startIndex > -1)
            {
                int endIndex = text.IndexOf(@"/>", startIndex);

                if (endIndex == -1)
                {
                    endIndex = text.IndexOf(closingTag, startIndex);
                }

                int count = (endIndex - startIndex) + closingTag.Length;

                return text.Remove(startIndex, count);
            }

            return text;
        }

        /// <summary>
        /// Removes all occurrences of a phrase within text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static string Strip(string text, string phrase)
        {
            int? index = text?.IndexOf(phrase);

            if (index > -1)
            {
                StringBuilder sb = new StringBuilder(text);

                sb.Replace(phrase, "");

                return sb.ToString();
            }

            return text;
        }

        /// <summary>
        /// Removes all occurences of all provided phrases within text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="phrases"></param>
        /// <returns></returns>
        public static string Strip(string text, params string[] phrases)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            foreach (string phrase in phrases)
            {
                text = Strip(text, phrase);
            }

            return text;
        }

        /// <summary>
        /// Removes all occurrences of character from text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string Strip(string text, char c)
        {
            int? index = text?.IndexOf(c);

            if (index > -1)
            {
                text = text.Remove((int)index, 1);

                return Strip(text, c);
            }

            return text;
        }

        /// <summary>
        /// Removes all occurrences of character from text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="characters"></param>
        /// <returns></returns>
        public static string Strip(string text, params char[] characters)
        {
            foreach (char c in characters)
            {
                text = Strip(text, c);
            }

            return text;
        }
    }
}