using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools.Exceptions;

namespace Tools.Extensions.Validation
{
    /// <summary>
    /// Provides common validation functionality.
    /// </summary>

    public static class Validation
    {
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        public static bool IsNumber(this object value)
        {
            if(value is string) return value.ToString().IsValidNumber();

            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }

        public static bool IsValidNumber(this string num)
        {
            if(string.IsNullOrWhiteSpace(num))
            {
                return false;
            }

            if(num[0] == '-')
            {
                num = num.Substring(1);
            }

            foreach(var c in num)
            {
                if(c >= 48 && c <= 57) continue;

                return false;
            }

            return true;
        }

        public static bool HasValidProperties<T>(this T value)
        {
            if(value == null)
            {
                return false;
            }

            foreach(var m in value.GetType().GetTypeInfo().GetProperties())
            {
                if(!m.GetValue(value).IsValid())
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsValid<T>(this T value)
        {
            if(value == null) return false;
            if(value.IsNumber()) return Convert.ToInt32(value) > -1;
            if(value is string) return !string.IsNullOrWhiteSpace(value.ToString());
            if(value is IEnumerable<object> col) return col.Any();
            if(value is DBNull) return false;

            return true;
        }

        public static bool HasOnlyLetters(this string letters)
        {
            for(int i = 0; i < letters.Length; i++)
            {
                if(!char.IsLetter(letters, i))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasCharType(this string pw, Predicate<char> predicate, int count = 1)
        {
            foreach(char letter in pw)
            {
                if(predicate(letter)) count--;
                if(count <= 0) return true;
            }

            return false;
        }

        public static bool HasUpper(this string pw, int count = 1)
        {
            bool predicate(char c) => char.IsUpper(c);

            return pw.HasCharType(predicate, count);
        }

        public static bool HasLower(this string pw, int count = 1)
        {
            bool predicate(char c) => char.IsLower(c);

            return pw.HasCharType(predicate, count);
        }

        public static bool HasNumber(this string pw, int count = 1)
        {
            bool predicate(char c) => char.IsDigit(c);

            return pw.HasCharType(predicate, count);
        }

        public static bool HasProp<T>(this T value, string propName)
        {
            PropertyInfo property = value.GetType().GetTypeInfo().GetProperty(propName);

            return property != null;
        }

        /// <summary>
        /// Compares two string using <see cref="StringComparison.OrdinalIgnoreCase"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string source, string target)
        {
            if(source == null) return false;

            bool areEqual = source.Equals(target, StringComparison.OrdinalIgnoreCase);

            return areEqual;
        }

        /// <summary>
        /// Returns true if a collection is sorted in ascending order
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsSortedAsc<T>(this IEnumerable<T> source)
        {
            return source.IsSortedAsc(Comparer<T>.Default);
        }

        /// <summary>
        /// Returns true if a collection is sorted in ascending order using provided <see cref="IComparer{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsSortedAsc<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            Guard.AssertArgs(source != null, "Collection is null");

            for(int i = 1; i < source.Count(); i++)
            {
                if(comparer.Compare(source.ElementAt(i - 1), source.ElementAt(i)) > 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}