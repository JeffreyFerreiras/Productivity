using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Extensions.Text
{
    public static class StringBuilderEx
    {
        const string OutOfBoundsMessage = "Target out of bounds of string builder length from start index";


        public static int IndexOf(this StringBuilder sb, char value)
        {
            return sb.IndexOf(value, 0);
        }

        public static int IndexOf(this StringBuilder sb, char value, int startIndex)
        {
            int length = (sb?.Length ?? 0);
            int count = length - startIndex;

            return sb.IndexOf(value, startIndex, count);
        }

        public static int IndexOf(this StringBuilder sb, char value, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            int length = startIndex + count;

            Guard.Assert<IndexOutOfRangeException>(sb.Length <= length, OutOfBoundsMessage);

            for(int i = startIndex; i < length; i++)
            {
                if(value == sb[i]) return i;
            }

            return -1;
        }

        public static int IndexOf(this StringBuilder sb, string phrase)
        {
            return sb.IndexOf(phrase, 0);
        }

        public static int IndexOf(this StringBuilder sb, string phrase, int startIndex)
        {
            int length = sb?.Length ?? 0;
            int count = length - startIndex;

            return sb.IndexOf(phrase, startIndex, count);
        }

        public static int IndexOf(this StringBuilder sb, string phrase, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");
            Guard.AssertArgs(phrase != null, nameof(phrase));

            if(phrase.Length == 0) return -1;
            int length = startIndex + count;

            BoundsCheck(sb, startIndex, count, length);

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

        private static void BoundsCheck(StringBuilder sb, int startIndex, int count, int length)
        {
            Guard.Assert<IndexOutOfRangeException>(startIndex >= 0 && sb.Length > startIndex, OutOfBoundsMessage);
            Guard.Assert<IndexOutOfRangeException>(sb.Length >= count, OutOfBoundsMessage);
            Guard.Assert<IndexOutOfRangeException>(sb.Length <= length, OutOfBoundsMessage);
        }

        public static int IndexOfAny(this StringBuilder sb, char[] anyOf)
        {
            return sb.IndexOfAny(anyOf, 0);
        }
        public static int IndexOfAny(this StringBuilder sb, char[]anyOf, int startIndex)
        {
            int length = sb?.Length ?? 0;
            int count = length - startIndex;

            return sb.IndexOfAny(anyOf, startIndex, count);
        }
        public static int IndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            int length = startIndex + count;

            BoundsCheck(sb, startIndex, count, length);

            var anyOfSet = new HashSet<char>(anyOf);

            for(int i = startIndex; i < length; i++)
            {
                char value = sb[i];

                if(anyOfSet.Contains(value))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int LastIndexOf(this StringBuilder sb, char value)
        {
            return sb.LastIndexOf(value, sb.Length -1);
        }

        public static int LastIndexOf(this StringBuilder sb, char value, int startIndex)
        {
            //remember to look backwards;
            for(int i = sb.Length - 1; i >= 0; i--)
            {
                if(sb[i] == value) return i;
            }

            return -1;
        }

        public static int LastIndexOf(this StringBuilder sb, string phrase)
        {
            sb[0] = 's';

            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf)
        {
            throw new NotImplementedException();
        }
    }
}