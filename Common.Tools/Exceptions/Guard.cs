using System;
using System.Diagnostics;
using System.Security.Authentication;

namespace Common.Tools.Exceptions
{
    /// <summary>
    /// Exception throwing class.
    /// </summary>
    public static class Guard
    {
        public static void Assert<TException>(bool assert, string message) where TException : Exception
        {
            if (!assert)
            {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        public static void AssertArgs(bool assert, string message)
        {
            if (!assert)
            {
                Assert<ArgumentException>(false, $"Invalid argument: {message}");
            }

            Debug.Assert(assert);
        }

        public static void AssertOperation(bool assert, string message)
        {
            if (!assert)
            {
                throw new InvalidOperationException($"Invalid operation: {message}");
            }
        }

        public static void AssertAuth(bool assert, string message = "Not logged in")
        {
            if (!assert)
            {
                throw new AuthenticationException(message);
            }
        }
    }
}