using Newtonsoft.Json;
using Productivity.Exceptions;
using Productivity.Extensions.Collection;
using Productivity.Extensions.Conversion;
using Productivity.Extensions.Validation;
using Productivity.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Productivity.Extensions.Conversion
{
    public static class ConversionEx
    {
        public static int ToInt32(this object source) => Convert.ToInt32(source);

        public static long ToInt64(this object source) => Convert.ToInt64(source);

        public static float ToFloat(this object source) => Convert.ToSingle(source);

        public static double ToDouble(this object source) => Convert.ToDouble(source);

        public static decimal ToDecimal(this object source) => Convert.ToDecimal(source);

        public static decimal FromMoneyToDecimal(this string source)
        {
            if (source.IsValid())
            {
                return source.CleanMoney().ToDecimal().Round();
            }

            return default;
        }

        public static bool ToBoolean(this object source) => Convert.ToBoolean(source);

        public static char ToChar(this object source) => Convert.ToChar(source);

        public static T ChangeType<T>(this object source)
        {
            return (T)ChangeType(source, typeof(T));
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsEnum)
            {
                return value.ToEnum(type);
            }

            //it's money
            if (value is string stringValue && (type == typeof(decimal) || type == typeof(double)))
            {
                stringValue = stringValue.CleanMoney();

                if (stringValue.IsValidNumber())
                {
                    return Convert.ChangeType(stringValue, type);
                }
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                return value;
            }

            return Convert.ChangeType(value, type);
        }

        public static string CleanMoney(this string source)
        {
            return string.IsNullOrEmpty(source?.Trim())
                ? "N/A"
                : Regex.Replace(source, @"[^0-9\.]+", "");
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
        /// <returns>
        /// </returns>
        public static TSource PopulateWith<TSource, TWith>(this TSource source, TWith target)
        {
            return source.PopulateWith(target, true);
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
        public static TSource PopulateWith<TSource, TWith>(this TSource source, TWith target, bool changeType)
        {
            return source.PopulateWith(target, changeType, true);
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
        /// <param name="ignoreCase">
        /// Ignore case matching for property names
        /// </param>
        /// <returns>
        /// </returns>
        public static TSource PopulateWith<TSource, TWith>(
            this TSource source,
            TWith target,
            bool changeType,
            bool ignoreCase
            )
        {
            var withProps = target.GetType()
                .GetProperties()
                .GroupBy(x => x.Name)
                .Select(g => g.First())
                .ToDictionary(x => GetKey(x.Name));

            foreach (PropertyInfo srcProp in source.GetType().GetProperties())
            {
                PropertyInfo withProp = withProps.TryGetValue(GetKey(srcProp.Name));

                if (withProp == null)
                {
                    continue;
                }

                object withValue = GetTargetValue(withProp);

                if (withValue == null || !srcProp.CanWrite)
                {
                    continue;
                }

                SetSourceValue(srcProp, withProp, withValue);

                withProps.Remove(GetKey(srcProp.Name));

                if (!withProps.Any())
                {
                    break;
                }
            }

            void SetSourceValue(PropertyInfo sourcePropInfo, PropertyInfo withPropInfo, object withValue)
            {
                object srcValue = sourcePropInfo.GetValue(source);

                if (withPropInfo.PropertyType == sourcePropInfo.PropertyType)
                {
                    bool isWithValueZero = !(withValue is string)
                        && withValue.IsNumber()
                        && withValue.ToInt64() == 0;

                    if (srcValue == null || !srcValue.Equals(withValue) && !isWithValueZero)
                    {
                        sourcePropInfo.SetValue(source, withValue);
                    }
                }
                else if (changeType)
                {
                    try
                    {
                        //High risk section, wrapping in try so entire conversion will not fail.
                        object typeChanged = ChangeType(withValue, sourcePropInfo.PropertyType);

                        if (srcValue == null || !srcValue.Equals(typeChanged))
                        {
                            sourcePropInfo.SetValue(source, typeChanged);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            object GetTargetValue(PropertyInfo withPropInfo)
            {
                if (!withPropInfo?.CanRead ?? true)
                {
                    return null;
                }

                try
                {
                    return withPropInfo.GetValue(target);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    return null;
                }
            }

            string GetKey(string name)
            {
                return ignoreCase ? name.ToLower() : name;
            }

            return source;
        }

        /// <summary>
        /// Create an object by using the matching properties in source
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public static T Create<T>(this object source)
        {
            Guard.AssertArgs(source.IsValid(), "source is not valid");

            T target = Activator.CreateInstance<T>();

            return target.PopulateWith(source);
        }

        /// <summary>
        /// Create an object by using the matching properties in source
        /// </summary>
        /// <param name="source">
        /// the source object containing the data to copy over
        /// </param>
        /// <param name="type">
        /// the type of object to create
        /// </param>
        /// <returns>
        /// </returns>
        public static dynamic Create(this object source, Type type)
        {
            Guard.AssertArgs(source.IsValid(), "source is not valid");

            object target = Activator.CreateInstance(type);

            return target.PopulateWith(source);
        }

        /// <summary>
        /// Converts string or integer representation of enum to given type of enum
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="ignoreCase">
        /// </param>
        /// <returns>
        /// </returns>
        public static T ToEnum<T>(this object source, bool ignoreCase = true)
        {
            object item = source.ToEnum(typeof(T), ignoreCase); //Boxing because of compiler error with converting dynamic

            return (T)item;
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
                else if (source is Enum enumSource)
                {
                    if (ignoreCase && enumSource.ToString().EqualsIgnoreCase(enumValue.ToString())
                        || enumSource.ToString().Equals(enumValue.ToString()))
                    {
                        return enumValue;
                    }
                }
                else if (source.IsNumber() && int.TryParse(source.ToString()?.Trim(), out int numValue))
                {
                    if (numValue == (int)enumValue)
                    {
                        return enumValue;
                    }
                }
                else if (source.IsNumber() && char.TryParse(source?.ToString()?.Trim(), out char character))
                {
                    int intValue = character;
                    if (intValue == (int)enumValue)
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
        /// Converts an int to DateTime (must be in format yyyyMMdd)
        /// </summary>
        /// <param name="num">
        /// </param>
        /// <returns>
        /// </returns>
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
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
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
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="model">
        /// </param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, object> ToDictionary<T>(this T model)
        {
            Guard.AssertArgs(model.IsValid(), $"{typeof(T).Name} not valid");

            var dict = new Dictionary<string, object>();

            foreach (var p in model.GetType().GetProperties())
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
        /// <param name="text">
        /// </param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, string> ToDictionary(this string text)
        {
            return text.ToDictionary(';');
        }

        /// <summary>
        /// Converts a string seperated by <see cref="seperator"/> into a dictionary
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <param name="seperator">
        /// </param>
        /// <returns>
        /// </returns>
        public static IDictionary<string, string> ToDictionary(this string text, char seperator)
        {
            return text.ToDictionary(seperator, '=');
        }

        /// <summary>
        /// Converts a string seperated by <see cref="seperator"/> and key values seperated by <see
        /// cref="keyValueSeperator"/> into a dictionary
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <param name="seperator">
        /// </param>
        /// <param name="keyValueSeperator">
        /// </param>
        /// <returns>
        /// </returns>
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

        public static string ToBase64String(this string text)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(text);
            return Convert.ToBase64String(bytes);
        }

        public static string FromBase64String(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Turns an object with the [Serializable] attribute into a byte array
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] ToByteArray<T>(this T source)
        {
            var binForm = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                binForm.Serialize(ms, source);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(this byte[] source)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();

                memStream.Write(source, 0, source.Length);
                memStream.Seek(0, SeekOrigin.Begin);

                var obj = binForm.Deserialize(memStream);

                if (obj is T src)
                {
                    return src;
                }

                return default;
            }
        }

        public static bool FromYn(this string source)
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

            var regionInfo = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID);

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

        public static string NullToBlank(this object source)
        {
            if (source is string src)
            {
                return src;
            }

            return string.Empty;
        }

        public static string ToYn(this bool source)
        {
            if (source)
            {
                return "Y";
            }

            return "N";
        }

        public static T[] GetEnumValues<T>(this T enumValue) where T : struct, IConvertible
        {
            var enums = typeof(T).GetEnumValues();
            var ret = new T[enums.Length];
            int index = 0;
            foreach (object e in enums)
            {
                ret[index] = (T)e;
                index++;
            }

            return ret;
        }

        public static string ToInputDate(this DateTime date) => date.ToString("yyyy-MM-dd");

        public static IEnumerable<T> From<T>(this DataView dataView, int pageNum, int pageCount)
        {
            int recordCount = dataView.Table.Rows.Count;

            SyncPageNum();

            int startIndex = pageNum * pageCount - pageCount;
            int endIndex = Math.Min(pageNum * pageCount, recordCount);

            for (int i = startIndex; i < endIndex; i++)
            {
                yield return (T)BuildModel(dataView.Table.Rows[i]);
            }

            object BuildModel(DataRow row)
            {
                var model = Activator.CreateInstance(typeof(T));

                foreach (var prop in typeof(T).GetProperties())
                {
                    if (!row.Table.Columns.Contains(prop.Name))
                    {
                        continue;
                    }

                    SetValue(prop, model, row[prop.Name]);
                }

                return model;
            }

            void SetValue(PropertyInfo prop, object model, object value)
            {
                if (value is string str && prop.PropertyType == typeof(bool) && (str?.ToUpper() == "Y" || str?.ToUpper() == "N"))
                {
                    prop.SetValue(model, str.FromYn());
                }
                else if (
                       prop.PropertyType.IsEnum
                    && char.TryParse(value?.ToString()?.Trim(), out char character)
                    && IsEnumMatch(prop.PropertyType, character)
                    )
                {
                    int charIntValue = character;
                    prop.SetValue(model, charIntValue.ToEnum(prop.PropertyType));
                }
                else
                {
                    TrySetValue(prop, model, value);
                }
            }

            void SyncPageNum()
            {
                if (recordCount != 0 && pageCount > 0)
                {
                    int availablePageCount = (int)Math.Ceiling(recordCount / (double)pageCount);

                    if (availablePageCount < pageNum)
                    {
                        pageNum = availablePageCount > 0 ? availablePageCount : 1;
                    }
                }
            }

            void TrySetValue(PropertyInfo prop, object model, object value)
            {
                try
                {
                    prop.SetValue(model, ChangeType(value, prop.PropertyType));
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static bool IsEnumMatch(Type enumType, int value)
        {
            foreach (int enumValue in Enum.GetValues(enumType))
            {
                if (value == enumValue)
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToJson<T>(this T source)
        {
            return JsonConvert.SerializeObject(source, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Culture = CultureInfo.CurrentCulture
            });
        }
    }
}