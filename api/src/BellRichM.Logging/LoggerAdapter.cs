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
        public void LogDiagnosticTrace(string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "TRACE"))
            {
                _logger.Verbose(message, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticDebug(string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "DEBUG"))
            {
                _logger.Debug(message, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticInformation(string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                _logger.Information(message, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticWarning(string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "WARNING"))
            {
                _logger.Warning(message, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticCritical(string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "CRITICAL"))
            {
                _logger.Fatal(message, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogDiagnosticError(string message, params object[] arguments)
        {
            using (LogContext.PushProperty("Type", "ERROR"))
            {
                _logger.Error(message, arguments);
            }
        }

        /// <inheritdoc/>
        public void LogEvent(EventId id, string message, params object[] arguments)
        {
            LogDiagnosticInformation(message, arguments);

            using (LogContext.Push(new PropertyEnricher("Type", "EVENT"), new PropertyEnricher("Id", (int)id), new PropertyEnricher("Event", id)))
            {
                _logger.Information(message, arguments);
            }
        }
    }
}