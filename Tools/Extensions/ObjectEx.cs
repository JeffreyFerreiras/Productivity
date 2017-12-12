using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tools.Exceptions;

namespace Tools.Extensions
{
    public static class ObjectEx
    {
        public static float ToFloat(this object o) => Convert.ToSingle(o);
        public static decimal ToDecimal(this object o) => Convert.ToDecimal(o);
        public static double ToDouble(this object o) => Convert.ToDouble(o);
        public static int ToInt32(this object o) => Convert.ToInt32(o);
        public static long ToInt64(this object o) => Convert.ToInt64(o);

        public static T ChangeType<T>(this object o)
        {
            return (T)Convert.ChangeType(o, typeof(T));
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

        public static string ToDateString(this object o)
        {
            string format = "MM/dd/yyyy h:mm:ss tt";
            if (o is DateTime) return ((DateTime)o).ToString(format);
            if (o is string && o.IsValid()) return Convert.ToDateTime(o).ToString(format);

            return string.Empty;
        }
    }
}
