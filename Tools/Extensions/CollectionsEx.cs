using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tools.Extensions
{
    using Exceptions;

    public static class CollectionsEx
    {
        private static readonly Random _random = new Random();
        private static readonly object _syncLock = new object();

        /// <summary>
        /// Gets a random value within a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>

        public static T GetRandomElement<T>(this ICollection<T> collection)
        {
            int index = 0;

            lock (_syncLock)
            {
                index = _random.Next(0, collection.Count - 1);
            }

            T element = collection.ElementAt(index);

            return element.IsValid() ? element : default(T);
        }

        /// <summary>
        /// Converts an object into a dictioanry of key value pair.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>

        public static IDictionary<string, dynamic> ToDictionary<T>(this T model)
        {
            Guard.ThrowIfInvalidArgs(model.IsValid(), $"{typeof(T).Name} not valid");

            IDictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

            foreach (var p in typeof(T).GetProperties())
            {
                if (!p.CanRead) continue;

                dynamic val = p.GetValue(model);

                dict[p.Name] = val;
            }

            return dict;
        }

        /// <summary>
        /// Assigns matching key value pairs to object properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T FromDictionary<T, TValue>(this IDictionary<string, TValue> dict, T model)
        {
            Guard.ThrowIfInvalidArgs(model.IsValid(), $"{typeof(T).Name} not valid");
            Guard.ThrowIfInvalidArgs(dict.IsValid(), "Dictionary not valid");

            foreach (var p in typeof(T).GetProperties())
            {
                TValue val = dict.TryGetValue(p.Name);

                if (p.CanWrite && val.IsValid())
                {
                    p.SetValue(model, val);
                }
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
        /// <returns></returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            Guard.ThrowIfInvalidArgs(key.IsValid(), $"{nameof(key)} not valid");
            Guard.ThrowIfInvalidArgs(dict.IsValid(), "Dictionary not valid");

            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            return default(TValue);
        }
    }
}