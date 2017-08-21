using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Tools.Extensions
{
    using Exceptions;

    public static class CollectionsEx
    {
        /// <summary>
        /// Converts an object into a dictioanry of key value pair.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        
        public static IDictionary<string, dynamic> ToDictionary<T>(this T item)
        {
            Guard.ThrowIfInvalidArgs(item.IsValid(), $"{typeof(T)} not valid");

            IDictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

            foreach (var p in typeof(T).GetProperties())
            {
                if (!p.CanRead) continue;

                dynamic val = p.GetValue(item);
                
                dict[p.Name]=val;
            }

            return dict;
        }

        public static T FromDictionary<T, TValue>(this IDictionary<string, TValue> dict, T item)
        {
            Guard.ThrowIfInvalidArgs(item.IsValid(), $"{typeof(T)} not valid");

            foreach (var p in typeof(T).GetProperties())
            {
                TValue val = dict.TryGetValue(p.Name);

                if (p.CanWrite && val.IsValid())
                {
                    p.SetValue(item, val);
                }
            }

            return item;
        }

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey index)
        {
            if (dict.ContainsKey(index))
            {
                return dict[index];
            }

            return default(TValue);
        }
    }
}
