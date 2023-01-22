using System;
using System.Text;

namespace Productivity.Exceptions
{
    public class DetailedException : Exception
    {
        public DetailedException() : base()
        { }

        public DetailedException(string message) : base(message)
        { }

        public DetailedException(string message, Exception innerException) : base(message, innerException)
        { }

        public override string ToString()
        {
            Exception ex = this;
            var sb = new StringBuilder();

            int count = 0;

            while (ex != null)
            {
                sb.AppendLine($"{++count}: {ex.GetType().Name} ---- Message: {ex.Message}");

                ex = ex?.InnerException;
            }

            sb.AppendLine($"\nShort Stack Trace: {StackTrace}");
            sb.AppendLine($"\nFull Stack Trace:  {Environment.StackTrace}");

            return sb.ToString();
        }
    }
}