using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
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
        /// Creates the <see cref="LoggerConfiguration" /> and sets the globally shared <see cref="Serilog.Log.Logger" />
        /// </summary>
        /// <param name="logDir">Relative path to the log directory.</param>
        public void Create(string logDir)
        {
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var loggingLevelSwitches = new LoggingLevelSwitches();
            var loggingFilterSwitches = new LoggingFilterSwitches();

            // ToDo: if (currentEnv == "Development")
            {
                loggingLevelSwitches.DefaultLoggingLevelSwitch.MinimumLevel = LogEventLevel.Debug;

                // ToDo: loggingLevelSwitches.MicrosoftLoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
                // ToDo: loggingLevelSwitches.SystemLoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
                loggingLevelSwitches.ConsoleSinkLevelSwitch.MinimumLevel = LogEventLevel.Verbose;
                loggingFilterSwitches.ConsoleSinkFilterSwitch.Expression = "true";
            }

            var logFile = Path.Combine(logDir, "logTrace-{Date}.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(loggingLevelSwitches.DefaultLoggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", loggingLevelSwitches.MicrosoftLoggingLevelSwitch)
                .MinimumLevel.Override("System", loggingLevelSwitches.SystemLoggingLevelSwitch)
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
                    .Filter.ControlledBy(loggingFilterSwitches.ConsoleSinkFilterSwitch)
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.File(
                        Path.Combine(logDir, "debug.log"),
                        fileSizeLimitBytes: 10485760,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Type} {RequestId}] {Message}{NewLine}"))
                .CreateLogger();
            Log.Information("*** Starting env {env} loggingLevelSwitches: {@loggingLevelSwitches} loggingFilterSwitches: {@loggingFilterSwitches}", currentEnv, loggingLevelSwitches, loggingFilterSwitches);
        }
    }
}
