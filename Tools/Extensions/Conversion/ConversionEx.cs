using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Tools.Exceptions;
using Tools.Extensions.Validation;
using Tools.Text;

namespace Tools.Extensions.Conversion
{
    public static class ConversionEx
    {
        public static int ToInt32(this object source) => Convert.ToInt32(source);

        public static long ToInt64(this object source) => Convert.ToInt64(source);

        public static float ToFloat(this object source) => Convert.ToSingle(source);

        public static double ToDouble(this object source) => Convert.ToDouble(source);

        public static decimal ToDecimal(this object source) => Convert.ToDecimal(source);

        public static char ToChar(this string source) => Convert.ToChar(source);

        /// <summary>
        /// Change type to specified type (handles enums and string currency types)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ChangeType(object value, Type type)
        {
            if (type.IsEnum)
            {
                return value.ToEnum(type);
            }

            //it's money
            if (value is string stringValue && (type == typeof(decimal) || type == typeof(double) || type == typeof(float)))
            {
                stringValue = stringValue.CleanCurrency();

                if (stringValue.IsValidNumber())
                {
                    return Convert.ChangeType(stringValue, type);
                }
            }

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Clean currency value leaving just the numbers
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CleanCurrency(this string source)
        {
            if (source == null)
            {
                return "N/A";
            }

            return System.Text.RegularExpressions.Regex.Replace(source, @"[^0-9\.]+", "N/A");
        }

        /// <summary>
        /// Converts string or integer representation of enum to given type of enum
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="enumType">
        /// </param>
        /// <param name="ignoreCase">
        /// </param>
        /// <returns>
        /// </returns>
        public static dynamic ToEnum(this object source, Type enumType, bool ignoreCase = true)
        {
            Array enumValues = enumType.GetTypeInfo().GetEnumValues();

            foreach (object enumValue in enumValues)
            {
                if (source is string text
                   && (ignoreCase && text.EqualsIgnoreCase(enumValue.ToString()) || text.Equals(enumValue.ToString())))
                {
                    return enumValue;
                }
                else if (source.GetType().IsPrimitive)
                {
                    if ((int)source == (int)enumValue)
                    {
                        return enumValue;
                    }
                }
            }

            throw new InvalidOperationException(
                "conversion failed because the input did" +
                " not match any enum values");
        }

        /// <summary>
        /// Populate an object with anoher object's properties that share the same property name.
        /// </summary>
        /// <typeparam name="TSource">
        /// </typeparam>
        /// <typeparam name="TWith">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="changeType">
        /// If set to true the method will attempt to change the type if the property names match but
        /// the types don't
        /// </param>
        /// <returns>
        /// </returns>
        public static TSource PopulateWith<TSource, TWith>(this TSource source, TWith target, bool changeType = true)
        {
            PropertyInfo[] sourceProps = source.GetType().GetProperties();
            PropertyInfo[] withProps = target.GetType().GetProperties();

            foreach (PropertyInfo srcProp in sourceProps)
            {
                PropertyInfo withProp = withProps.SingleOrDefault(p => p.Name == srcProp.Name);

                object withValue = withProp?.GetValue(target);

                if (withValue != null && srcProp.CanWrite)
                {
                    if (withProp.PropertyType == srcProp.PropertyType)
                    {
                        srcProp.SetValue(source, withValue);
                    }
                    else if (changeType)
                    {
                        try
                        {   //High risk section, wrapping in try so entire conversion will not fail.
                            srcProp.SetValue(source, ChangeType(withValue, srcProp.PropertyType));
                        }
                        catch { }
                    }
                }
            }

            return source;
        }

        /// <summary>
        /// Create an object by using the matching properties in source
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Create<T>(this object source)
        {
            Guard.AssertArgs(source.IsValid(), "source is not valid");

            T target = Activator.CreateInstance<T>();

            return target.PopulateWith(source);
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
        /// Converts source to standard Date Time format MM/dd/yyyy h:mm:ss tt
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime source)
        {
            string format = "MM/dd/yyyy h:mm:ss tt";

            return source.ToString(format);
        }

        /// <summary>
        /// Converts source to standard Date Time format MM/dd/yyyy h:mm:ss tt
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this string source)
        {
            return Convert.ToDateTime(source).ToDateTimeString();
        }

        /// <summary>
        /// Converts an object into a Dictionary of key value pair
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>

        public static IDictionary<string, object> ToDictionary<T>(this T model)
        {
            Guard.AssertArgs(model.IsValid(), $"{typeof(T).Name} not valid");

            var dict = new Dictionary<string, dynamic>();

            foreach (var p in typeof(T).GetProperties())
            {
                if (!p.CanRead) continue;

                object val = p.GetValue(model);

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

        public static string ToCurrency(this string source)
        {
            if (!source.IsValid())
            {
                return source;
            }

            RegionInfo regionInfo = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID);

            source = Editor.Strip(source, regionInfo.ISOCurrencySymbol.ToChar(), ',');

            Guard.AssertArgs(source.IsValid(), "invalid amount string format");

            if (decimal.TryParse(source, out decimal result))
            {
                result.ToCurrency();
            }

            return source;
        }

        public static string ToCurrency(this double source)
        {
            return source.ToDecimal().ToCurrency();
        }

        public static string ToCurrency(this decimal source)
        {
            return source.ToString("C");
        }

        public static string ToYN(this bool source)
        {
            if (source)
            {
                return "Y";
            }

            return "N";
        }

        public static string NullToBlank(this object source)
        {
            if (source is string src)
            {
                return src;
            }

            return string.Empty;
        }
    }
}