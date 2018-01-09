using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
