namespace Nucleotic.Framework.Logging
{
    public enum LogLevel
    {
        /// <summary>
        /// Switches off logging
        /// </summary>
        Off = 0,

        /// <summary>
        /// The least restrictive level.
        /// </summary>
        All = 1,

        /// <summary>
        /// The trace
        /// </summary>
        Trace = 2,

        /// <summary>
        /// For debugging purposes.
        /// </summary>
        Debug = 3,

        /// <summary>
        /// Signifies verbose information.
        /// </summary>
        Info = 4,

        /// <summary>
        /// Signifies a warning from e.g. an unexpected event.
        /// </summary>
        Warn = 5,

        /// <summary>
        /// When an application error occurs.
        /// </summary>
        Error = 6,

        /// <summary>
        /// When the application is no longer able to function or is in an unreliable state. Highly restrive logging.
        /// </summary>
        Fatal = 7
    }
}