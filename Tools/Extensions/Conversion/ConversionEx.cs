using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Extensions.Conversion
{
    public static class ConversionEx
    {
        public static int ToInt32(this object source) => Convert.ToInt32(source);

        public static long ToInt64(this object source) => Convert.ToInt64(source);

        public static float ToFloat(this object source) => Convert.ToSingle(source);

        public static double ToDouble(this object source) => Convert.ToDouble(source);

        public static decimal ToDecimal(this object source) => Convert.ToDecimal(source);

        public static T ChangeType<T>(this object source)
        {
            return (T)Convert.ChangeType(source, typeof(T));
        }

        /// <summary>
        /// Converts string representation of enum to given type of enum
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string source, bool ignoreCase = true)
        {
            object item = source.ToEnum(typeof(T), ignoreCase); //Boxing because of compiler error with converting dynamic

            return (T)item;
        }

        /// <summary>
        /// Converts string representation of enum to given type of enum
        /// </summary>
        /// <param name="source"></param>
        /// <param name="enumType"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static dynamic ToEnum(this string source, Type enumType, bool ignoreCase = true)
        {
            Array enumValues = enumType.GetTypeInfo().GetEnumValues();

            foreach (object enumValue in enumValues)
            {
                if (ignoreCase && source.EqualsIgnoreCase(enumValue.ToString()) || source.Equals(enumValue.ToString()))
                {
                    return enumValue;
                }
            }

            throw new InvalidOperationException(
                "conversion failed because the input did" +
                " not match any enum values");
        }

        /// <summary>
        /// Converts an int to DateTime (must be in format yyyyMMdd)
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this int num)
        {
            bool parsed = DateTime.TryParseExact(num.ToString(),
                "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime result);

            return parsed ? result : throw new InvalidOperationException("Invalid number format");
        }

        /// <summary>
        /// Converts an object to standard Date Time format MM/dd/yyyy h:mm:ss tt
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this object source)
        {
            string format = "MM/dd/yyyy h:mm:ss tt";

            if (source is DateTime) return ((DateTime)source).ToString(format);
            if (source is string && source.IsValid()) return Convert.ToDateTime(source).ToString(format);

            return string.Empty;
        }

        /// <summary>
        /// Converts an object into a Dictionary of key value pair
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>

        public static IDictionary<string, dynamic> ToDictionary<T>(this T model)
        {
            Guard.AssertArgs(model.IsValid(), $"{typeof(T).Name} not valid");

            var dict = new Dictionary<string, dynamic>();

            foreach (var p in typeof(T).GetProperties())
            {
                if (!p.CanRead) continue;

                dynamic val = p.GetValue(model);

                dict[p.Name] = val;
            }

            return dict;
        }

        /// <summary>
        /// Converts a string seperated by ';' into a dictionary
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionary(this string text)
        {
            return text.ToDictionary(';');
        }

        /// <summary>
        /// Converts a string seperated by <see cref="seperator"/> into a dictionary
        /// </summary>
        /// <param name="text"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionary(this string text, char seperator)
        {
            return text.ToDictionary(seperator, '=');
        }

        /// <summary>
        /// Converts a string seperated by <see cref="seperator"/> and key values seperated by <see cref="keyValueSeperator"/> into a dictionary
        /// </summary>
        /// <param name="text"></param>
        /// <param name="seperator"></param>
        /// <param name="keyValueSeperator"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionary(this string text, char seperator, char keyValueSeperator)
        {
            if (!text.IsValid()) return new Dictionary<string, string>(0);

            string[] pairs = text.Split(seperator);
            string[] keyValue = new string[2];

            var dict = new Dictionary<string, string>(pairs.Length);

            for (int i = 0; i < pairs.Length; i++)
            {
                keyValue = pairs[i].Split(keyValueSeperator);

                dict.Add(keyValue[0], keyValue[1]);
            }

            return dict;
        }

        public static char[] ToCharArray(this int[] numbers)
        {
            char[] chars = new char[numbers.Length];

            for (int i = 0; i < numbers.Length; i++)
            {
                chars[i] = (char)numbers[i];
            }

            return chars;
        }

        public static byte[] ToByteArray(this string text)
        {
            return System.Text.Encoding.ASCII.GetBytes(text);
        }

        public static bool FromYN(this string source)
        {
            source = source?.ToUpper();

            return source == "Y" || source == "YES" || source == "T" || source == "TRUE";
        }
    }
}