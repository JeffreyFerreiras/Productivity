using System;
using System.Collections.Generic;
using System.Text;
using Tools.Exceptions;

namespace Tools.Extensions.Text
{
    public static class StringBuilderEx
    {
        private const string OutOfBoundsMessage = "Target out of bounds of string builder length from start index";

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

            if(phrase.Length > 0)
            {
                int length = startIndex + count;
                BoundsCheck(sb, startIndex, count, length);

                return GetIndexOf(sb, phrase, startIndex, length);
            }

            return -1;
        }

        private static int GetIndexOf(StringBuilder sb, string phrase, int startIndex, int length)
        {
            for(int i = startIndex; i < length; i++)
            {
                if(sb[i] != phrase[0]) continue;

                if(IsIndexOfMatch(sb, phrase, ref i))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool IsIndexOfMatch(StringBuilder sb, string phrase, ref int index)
        {
            bool isMatch = true;

            for(int j = 1; j < phrase.Length; j++)
            {
                if(phrase[j] != sb[index + j])
                {
                    isMatch = false;
                    index += j;

                    break;
                }
            }

            return isMatch;
        }

        public static int IndexOfAny(this StringBuilder sb, char[] anyOf)
        {
            return sb.IndexOfAny(anyOf, 0);
        }

        public static int IndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex)
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
            return sb.LastIndexOf(value, 0);
        }

        public static int LastIndexOf(this StringBuilder sb, char value, int startIndex)
        {
            int length = sb?.Length ?? 0;
            int count = length - (startIndex + 1);

            return sb.LastIndexOf(value, startIndex, count);
        }

        public static int LastIndexOf(this StringBuilder sb, char value, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            int startPos = startIndex + count;

            BoundsCheck(sb, startIndex, count, startPos + 1);

            for(int i = startPos; i >= startIndex; i--)
            {
                if(sb[i] == value) return i;
            }

            return -1;
        }

        public static int LastIndexOf(this StringBuilder sb, string phrase)
        {
            return sb.LastIndexOf(phrase, 0);
        }

        public static int LastIndexOf(this StringBuilder sb, string phrase, int startIndex)
        {
            int length = sb?.Length ?? 0;
            int count = length - (startIndex + 1);

            return sb.LastIndexOf(phrase, startIndex, count);
        }

        public static int LastIndexOf(this StringBuilder sb, string phrase, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");
            Guard.AssertArgs(phrase != null, nameof(phrase));

            if(phrase.Length > 0)
            {
                int startPos = startIndex + count;

                BoundsCheck(sb, startIndex, count, startPos + 1);

                return GetLastIndexOf(sb, phrase, startIndex, startPos);
            };

            return -1;
        }

        private static int GetLastIndexOf(StringBuilder sb, string phrase, int startIndex, int startPos)
        {
            for(int i = startPos; i >= startIndex; i--)
            {
                if(sb[i] != phrase[phrase.Length - 1]) continue;

                if(IsLastIndexOfMatch(sb, phrase, ref i))
                {
                    return i;
                }
            }

            return -1;
        }

        private static bool IsLastIndexOfMatch(StringBuilder sb, string phrase, ref int index)
        {
            bool isMatch = true;

            for(int j = phrase.Length - 2; j >= 0; j--)
            {
                if(sb[--index] != phrase[j])
                {
                    isMatch = false;
                    break;
                }
            }

            return isMatch;
        }

        public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf)
        {
            return sb.LastIndexOfAny(anyOf, 0);
        }

        public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex)
        {
            int length = sb?.Length ?? 0;
            int count = length - (startIndex + 1);

            return sb.LastIndexOfAny(anyOf, startIndex, count);
        }

        public static int LastIndexOfAny(this StringBuilder sb, char[] anyOf, int startIndex, int count)
        {
            Guard.AssertArgs(sb != null, "StringBuilder is null");

            int startPos = startIndex + count;
            BoundsCheck(sb, startIndex, count, startPos + 1);
            HashSet<char> anyOfSet = new HashSet<char>(anyOf); //128 chars max

            for(int i = startPos; i >= startIndex; i--)
            {
                if(anyOfSet.Contains(sb[i])) return i;
            }

            return -1;
        }

        private static void BoundsCheck(StringBuilder sb, int startIndex, int count, int length)
        {
            Guard.Assert<IndexOutOfRangeException>(startIndex >= 0 && sb.Length > startIndex, OutOfBoundsMessage);
            Guard.Assert<IndexOutOfRangeException>(sb.Length >= count, OutOfBoundsMessage);
            Guard.Assert<IndexOutOfRangeException>(sb.Length >= length, OutOfBoundsMessage);
        }
    }
}