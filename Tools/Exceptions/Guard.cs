using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Exceptions
{
    using Extensions;

    public static class Guard
    {
        public static void ThrowIfInvalidArgs(object o)
        {
            if (!o.IsValid()) throw new ArgumentException("Invlaid argument");
        }

        public static void ThrowIfInvalidArgs(params object[] oArry)
        {
            foreach (var o in oArry) ThrowIfInvalidArgs(o);          
        }

        public static void ThrowIfNullReference(object o)
        {
            if (o == null) throw new NullReferenceException($"{o.GetType().Name} object is null");
        }

        public static void ThrowIfNullReference(params object [] oArry)
        {
            foreach (var o in oArry) ThrowIfNullReference(o);
        }
    }
}
