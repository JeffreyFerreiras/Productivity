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
    }
}