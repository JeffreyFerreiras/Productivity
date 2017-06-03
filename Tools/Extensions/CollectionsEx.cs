using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Tools.Extensions
{
    public static class CollectionsEx
    {
        /// <summary>
        /// Converts an object into a dictioanry of key value pair.
        /// The keys are strings matching the property names and the values match that properties value.
        /// If a key exists it will be overwritten.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IDictionary<string, TValue> ToDictionary<T, TValue>(this T item)
        {
            IDictionary<string, TValue> dict = new Dictionary<string, TValue>();

            foreach (var p in typeof(T).GetProperties())
            {
                if (!p.CanRead) continue;

                dynamic val = p.GetValue(item);
                
                dict[p.Name]=val;
            }

            return dict;
        }
    }
}
