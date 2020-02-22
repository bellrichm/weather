using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace BellRichM.Logging
{
    /// <inheritdoc/>
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerAdapter{T}"/> class.
        /// </summary>
        public LoggerAdapter()
        {
            _logger = Log.ForContext<T>();
        }

        /// <inheritdoc/>
        public void LogDiagnosticTrace(string logmsg, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "TRACE"))
            {
                _logger.Verbose(logmsg, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticDebug(string logmsg, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "DEBUG"))
            {
                _logger.Debug(logmsg, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticInformation(string logmsg, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                _logger.Information(logmsg, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticWarning(string logmsg, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "WARNING"))
            {
                _logger.Warning(logmsg, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticCritical(string logmsg, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "CRITICAL"))
            {
                _logger.Fatal(logmsg, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticError(string logmsg, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "ERROR"))
            {
                _logger.Error(logmsg, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogEvent(EventId id, string logmsg, params object[] arguments)
        {
            LogDiagnosticInformation(logmsg, arguments);

            using (LogContext.Push(new PropertyEnricher("Type", "EVENT"), new PropertyEnricher("Id", (int)id), new PropertyEnricher("Event", id)))
            {
                _logger.Information(logmsg, arguments);
            }
        }
    }
}