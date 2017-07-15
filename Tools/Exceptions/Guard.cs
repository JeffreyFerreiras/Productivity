using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Exceptions
{
    using Extensions;
    using System.Reflection;

    /// <summary>
    /// Exeption throwing class.
    /// </summary>
    public static class Guard
    {
        public static void ThrowIfInvalidArgs(bool assert, string msg)
        {
            if (!assert)
            {
                throw new ArgumentException($"Invlaid argument: {msg}");
            }
        }
        
        public static void ThrowIfInvalidOperation(bool assert, string msg)
        {
            if(!assert)
            {
                throw new InvalidOperationException($"Invalid operation: {msg}");
            }
        }
    }
}
