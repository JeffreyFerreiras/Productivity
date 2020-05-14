using Common.Tools.Exceptions;
using Common.Tools.Extensions.Conversion;
using Common.Tools.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Tools.Extensions.Validation
{
    /// <summary>
    /// Provides common validation functionality.
    /// </summary>

    public static class Validation
    {
        public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);

        /// <summary>
        /// Indicates whether URL is well-formed and is not required to be further escaped.
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <returns>
        /// A System.Boolean value that is true if the string was well-formed; else false.
        /// </returns>
        public static bool IsValidUrl(this string url)
        {
            try
            {
                return new Uri(url)?.IsWellFormedOriginalString() ?? false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Confirms if a given object is a number.
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsNumber(this object value)
        {
            if (value is string || value is char) return value.ToString().IsValidNumber();

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

        /// <summary>
        /// Confirms if a given string is a number.
        /// </summary>
        /// <param name="num">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsValidNumber(this string num)
        {
            if (string.IsNullOrWhiteSpace(num))
            {
                return false;
            }

            num = num.Trim();

            if (num[0] == '-')
            {
                num = num.Substring(1);
            }

            int periodCount = 0;

            foreach (char c in num)
            {
                if (c == '.')
                {
                    periodCount++;

                    if (periodCount > 1)
                    {
                        return false;
                    }

                    continue;
                }

                if (c >= 48 && c <= 57)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates all properties
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasValidProperties<T>(this T value)
        {
            if (value == null)
            {
                return false;
            }

            foreach (var m in value.GetType().GetTypeInfo().GetProperties())
            {
                if (!m.GetValue(value).IsValid())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks a value for null, negative numbers, empty strings, count of a collection not zero
        /// and not DBNull
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsValid<T>(this T value)
        {
            if (value == null) return false;
            if (value.IsNumber()) return Convert.ToDecimal(value) > -1;

            switch (value)
            {
                case string _:
                    return !string.IsNullOrWhiteSpace(value.ToString());

                case IEnumerable<object> col:
                    return col.Any();
                case DBNull _:
                    return false;
                case DateTime dateTime:
                    return dateTime != default;
                default:
                    return true;
            }
        }

        public static bool HasOnlyLetters(this string letters)
        {
            for (int i = 0; i < letters.Length; i++)
            {
                if (!char.IsLetter(letters, i))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool HasCharType(this string pw, Predicate<char> predicate, int count = 1)
        {
            foreach (char letter in pw)
            {
                if (predicate(letter)) count--;
                if (count <= 0) return true;
            }

            return false;
        }

        public static bool HasUpper(this string pw, int count = 1)
        {
            bool Predicate(char c) => char.IsUpper(c);

            return pw.HasCharType(Predicate, count);
        }

        public static bool HasLower(this string pw, int count = 1)
        {
            bool Predicate(char c) => char.IsLower(c);

            return pw.HasCharType(Predicate, count);
        }

        public static bool HasNumber(this string pw, int count = 1)
        {
            bool Predicate(char c) => char.IsDigit(c);

            return pw.HasCharType(Predicate, count);
        }

        public static bool HasProp<T>(this T value, string propName)
        {
            PropertyInfo property = value.GetType().GetTypeInfo().GetProperty(propName);

            return property != null;
        }

        /// <summary>
        /// Checks if the source type has all the properties from the given target
        /// </summary>
        /// <typeparam name="TType">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasProps<TType>(this object source)
        {
            return source.HasProps<TType>(true, false);
        }

        /// <summary>
        /// Checks if the source type has all the properties from the given target
        /// </summary>
        /// <typeparam name="TType">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <param name="ignoreCase">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasProps<TType>(this object source, bool ignoreCase)
        {
            return source.HasProps<TType>(ignoreCase, false);
        }

        /// <summary>
        /// Checks if the source type has all the properties from the given target
        /// </summary>
        /// <typeparam name="TSource">
        /// </typeparam>
        /// <typeparam name="TType">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <param name="ignoreCase">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasProps<TType>(this object source, bool ignoreCase, bool matchType)
        {
            var typePropDict = ignoreCase
                ? typeof(TType).GetProperties().ToDictionary(p => p.Name, StringComparer.CurrentCultureIgnoreCase)
                : typeof(TType).GetProperties().ToDictionary(p => p.Name);
            var sourceProps = source.GetType().GetProperties();

            return sourceProps.All(prop =>
            {
                if (typePropDict.ContainsKey(prop.Name))
                {
                    if (matchType)
                    {
                        return prop.PropertyType == typePropDict[prop.Name].PropertyType;
                    }

                    return true;
                }

                return false;
            });
        }

        public static bool Matches(this string source, string target, bool ignoreCase)
        {
            if (source == null || target == null)
            {
                return source == target;
            }

            return ignoreCase
                ? source.Equals(target, StringComparison.OrdinalIgnoreCase)
                : source.Equals(target);
        }

        /// <summary>
        /// Compares two string using <see cref="StringComparison.OrdinalIgnoreCase"/>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool EqualsIgnoreCase(this string source, string target)
        {
            if (source == null) return false;

            bool areEqual = source.Equals(target, StringComparison.OrdinalIgnoreCase);

            return areEqual;
        }

        /// <summary>
        /// Returns true if a collection is sorted in ascending order
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsSortedAsc<T>(this IEnumerable<T> source)
        {
            return source.IsSortedAsc(Comparer<T>.Default);
        }

        /// <summary>
        /// Returns true if a collection is sorted in ascending order using provided <see cref="IComparer{T}"/>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsSortedAsc<T>(this IEnumerable<T> source, IComparer<T> comparer)
        {
            Guard.AssertArgs(source != null, "Collection is null");

            for (int i = 1; i < source.Count(); i++)
            {
                if (comparer.Compare(source.ElementAt(i - 1), source.ElementAt(i)) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if an ABA number is valid by applying the MOD 10 algorithm
        /// </summary>
        /// <param name="aba">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsValidAba(this string aba)
        {
            if (string.IsNullOrWhiteSpace(aba) || aba.Length != 9)
            {
                return false;
            }

            const string abaDigitWeightString = "37137137";
            bool isValidStart = aba[0] != '4' && aba[0] != '5'; //Can't start with 4 or 5

            if (isValidStart && aba != "000000000" && aba.IsNumber())
            {
                int checkSum = 0;

                for (int i = 1; i < 8; i++)
                {
                    checkSum += aba[i].ToString().ToInt32() * abaDigitWeightString[i].ToString().ToInt32();
                }

                int remainder = checkSum % 10;
                int checkDigit = remainder == 0 ? 0 : 10 - remainder;

                if (checkDigit == aba[8].ToString().ToInt32())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a card number is valid by applying the Credit/Debit card MOD 10 Algorithm
        /// </summary>
        /// <param name="ccnum">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsValidCard(this string ccnum)
        {
            ccnum = Editor.Strip(ccnum, '-');

            if (string.IsNullOrWhiteSpace(ccnum) && !ccnum.IsNumber())
            {
                return false;
            }

            int total = 0, multiplier = 0;

            for (int i = ccnum.Length - 1; i >= 1; i -= 2)
            {
                total += ccnum[i].ToString().ToInt32();

                multiplier = ccnum[i - 1].ToString().ToInt32() * 2;
                multiplier = multiplier >= 10 ? multiplier / 10 : multiplier;

                total += multiplier;

                if (multiplier / 10 > 0)
                {
                    total += multiplier >= 10 ? multiplier % 10 : multiplier;
                }
            }

            if (ccnum.Length % 2 != 0)
            {
                total += ccnum[0].ToString().ToInt32();
            }

            return total % 10 == 0;
        }
        public static bool IsValidCheckNumber(this string checkNumber)
        {
            return checkNumber.Skip(1).All(x => x.IsNumber());
        }

        public static bool DeepEquals<T>(this T source, T target)
        {
            Guard.AssertArgs(source.IsValid(), "Invalid source");
            Guard.AssertArgs(target.IsValid(), "Invalid target");

            var type = typeof(T);
            var propInfo = source.GetType().GetProperties();

            if (type.IsPrimitive || source is string)
            {
                return source.Equals(target);
            }

            foreach (var prop in propInfo)
            {
                var src = prop.GetValue(source);
                var trg = prop.GetValue(target);

                if (src == trg) //making sure to continue in case both are null
                {
                    continue;
                }

                if (src is ICollection)
                {
                    if (!InitCollectionMatches((ICollection)trg, (ICollection)src))
                    {
                        return false;
                    }
                }
                else if (src is string && !src.Equals(trg))
                {
                    return false;
                }
                else if (!(src is string) && !src.Equals(trg))
                {
                    if (!src.DeepEquals(trg))
                    {
                        return false;
                    }
                }
            }

            return true;

            bool InitCollectionMatches(ICollection trg, ICollection src)
            {
                if (trg.Count != src.Count)
                {
                    return false;
                }

                object[] targetArr = new object[trg.Count];
                object[] sourceArr = new object[src.Count];

                int index = 0;
                foreach (var item in trg)
                {
                    targetArr[index++] = item;
                }

                index = 0;
                foreach (var item in src)
                {
                    sourceArr[index++] = item;
                }

                for (int i = 0; i < targetArr.Length; i++)
                {
                    if (!sourceArr[i].DeepEquals(targetArr[i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}