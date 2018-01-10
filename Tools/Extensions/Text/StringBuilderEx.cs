using System;
using System.Text;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Extensions.Text
{
    public static class StringBuilderEx
    {
        public static int IndexOf(this StringBuilder sb, char value)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            return sb.IndexOf(value, 0, sb.Length);
        }

        public static int IndexOf(this StringBuilder sb, char value, int startIndex)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            return sb.IndexOf(value, startIndex, sb.Length - startIndex);
        }

        public static int IndexOf(this StringBuilder sb, char value, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            int length = startIndex + count;

            for(int i = startIndex; i < length; i++)
            {
                if(value == sb[i]) return i;
            }

            return -1;
        }

        public static int IndexOf(this StringBuilder sb, string phrase)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            return sb.IndexOf(phrase, 0, sb.Length);
        }

        public static int IndexOf(this StringBuilder sb, string phrase, int startIndex)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");
            Guard.AssertArgs(phrase != null, nameof(phrase));

            return sb.IndexOf(phrase, startIndex, phrase.Length);
        }

        public static int IndexOf(this StringBuilder sb, string phrase, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");
            Guard.AssertArgs(phrase.IsValid(), nameof(phrase));

            int length = startIndex + count;

            Guard.Assert<IndexOutOfRangeException>(sb.Length >= length, "Count out of bounds from start index");

            for(int i = startIndex; i < length; i++)
            {
                int foundAtIndex = i;
                if(sb[i] != phrase[0]) continue;

                bool isMatch = false;

                for(int j = 1; j < phrase.Length; j++)
                {
                    if(phrase[j] == sb[i + j])
                    {
                        isMatch = true;
                    }
                    else
                    {
                        isMatch = false;
                        i += j;

                        break;
                    }
                }

                if(isMatch)
                {
                    return foundAtIndex;
                }
            }

            return -1;
        }

        public static int IndexOfAny(this StringBuilder sb, params char[] characters)
        {
            sb[0] = 's';

            throw new NotImplementedException();
        }

        public static int LastIndexOf(this StringBuilder sb, char c)
        {
            sb[0] = 's';

            throw new NotImplementedException();
        }

        public static int LastIndexOf(this StringBuilder sb, string phrase)
        {
            sb[0] = 's';

            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this StringBuilder sb, params char[] characters)
        {
            throw new NotImplementedException();
        }
    }
}