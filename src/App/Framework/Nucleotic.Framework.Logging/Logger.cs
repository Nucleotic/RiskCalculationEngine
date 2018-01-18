using Common.Logging;
using Nucleotic.Framework.Logging.LogEntries;
using System;
using System.Diagnostics;

namespace Nucleotic.Framework.Logging
{
    public class Logger<T> where T : class
    {
        private readonly ILog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger{T}"/> class.
        /// </summary>
        public Logger()
        {
            _log = LogManager.GetLogger<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger{T}"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public Logger(ILog log)
        {
            _log = log;
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public void LogMessage(LogLevel level, string message, Exception ex = null, params object[] args)
        {
            switch (level)
            {
                case LogLevel.All:
                case LogLevel.Info:
                    LogInfoMessage(message);
                    break;

                case LogLevel.Debug:
                    LogDebugMessage(message, ex, args);
                    break;

                case LogLevel.Error:
                    LogErrorMessage(message, ex, args);
                    break;

                case LogLevel.Trace:
                    LogTraceMessage(message, ex, args);
                    break;

                case LogLevel.Warn:
                    LogWarningMessage(message);
                    break;

                case LogLevel.Fatal:
                    LogFatalErrorMessage(message, ex, args);
                    break;

                case LogLevel.Off:
                    break;

                default:
                    var exception = new ArgumentOutOfRangeException(nameof(level), level, null);
                    LogErrorMessage(exception.Message, exception);
                    break;
            }
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="o">The object raising an exception</param>
        /// <param name="level">The level.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="method">The method.</param>
        public void LogMessage(object o, LogLevel level, Exception exception, string method)
        {
            LogMessage(level, exception.Message, exception, nameof(o), method);
        }

        /// <summary>
        /// Logs the information message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogInfoMessage(string message)
        {
            _log.Info(m => m(message));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarningMessage(string message)
        {
            _log.Warn(m => m(message));
        }

        /// <summary>
        /// Logs the trace message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public void LogTraceMessage(string message, Exception ex = null, params object[] args)
        {
            _log.Trace(m => m(message, args), ex);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public void LogDebugMessage(string message, Exception ex = null, params object[] args)
        {
            _log.Debug(m => m(message, args), ex);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public void LogErrorMessage(string message, Exception ex = null, params object[] args)
        {
            _log.Error(m => m(message, args), ex);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="args">The arguments.</param>
        public void LogFatalErrorMessage(string message, Exception ex = null, params object[] args)
        {
            if (ex == null)
                _log.Fatal(m => m(message, args));
            else
                _log.Fatal(m => m(message, args), ex);
        }

        /// <summary>
        /// Gets the caller location.
        /// </summary>
        /// <param name="methodCallCount">The method call count.</param>
        /// <returns></returns>
        private static CodeLocation GetCallerLocation(int methodCallCount)
        {
            string className;
            string methodName;
            string fileName;
            int lineNumber;

            var stackTrace = new StackTrace(methodCallCount, true);
            var stackFrame = stackTrace.GetFrame(0);

            if (stackFrame != null)
            {
                var method = stackFrame.GetMethod();
                if (method != null)
                {
                    className = method.ReflectedType.FullName;
                    methodName = method.Name;
                }
                else
                {
                    className = string.Empty;
                    methodName = string.Empty;
                }

                fileName = stackFrame.GetFileName();
                lineNumber = stackFrame.GetFileLineNumber();
            }
            else
            {
                className = string.Empty;
                methodName = string.Empty;
                fileName = string.Empty;
                lineNumber = -1;
            }

            var callerLocation = new CodeLocation
            {
                ClassName = className,
                MethodName = methodName,
                FileName = fileName,
                LineNumber = lineNumber
            };
            return callerLocation;
        }
    }
}