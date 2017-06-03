using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Exceptions
{
    using Extensions;
    /// <summary>
    /// Exeption throwing class.
    /// </summary>
    public static class Guard
    {
        public static void ThrowIfInvalidArgs(params object[] oArry)
        {
            foreach (var o in oArry)
            {
                if (!o.IsValid())
                    throw new ArgumentException("Invlaid argument");
            }
        }
        
        public static void ThrowIfNullReference(params object [] oArry)
        {
            foreach (var o in oArry)
            {
                if (o == null)
                    throw new NullReferenceException($"{o.GetType().Name} object is null");
            }
        }
    }
}
