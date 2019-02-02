using System;

namespace Tools.Extensions.Text
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
            text = (text ?? string.Empty);
            return text.Substring(0, Math.Min(length, text.Length));
        }

        public static string Right(this string text, int length)
        {
            text = (text ?? string.Empty);
            return (text.Length >= length)
                ? text.Substring(text.Length - length, length)
                : text;
        }
    }
}