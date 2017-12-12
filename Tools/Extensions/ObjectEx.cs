using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
