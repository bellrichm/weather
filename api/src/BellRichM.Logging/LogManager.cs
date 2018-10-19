using System;
using System.IO;
using Destructurama;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private readonly LoggingConfiguration _loggingConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogManager"/> class.
        /// </summary>
        /// <param name="configuration">The runtime configuration.</param>
        public LogManager(IConfiguration configuration)
        {
            var a = configuration.GetSection("Logging");
            _loggingConfiguration = new LoggingConfiguration();
            new ConfigureFromConfigurationOptions<LoggingConfiguration>(a)
                .Configure(_loggingConfiguration);

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
        public void Create()
        {
            LoggingLevelSwitches.DefaultLoggingLevelSwitch.MinimumLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), _loggingConfiguration.LevelSwitches.Default.Level, true);
            LoggingLevelSwitches.MicrosoftLoggingLevelSwitch.MinimumLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), _loggingConfiguration.LevelSwitches.Microsoft.Level, true);
            LoggingLevelSwitches.SystemLoggingLevelSwitch.MinimumLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), _loggingConfiguration.LevelSwitches.System.Level, true);
            LoggingLevelSwitches.ConsoleSinkLevelSwitch.MinimumLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), _loggingConfiguration.LevelSwitches.ConsoleSink.Level, true);
            LoggingFilterSwitches.ConsoleSinkFilterSwitch.Expression = _loggingConfiguration.FilterSwitches.ConsoleSink.Expression;

            Log.Logger = new LoggerConfiguration()
                .Destructure.UsingAttributes()
                .MinimumLevel.ControlledBy(LoggingLevelSwitches.DefaultLoggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", LoggingLevelSwitches.MicrosoftLoggingLevelSwitch)
                .MinimumLevel.Override("System", LoggingLevelSwitches.SystemLoggingLevelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.Logger(l => l
                    .Filter.ByIncludingOnly(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.RollingFile(
                        new CompactJsonFormatter(),
                        Path.Combine(_loggingConfiguration.Sinks.EventLog.LogPath, _loggingConfiguration.Sinks.EventLog.LogName),
                        fileSizeLimitBytes: _loggingConfiguration.Sinks.EventLog.LogSize,
                        retainedFileCountLimit: _loggingConfiguration.Sinks.EventLog.LogRetention))
                .WriteTo.Logger(l => l
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.RollingFile(
                        new CompactJsonFormatter(),
                        Path.Combine(_loggingConfiguration.Sinks.DiagnosticLog.LogPath, _loggingConfiguration.Sinks.DiagnosticLog.LogName),
                        fileSizeLimitBytes: _loggingConfiguration.Sinks.DiagnosticLog.LogSize,
                        retainedFileCountLimit: _loggingConfiguration.Sinks.DiagnosticLog.LogRetention))
                .WriteTo.Logger(l => l
                    .Filter.ControlledBy(LoggingFilterSwitches.ConsoleSinkFilterSwitch)
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.File(
                        Path.Combine(_loggingConfiguration.Sinks.DebugLog.LogPath, _loggingConfiguration.Sinks.DebugLog.LogName),
                        fileSizeLimitBytes: _loggingConfiguration.Sinks.DebugLog.LogSize,
                        outputTemplate: _loggingConfiguration.Sinks.DebugLog.OutputTemplate))
                .CreateLogger();
            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                Log.Information("*** Starting loggingConfiguration {@loggingConfiguration}", _loggingConfiguration);
            }
        }
    }
}
