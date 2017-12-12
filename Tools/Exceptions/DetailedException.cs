using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Exceptions
{
    public class DetailedException : Exception
    {

        public DetailedException(string message) : base(message)
        { }
        public DetailedException(string message, Exception innerException) : base(message, innerException)
        {}

        public override string ToString()
        {
            Exception ex = this;
            StringBuilder sb = new StringBuilder();
            int count = 0;

            while (ex != null)
            {
                sb.AppendLine($"{++count}: {ex.GetType().Name} ---- Message: {ex.Message}");

                ex = ex?.InnerException;
            }

            sb.AppendLine($"\nShort Stack Trace: {this.StackTrace}");
            sb.AppendLine($"\nFull Stack Trace: {Environment.StackTrace}");

            return sb.ToString();
        }
    }
}
