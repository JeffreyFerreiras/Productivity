using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Extensions.Conversion
{
    public static class ConversionEx
    {
        public static int ToInt32(this object o) => Convert.ToInt32(o);
        public static long ToInt64(this object o) => Convert.ToInt64(o);
        public static float ToFloat(this object o) => Convert.ToSingle(o);
        public static double ToDouble(this object o) => Convert.ToDouble(o);
        public static decimal ToDecimal(this object o) => Convert.ToDecimal(o);

        public static T ChangeType<T>(this object o)
        {
            return (T)Convert.ChangeType(o, typeof(T));
        }

        /// <summary>
        /// Converts string representation of enum to given type of enum
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string s, bool ignoreCase = true)
        {
            object item = s.ToEnum(typeof(T), ignoreCase); //Boxing because of compiler error with converting dynamic

            return (T)item;
        }

        /// <summary>
        /// Converts string representation of enum to given type of enum
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static dynamic ToEnum(this string s, Type type, bool ignoreCase = true)
        {
            Array modeTypes = type.GetTypeInfo().GetEnumValues();

            foreach(object mode in modeTypes)
            {
                if(ignoreCase && s.Equals(mode.ToString(), StringComparison.OrdinalIgnoreCase)
                    || !ignoreCase && s.Equals(mode.ToString()))
                {
                    return mode;
                }
            }

            throw new InvalidOperationException(
                "conversion failed because the input did" +
                " not match any enum value of the given type");
        }

        /// <summary>
        /// Converts an object to standard Date Time format MM/dd/yyyy h:mm:ss tt
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToDateString(this object o)
        {
            string format = "MM/dd/yyyy h:mm:ss tt";
            if(o is DateTime) return ((DateTime)o).ToString(format);
            if(o is string && o.IsValid()) return Convert.ToDateTime(o).ToString(format);

            return string.Empty;
        }

        /// <summary>
        /// Converts an object into a dictioanry of key value pair.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>

        public static IDictionary<string, dynamic> ToDictionary<T>(this T model) where T : class
        {
            Guard.AssertArgs(model.IsValid(), $"{typeof(T).Name} not valid");

            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

            foreach(var p in typeof(T).GetProperties())
            {
                if(!p.CanRead) continue;

                dynamic val = p.GetValue(model);

                dict[p.Name] = val;
            }

            return dict;
        }
        
        public static IDictionary<string, string> ToDictionary(this string text)
        {
            return text.ToDictionary(';');
        }

        public static IDictionary<string, string> ToDictionary(this string text, char seperator)
        {
            return text.ToDictionary(seperator, '=');
        }

        public static IDictionary<string, string> ToDictionary(this string text, char seperator, char keyValueSeperator)
        {
            if(!text.IsValid()) return new Dictionary<string, string>(0);

            string[] pairs = text.Split(seperator);
            string[] keyValue = new string[2];

            var dict = new Dictionary<string, string>(pairs.Length);

            for(int i = 0; i < pairs.Length; i++)
            {
                keyValue = pairs[i].Split(keyValueSeperator);
                dict.Add(keyValue[0], keyValue[1]);
            }

            return dict;
        }
    }
}