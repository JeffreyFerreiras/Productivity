using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using Tools.Exceptions;
using Tools.Extensions.Validation;

namespace Tools.Extensions.Collection
{
    public static class CollectionsEx
    {
        private static readonly Random s_random = new Random();
        private static readonly object s_syncLock = new object();

        /// <summary>
        /// Returns a sub collection from the starting index to the end of the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static IEnumerable<T> SubSequence<T>(this IEnumerable<T> source, int startIndex)
        {
            int length = (source.Count() - 1) - startIndex;

            return source.SubSequence(startIndex, length);
        }

        /// <summary>
        /// Returns a sub collection from the starting index to specified length of the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<T> SubSequence<T>(this IEnumerable<T> source, int startIndex, int length)
        {
            ValidateSubSequenceArgs(source, startIndex, length);

            T[] sequence = new T[length];

            for(int i = 0; i < length; i++)
            {
                T elem = source.ElementAt(i + startIndex);

                sequence[i] = elem;
            }

            return sequence;
        }

        private static void ValidateSubSequenceArgs<T>(IEnumerable<T> source, int startIndex, int length)
        {
            Guard.AssertArgs(source != null, nameof(source));

            bool isValidSequenceRange = (startIndex + 1) + length == source.Count();

            Guard.Assert<IndexOutOfRangeException>(length <= source.Count() && length > 0, "Length out of bounds");
            Guard.Assert<IndexOutOfRangeException>(startIndex > 0 && startIndex < source.Count() - 1, "Start index out of bounds");
            Guard.AssertArgs(isValidSequenceRange, "Sequence range out of bounds");
        }

        /// <summary>
        /// Gets a IEnumerable <typeparamref name="T"/> underlying type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>

        public static Type GetUnderlyingType<T>(this IEnumerable<T> source) => typeof(T);

        /// <summary>
        /// Gets a random value within a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>

        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            Guard.AssertArgs(source.IsValid(), nameof(source));

            int index = 0;

            lock(s_syncLock)
            {
                index = s_random.Next(0, source.Count() - 1);
            }

            T element = source.ElementAt(index);

            return element.IsValid() ? element : default(T);
        }

        /// <summary>
        /// Assigns matching key value pairs to object properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="model"></param>
        /// <returns></returns>

        public static T FromDictionary<T>(this IDictionary<string, object> dict, T model)
        {
            Guard.AssertArgs(model.IsValid(), $"{typeof(T).Name} not valid");
            Guard.AssertArgs(dict.IsValid(), "Dictionary not valid");

            foreach(var prop in model.GetType().GetProperties())
            {
                if(!prop.CanWrite) continue;
                if(!dict.ContainsKey(prop.Name)) continue;

                object val = dict[prop.Name];

                if(val.GetType() != prop.PropertyType && !(val is ICollection))
                {
                    val = Convert.ChangeType(val, prop.PropertyType);
                }

                prop.SetValue(model, val);
            }

            return model;
        }

        /// <summary>
        /// Get value from Dictionary if key exists, otherwise returns default value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns ></returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            Guard.AssertArgs(key.IsValid(), $"{nameof(key)} not valid");
            Guard.AssertArgs(dict.IsValid(), "Dictionary not valid");

            if(dict.ContainsKey(key))
            {
                return dict[key];
            }

            return default(TValue);
        }
    }
}