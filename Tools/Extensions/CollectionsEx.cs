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
            Guard.ThrowIfInvalidArgs(item);

            IDictionary<string, dynamic> dict = new Dictionary<string, dynamic>();

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
