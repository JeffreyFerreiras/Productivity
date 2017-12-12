using System;

namespace Tools.Exceptions
{
    /// <summary>
    /// Exeption throwing class.
    /// </summary>
    public static class Guard
    {
        public static void Throw<TException>(bool assert, string message) where TException : Exception
        {
            if (!assert)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        public static void ThrowIfInvalidArgs(bool assert, string message)
        {
            if (!assert)
            {
                throw new ArgumentException($"Invlaid argument: {message}");
            }
        }

        public static void ThrowIfInvalidOperation(bool assert, string message)
        {
            if (!assert)
            {
                throw new InvalidOperationException($"Invalid operation: {message}");
            }
        }
    }
}