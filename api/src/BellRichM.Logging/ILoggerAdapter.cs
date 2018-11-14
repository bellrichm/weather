namespace BellRichM.Logging
{
    /// <summary>
    /// Adapter to enable unit testing logging
    ///
    /// Microsoft Log Level     Serilog Log Level
    /// ===================     =================
    /// Trace                   Verbose
    /// Debug                   Debug
    /// Information             Information
    /// Warning                 Warning
    /// Error                   Error
    /// Critical                Fatal
    /// </summary>
    /// <typeparam name="T">The logger type</typeparam>
    public interface ILoggerAdapter<T>
    {
        /// <summary>
        /// Logs the trace message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
         void LogDiagnosticTrace(string message, params object[] arguments);

         /// <summary>
        /// Logs the debig message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
         void LogDiagnosticDebug(string message, params object[] arguments);

         /// <summary>
         /// Logs the information message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
         void LogDiagnosticInformation(string message, params object[] arguments);

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
         void LogDiagnosticWarning(string message, params object[] arguments);

        /// <summary>
        /// Logs the critical message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
         void LogDiagnosticCritical(string message, params object[] arguments);

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
         void LogDiagnosticError(string message, params object[] arguments);

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="id">The event id</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
         void LogEvent(EventId id, string message, params object[] arguments);
    }
}