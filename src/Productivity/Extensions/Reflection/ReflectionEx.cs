using Productivity.Exceptions;
using Productivity.Extensions.Conversion;
using Productivity.Extensions.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Productivity.Extensions.Reflection
{
    public static class ReflectionEx
    {
        /// <summary>
        /// Returns true if source object contains a collection.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
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
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <param name="property">
        /// </param>
        /// <returns>
        /// </returns>
        public static object GetValueOf<T>(this T source, string property)
        {
            Guard.AssertArgs(source.IsValid(), nameof(source));
            Guard.AssertArgs(property.IsValid(), nameof(property));

            PropertyInfo propInfo = source.GetType().GetTypeInfo().GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propInfo == null || !propInfo.CanRead)
            {
                return null;
            }

            object val = propInfo.GetValue(source);

            return val;
        }

        /// <summary>
        /// Returns a deep copy of an object.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
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
            if (source is string s)
            {
                return s as T;
            }

            MethodInfo memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
            var clone = (T)memberwiseClone.Invoke(source, null);

            if (typeof(T).IsPrimitive || typeof(T).IsValueType) return clone;

            foreach (FieldInfo field in source.GetType().GetRuntimeFields())
            {
                if (field.IsStatic) continue;
                if (field.FieldType.GetTypeInfo().IsPrimitive) continue;

                object sourceValue = field.GetValue(source);
                field.SetValue(clone, sourceValue.DeepClone());
            }

            return clone;
        }

        private static ICollection<object> DeepCloneCollection(ICollection<object> source)
        {
            object[] arr = (object[])Activator.CreateInstance(source.GetType(), new object[] { source.Count });

            for (int i = 0; i < source.Count; i++)
            {
                object original = source.ElementAt(i);
                object clone = original.DeepClone();

                arr[i] = clone;
            }

            return arr;
        }

        private static IDictionary DeepCloneDictionary(IDictionary dict)
        {
            var clone = (IDictionary)Activator.CreateInstance(dict.GetType());

            foreach (object pair in dict)
            {
                object key = pair.GetValueOf("Key");
                object original = pair.GetValueOf("Value");

                clone.Add(key, original.DeepClone());
            }

            return clone;
        }

        public static void SetPropertyValue(this object source, string propertyName, object value)
        {
            Guard.AssertArgs(source != null, nameof(source));

            var propertyInfo = source.GetType().GetProperty(propertyName);

            Guard.AssertOperation(propertyInfo != null, "Property name was not found in source object");

            if (propertyInfo.CanWrite && propertyInfo.Name == propertyName)
            {
                propertyInfo.SetValue(propertyName, value);
            }
            else
            {
                throw new InvalidOperationException("Cannot write to object");
            }
        }

        public static object Combine(this object source, object expand)
        {
            if (!expand.IsValid())
            {
                return source;
            }

            var dictionarySource = source is ExpandoObject ? source as IDictionary<string, object> : source.ToDictionary();
            var dictionaryExpand = expand is ExpandoObject ? expand as IDictionary<string, object> : expand.ToDictionary();
            var result = new ExpandoObject();
            var dictionaryExpando = result as IDictionary<string, object>; //work with the Expando as a Dictionary

            foreach (var pair in dictionarySource.Concat(dictionaryExpand))
            {
                dictionaryExpando[pair.Key] = pair.Value;
            }

            return result;
        }

        public static string ResolvedLabelText(this MemberInfo member)
        {
            DisplayAttribute attribute = member
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault();

            string resolvedLabelText = attribute?.Name;

            if (resolvedLabelText != null)
            {
                return resolvedLabelText;
            }

            DisplayNameAttribute displayNameAttribute = member
                .GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .Cast<DisplayNameAttribute>()
                .FirstOrDefault();

            resolvedLabelText = displayNameAttribute?.DisplayName;

            return resolvedLabelText;
        }

        public static bool IsBrowsable(this MemberInfo propertyInfo)
        {
            BrowsableAttribute attribute = propertyInfo
                .GetCustomAttributes(typeof(BrowsableAttribute), false)
                .Cast<BrowsableAttribute>()
                .FirstOrDefault();

            return attribute?.Browsable ?? true;
        }
    }
}