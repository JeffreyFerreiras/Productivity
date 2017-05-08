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
            if (!o.IsValid())
            {
                throw new ArgumentNullException("Invlaid argument");
            }
        }
        public static void ThrowIfInvalidArgs(params object[] oArry)
        {
            ThrowIfInvalidArgs(oArry);
            foreach (var o in oArry)
            {
                ThrowIfInvalidArgs(o);
            }
        }
    }
}
