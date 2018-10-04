using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting.Compact;

namespace BellRichM.Logging
{
    /// <summary>
    /// Manage the logging
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        public LogManager()
        {
            LoggingLevelSwitches = new LoggingLevelSwitches();
            LoggingFilterSwitches = new LoggingFilterSwitches();
        }

        /// <summary>
        /// Gets or sets the logging level switches.
        /// </summary>
        /// <value>
        /// The logging level switches.
        /// </value>
        public LoggingLevelSwitches LoggingLevelSwitches { get; set; }

        /// <summary>
        /// Gets or sets the logging filter switches.
        /// </summary>
        /// <value>
        /// The logging filter switches.
        /// </value>
        public LoggingFilterSwitches LoggingFilterSwitches { get; set; }

        /// <summary>
        /// Creates the <see cref="LoggerConfiguration" /> and sets the globally shared <see cref="Serilog.Log.Logger" />
        /// </summary>
        /// <param name="logDir">Relative path to the log directory.</param>
        public void Create(string logDir)
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // ToDo: if (currentEnv == "Development")
            {
                LoggingLevelSwitches.DefaultLoggingLevelSwitch.MinimumLevel = LogEventLevel.Debug;

                // ToDo: loggingLevelSwitches.MicrosoftLoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
                // ToDo: loggingLevelSwitches.SystemLoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
                LoggingLevelSwitches.ConsoleSinkLevelSwitch.MinimumLevel = LogEventLevel.Verbose;
                LoggingFilterSwitches.ConsoleSinkFilterSwitch.Expression = "true";
            }

            var logFile = Path.Combine(logDir, "logTrace-{Date}.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LoggingLevelSwitches.DefaultLoggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", LoggingLevelSwitches.MicrosoftLoggingLevelSwitch)
                .MinimumLevel.Override("System", LoggingLevelSwitches.SystemLoggingLevelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.RollingFile(
                        new CompactJsonFormatter(),
                        Path.Combine(logDir, "events-{Date}.log.json"),
                        fileSizeLimitBytes: 10485760,
                        retainedFileCountLimit: 7))
                .WriteTo.Logger(l => l
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.RollingFile(
                        new CompactJsonFormatter(),
                        Path.Combine(logDir, "diagnostics-{Date}.log.json"),
                        fileSizeLimitBytes: 10485760,
                        retainedFileCountLimit: 7))
                .WriteTo.Logger(l => l
                    .Filter.ControlledBy(LoggingFilterSwitches.ConsoleSinkFilterSwitch)
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.File(
                        Path.Combine(logDir, "debug.log"),
                        fileSizeLimitBytes: 10485760,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Type} {RequestId}] {Message}{NewLine}"))
                .CreateLogger();
            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                Log.Information("*** Starting env {env} loggingLevelSwitches: {@loggingLevelSwitches} loggingFilterSwitches: {@loggingFilterSwitches}", currentEnv, LoggingLevelSwitches, LoggingFilterSwitches);
            }
        }
    }
}
