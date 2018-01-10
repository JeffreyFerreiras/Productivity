using System.Text;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Text
{
    public class Editor
    {
        public static string Mask(string text)
        {
            return Mask(text, 4);
        }

        public static string Mask(string text, int showLast)
        {
            if(text == null || showLast >= text.Trim().Length || showLast <= 0) return text;

            string mask = new string('*', text.Length - showLast);
            string last = text.Substring((text.Length) - showLast);

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

            if(startIndex > -1)
            {
                int endIndex = text.IndexOf(@"/>", startIndex);

                if(endIndex == -1)
                {
                    endIndex = text.IndexOf(closingTag, startIndex);
                }

                int count = (endIndex - startIndex) + closingTag.Length;

                return text.Remove(startIndex, count);
            }

            return text;
        }

        public static string Strip(string text, string phrase)
        {
            int? index = text?.IndexOf(phrase);

            if(index > -1)
            {
                StringBuilder sb = new StringBuilder(text);
                sb.Replace(phrase, "");

                return sb.ToString();
            }

            return text;
        }

        public static string Strip(string text, char c)
        {
            int? index = text?.IndexOf(c);

            if(index > -1)
            {
                text = text.Remove((int)index, 1);
                return Strip(text, c);
            }

            return text;
        }

        public static string Strip(string text, params char[] characters)
        {
            foreach(char c in characters)
            {
                text = Strip(text, c);
            }

            return text;
        }
    }
}