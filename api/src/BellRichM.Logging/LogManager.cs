using System;
using System.IO;
using Destructurama;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Filters.Expressions;
using Serilog.Formatting.Compact;

namespace BellRichM.Logging
{
    /// <summary>
    /// Manage the logging.
    /// </summary>
    public class LogManager
    {
        private readonly LoggingConfiguration _loggingConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        /// <param name="configuration">The runtime configuration.</param>
        public LogManager(IConfiguration configuration)
        {
            var loggingSection = configuration.GetSection("Logging");
            _loggingConfiguration = new LoggingConfiguration();
            new ConfigureFromConfigurationOptions<LoggingConfiguration>(loggingSection)
                .Configure(_loggingConfiguration);

            LoggingLevelSwitches = new LoggingLevelSwitches();
            LoggingLevelSwitches.DefaultLoggingLevelSwitch = new LoggingLevelSwitch(_loggingConfiguration.LevelSwitches.Default.Level);
            LoggingLevelSwitches.MicrosoftLoggingLevelSwitch = new LoggingLevelSwitch(_loggingConfiguration.LevelSwitches.Microsoft.Level);
            LoggingLevelSwitches.SystemLoggingLevelSwitch = new LoggingLevelSwitch(_loggingConfiguration.LevelSwitches.System.Level);
            LoggingLevelSwitches.ConsoleSinkLevelSwitch = new LoggingLevelSwitch(_loggingConfiguration.LevelSwitches.ConsoleSink.Level);

            LoggingFilterSwitches = new LoggingFilterSwitches();
            LoggingFilterSwitches.ConsoleSinkFilterSwitch = new LoggingFilterSwitch(_loggingConfiguration.FilterSwitches.ConsoleSink.Expression);
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
        /// Creates the <see cref="LoggerConfiguration" />.
        /// </summary>
        /// <returns>The configured <see cref="Logger" />.</returns>
        public Logger Create()
        {
            var logger = new LoggerConfiguration()
                .Destructure.UsingAttributes()
                .MinimumLevel.ControlledBy(LoggingLevelSwitches.DefaultLoggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", LoggingLevelSwitches.MicrosoftLoggingLevelSwitch)
                .MinimumLevel.Override("System", LoggingLevelSwitches.SystemLoggingLevelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(Matching.WithProperty<string>("Type", p => p.Equals("EVENT", StringComparison.Ordinal)))
                    .WriteTo.File(
                        new CompactJsonFormatter(),
                        Path.Combine(_loggingConfiguration.Sinks.EventLog.LogPath, _loggingConfiguration.Sinks.EventLog.LogName),
                        rollOnFileSizeLimit: true,
                        rollingInterval: _loggingConfiguration.Sinks.EventLog.RollingInterval,
                        fileSizeLimitBytes: _loggingConfiguration.Sinks.EventLog.LogSize,
                        retainedFileCountLimit: _loggingConfiguration.Sinks.EventLog.LogRetention))
                .WriteTo.Logger(l => l
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT", StringComparison.Ordinal)))
                    .WriteTo.File(
                        new CompactJsonFormatter(),
                        Path.Combine(_loggingConfiguration.Sinks.DiagnosticLog.LogPath, _loggingConfiguration.Sinks.DiagnosticLog.LogName),
                        rollOnFileSizeLimit: true,
                        rollingInterval: _loggingConfiguration.Sinks.DiagnosticLog.RollingInterval,
                        fileSizeLimitBytes: _loggingConfiguration.Sinks.DiagnosticLog.LogSize,
                        retainedFileCountLimit: _loggingConfiguration.Sinks.DiagnosticLog.LogRetention))
                .WriteTo.Logger(l => l
                    .Filter.ControlledBy(LoggingFilterSwitches.ConsoleSinkFilterSwitch)
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT", StringComparison.Ordinal)))
                    .WriteTo.File(
                        Path.Combine(_loggingConfiguration.Sinks.DebugLog.LogPath, _loggingConfiguration.Sinks.DebugLog.LogName),
                        rollOnFileSizeLimit: true,
                        rollingInterval: _loggingConfiguration.Sinks.DebugLog.RollingInterval,
                        fileSizeLimitBytes: _loggingConfiguration.Sinks.DebugLog.LogSize,
                        retainedFileCountLimit: _loggingConfiguration.Sinks.DebugLog.LogRetention,
                        outputTemplate: _loggingConfiguration.Sinks.DebugLog.OutputTemplate))
                .CreateLogger();
            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                logger.Information("*** Starting: {@loggingConfiguration}", _loggingConfiguration);
            }

            return logger;
        }
    }
}
