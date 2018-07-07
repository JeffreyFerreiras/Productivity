using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Extensions.Reflection
{
    public static class Reflection
    {
        /// <summary>
        /// Returns true if source object contains a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasCollection<T>(this T source)
        {
            foreach (PropertyInfo propInfo in source.GetType().GetProperties())
            {
                Type type = propInfo.PropertyType.GetTypeInfo().GetInterface(nameof(ICollection));

                if (type != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get value of a property within object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static dynamic GetValueOf<T>(this T source, string property)
        {
            Guard.AssertArgs(source.IsValid(), nameof(source));
            Guard.AssertArgs(property.IsValid(), nameof(property));

            PropertyInfo propInfo = source.GetType().GetTypeInfo().GetProperty(property);

            if (propInfo != null && propInfo.CanRead)
            {
                dynamic val = propInfo.GetValue(source);

                return val;
            }

            return Activator.CreateInstance(propInfo.PropertyType); //Property does not have value, return default
        }

        /// <summary>
        /// Returns a deep copy of an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T source) where T : class
        {
            if (source == null) return null;

            if (source is ICollection<object> col)
            {
                return (T)DeepCloneCollection(col);
            }

            if (source is IDictionary dict)
            {
                return (T)DeepCloneDictionary(dict);
            }

            T clone = InternalDeepClone(source);

            return clone;
        }

        private static T InternalDeepClone<T>(T source) where T : class
        {
            MethodInfo memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
            T clone = (T)memberwiseClone.Invoke(source, null);

            foreach (FieldInfo field in source.GetType().GetRuntimeFields())
            {
                if (field.IsStatic) continue;
                if (field.FieldType.GetTypeInfo().IsPrimitive) continue;

                object sourceValue = field.GetValue(source);
                field.SetValue(clone, DeepClone(sourceValue));
            }

            return clone;
        }

        private static ICollection<object> DeepCloneCollection(ICollection<object> source)
        {
            object[] arr = (object[])Activator.CreateInstance(source.GetType(), new object[] { source.Count });

            for (int i = 0; i < source.Count; i++)
            {
                object original = source.ElementAt(i);
                object clone = DeepClone(original);

                arr[i] = clone;
            }

            return arr;
        }

        private static IDictionary DeepCloneDictionary(IDictionary dict)
        {
            IDictionary clone = (IDictionary)Activator.CreateInstance(dict.GetType());

            foreach (object pair in dict)
            {
                object key = pair.GetValueOf("Key");
                object original = pair.GetValueOf("Value");

                clone.Add(key, original.DeepClone());
            }

            return clone;
        }
    }
}