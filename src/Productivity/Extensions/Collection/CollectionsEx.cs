﻿using Productivity.Exceptions;
using Productivity.Extensions.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Productivity.Extensions.Collection
{
    public static class CollectionsEx
    {
        private static readonly Random SRandom = new Random();
        private static readonly object SSyncLock = new object();

        /// <summary>
        /// Rotates a collection to the right by the given off set
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <param name="offSet">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> Rotate<T>(this IEnumerable<T> source, int offSet)
        {
            AssertRotateParams(source, offSet);

            return source.Skip(offSet).Concat(source.Take(offSet));
        }

        /// <summary>
        /// Rotates a collection to the right by the given off set
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="arr">
        /// </param>
        /// <param name="offSet">
        /// </param>
        /// <returns>
        /// </returns>
        public static T[] Rotate<T>(this T[] arr, int offSet)
        {
            AssertRotateParams(arr, offSet);

            Array.Reverse(arr, 0, offSet);
            Array.Reverse(arr, offSet, arr.Count() - offSet);
            Array.Reverse(arr, 0, arr.Count());

            return arr;
        }

        private static void AssertRotateParams<T>(IEnumerable<T> source, int offSet)
        {
            Guard.AssertArgs(source != null, "source array is null");
            Guard.Assert<IndexOutOfRangeException>(offSet < source.Count(), "Out of range");
            Guard.Assert<IndexOutOfRangeException>(offSet > 0, "Out of range");
        }

        /// <summary>
        /// Returns a sub collection from the starting index to the end of the collection
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <param name="startIndex">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> SubSequence<T>(this IEnumerable<T> source, int startIndex)
        {
            int length = source.Count() - 1 - startIndex;

            return source.SubSequence(startIndex, length);
        }

        /// <summary>
        /// Returns a sub collection from the starting index to specified length of the collection
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <param name="startIndex">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> SubSequence<T>(this IEnumerable<T> source, int startIndex, int length)
        {
            Asserts();

            var sequence = new T[length];

            for (int i = 0; i < length; i++)
            {
                T elem = source.ElementAt(i + startIndex);

                sequence[i] = elem;
            }

            return sequence;

            void Asserts()
            {
                Guard.AssertArgs(source != null, nameof(source));
                Guard.Assert<IndexOutOfRangeException>(length <= source.Count() && length > 0, "Length out of bounds");
                Guard.Assert<IndexOutOfRangeException>(startIndex > 0 && startIndex < source.Count() - 1, "Start index out of bounds");
                Guard.AssertArgs(startIndex + 1 + length == source.Count(), "Sequence range out of bounds");
            }
        }

        /// <summary>
        /// Gets a IEnumerable <typeparamref name="T"/> underlying type.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>

        public static Type GetUnderlyingType<T>(this IEnumerable<T> source) => typeof(T);

        /// <summary>
        /// Gets a random value within a collection
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>

        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            Guard.AssertArgs(source.IsValid(), nameof(source));

            int index = 0;

            lock (SSyncLock)
            {
                index = SRandom.Next(0, source.Count() - 1);
            }

            return source.ElementAt(index);
        }

        /// <summary>
        /// Assigns matching key value pairs to object properties.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="dict">
        /// </param>
        /// <returns>
        /// </returns>
        public static T FromDictionary<T>(this IDictionary<string, object> dict)
        {
            return dict.FromDictionary(Activator.CreateInstance<T>());
        }

        /// <summary>
        /// Assigns matching key value pairs to object properties.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <typeparam name="TValue">
        /// </typeparam>
        /// <param name="dict">
        /// </param>
        /// <param name="model">
        /// </param>
        /// <returns>
        /// </returns>

        public static T FromDictionary<T>(this IDictionary<string, object> dict, T model)
        {
            Guard.AssertArgs(model.IsValid(), $"{typeof(T).Name} not valid");
            Guard.AssertArgs(dict.IsValid(), "Dictionary not valid");

            foreach (var prop in model.GetType().GetProperties())
            {
                if (!prop.CanWrite) continue;
                if (!dict.ContainsKey(prop.Name)) continue;

                object val = dict[prop.Name];

                try
                {
                    if (val.GetType() != prop.PropertyType && !(val is ICollection))
                    {
                        val = Convert.ChangeType(val, prop.PropertyType);
                    }

                    prop.SetValue(model, val);
                }
                catch (Exception)
                {


                }
            }

            return model;
        }

        /// <summary>
        /// Get value from Dictionary if key exists, otherwise returns default value
        /// </summary>
        /// <typeparam name="TKey">
        /// </typeparam>
        /// <typeparam name="TValue">
        /// </typeparam>
        /// <param name="dict">
        /// </param>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            Guard.AssertArgs(key.IsValid(), $"{nameof(key)} not valid");
            Guard.AssertArgs(dict.IsValid(), "Dictionary not valid");

            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            return default;
        }

        /// <summary>
        /// Provides paging for IEnumerable sources.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="source">
        /// data source
        /// </param>
        /// <param name="pageNum">
        /// page number
        /// </param>
        /// <param name="count">
        /// number of items per page
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNum, int count)
        {
            if (source == null) { return source; }

            pageNum -= 1; //offset page number
            int skipCount = pageNum * count;

            Guard.AssertArgs(pageNum >= 0, $"{nameof(pageNum)} must be >= 0");
            Guard.Assert<ArgumentOutOfRangeException>(skipCount >= 0, $"product of {nameof(pageNum)} and {nameof(count)} is out of range");

            return source.Skip(skipCount).Take(count);
        }

        public static IEnumerable<T> AsEnumerable<T>(this DataView dataView) where T : class, new()
        {
            var props = typeof(T).GetProperties();

            foreach (DataRow row in dataView.Table.Rows)
            {
                var model = new T();

                foreach (var prop in props)
                {
                    try
                    {
                        var rowValue = row[prop.Name];

                        prop.SetValue(model, Convert.ChangeType(rowValue, prop.PropertyType));
                    }
                    catch
                    {
                        // Ignored
                    }
                }

                yield return model;
            }
        }
    }
}