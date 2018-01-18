using System;
using System.Text;

namespace Nucleotic.Common.Extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Rolls the exception message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static string RollExceptionMessages(this Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(ex.Message);
            var inner = ex.InnerException;
            while (inner != null)
            {
                builder.AppendLine(inner.Message);
                inner = inner.InnerException;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Rolls the stack trace.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static string RollStackTraces(this Exception ex)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(ex.StackTrace);
            var inner = ex.InnerException;
            while (inner != null)
            {
                builder.AppendLine(inner.StackTrace);
                inner = inner.InnerException;
            }
            return builder.ToString();
        }
    }
}
