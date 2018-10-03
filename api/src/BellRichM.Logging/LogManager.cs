using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
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
            var logFile = Path.Combine(logDir, "logTrace-{Date}.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Filter.ByExcluding(Matching.FromSource‌​("Microsoft"))
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
                    .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                    .WriteTo.File(
                        Path.Combine(logDir, "debug.log"),
                        fileSizeLimitBytes: 10485760,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Type} {RequestId}] {Message}{NewLine}"))
                .CreateLogger();
        }
    }
}
