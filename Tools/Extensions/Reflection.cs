using System;
using System.Reflection;

namespace Tools.Extensions
{
    using Exceptions;

    public static class Reflection
    {
        /// <summary>
        /// Get value of a property within object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="property"></param>
        /// <returns></returns>

        public static dynamic GetValueOf<T>(this T value, string property)
        {
            Guard.ThrowIfInvalidArgs(value.IsValid(), nameof(value));
            Guard.ThrowIfInvalidArgs(property.IsValid(), nameof(property));

            PropertyInfo p = value.GetType().GetTypeInfo().GetProperty(property);

            if (p != null && p.CanRead)
            {
                dynamic val = p.GetValue(value);

                return val;
            }

            return Activator.CreateInstance(p.PropertyType); //Property does not have  value, return default
        }

        public static dynamic ToEnum(this string s, Type type)
        {
            Array modeTypes = type.GetTypeInfo().GetEnumValues();

            foreach (var mode in modeTypes)
            {
                if (s.IsNumber())
                {
                    int numVal = Convert.ToInt32(s);
                    if ((int)mode == numVal)
                        return mode;
                }

                if (s.ToLower() == mode.ToString().ToLower())
                    return mode;

                if (s.Length == 1)
                {
                    int ascii = Convert.ToChar(s);

                    if ((int)mode == ascii)
                        return mode;
                }
            }

            return null;
        }
    }
}