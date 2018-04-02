using System;

namespace Tools.Exceptions
{
    /// <summary>
    /// Exeption throwing class.
    /// </summary>
    public static class Guard
    {
        public static void Assert<TException>(bool assert, string message) where TException : Exception
        {
            if(!assert)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        public static void AssertArgs(bool assert, string message)
        {
            if(!assert)
            {
                Assert<ArgumentException>(assert, $"Invlaid argument: {message}");
            }
        }

        public static void AssertOperation(bool assert, string message)
        {
            if(!assert)
            {
                throw new InvalidOperationException($"Invalid operation: {message}");
            }
        }
    }
}