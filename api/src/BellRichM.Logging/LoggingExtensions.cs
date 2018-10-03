using Serilog;
using System.Linq;
using Serilog.Context;
using Serilog.Core.Enrichers;

/*
Microsoft Log Level     Serilog Log Level
===================     =================
Trace                   Verbose
Debug                   Debug
Information             Information
Warning                 Warning
Error                   Error
Critical                Fatal
 */
namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Logging extension methods
    /// </summary>
    public static class LoggingExtensions
    {
        // TODO: - a better way to concatenate the arrays?

        /// <summary>
        /// Logs the trace  message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
        public static void LogDiagnosticTrace(this ILogger logger, string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "TRACE"))
            {
                logger.LogTrace(message, arguments);
            }
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
        public static void LogDiagnosticDebug(this ILogger logger, string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "DEBUG"))
            {
                logger.LogDebug(message, arguments);
            }
        }

        /// <summary>
        /// Logs the information message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
        public static void LogDiagnosticInformation(this ILogger logger, string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                logger.LogInformation(message, arguments);
            }
        }

        /// <summary>
        /// Logs the warninng message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
        public static void LogDiagnosticWarning(this ILogger logger, string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "WARNING"))
            {
                logger.LogWarning(message, arguments);
            }
        }

        /// <summary>
        /// Logs the critical message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
        public static void LogDiagnosticCritical(this ILogger logger, string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "CRITICAL"))
            {
                logger.LogCritical(message, arguments);
            }
        }

        /// <summary>
        /// Logs the diagnostic message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
        public static void LogDiagnosticError(this ILogger logger, string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "ERROR"))
            {
                logger.LogError(message, arguments);
            }
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="id">The event id</param>
        /// <param name="message">The message.</param>
        /// <param name="arguments">Additional arguments to be logged.</param>
        public static void LogEvent(this ILogger logger, int id, string message, params object[] arguments)
        {
            logger.LogDiagnosticInformation(message, arguments);

            using (LogContext.Push(new PropertyEnricher("Type", "EVENT"), new PropertyEnricher("Id", id)))
            {
                logger.LogInformation(message, arguments);
            }
        }
    }
}