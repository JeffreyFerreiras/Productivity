using System;
using System.Reflection;

namespace Tools.Extensions.Reflection
{
    using Exceptions;
    using Extensions.Validation;

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
            Guard.AssertArgs(value.IsValid(), nameof(value));
            Guard.AssertArgs(property.IsValid(), nameof(property));

            PropertyInfo p = value.GetType().GetTypeInfo().GetProperty(property);

            if (p != null && p.CanRead)
            {
                dynamic val = p.GetValue(value);

                return val;
            }

            return Activator.CreateInstance(p.PropertyType); //Property does not have  value, return default
        }

        /// <summary>
        /// Returns a deep copy of an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T source) where T : class
        {
            if(source == null) return null;

            MethodInfo method = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
            T clone = (T)method.Invoke(source, null);

            foreach(FieldInfo field in source.GetType().GetRuntimeFields())
            {
                if(field.IsStatic) continue;
                if(field.FieldType.GetTypeInfo().IsPrimitive) continue;

                object sourceValue = field.GetValue(source);
                field.SetValue(clone, DeepClone(sourceValue));
            }

            return clone;
        }
    }
}