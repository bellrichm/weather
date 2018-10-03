using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;

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
                .Enrich.FromLogContext() // ToDo get this to work
                .WriteTo.Console()
                .Filter.ByExcluding(Matching.WithProperty<string>("Type", p => p.Equals("EVENT")))
                .WriteTo.RollingFile(logFile, fileSizeLimitBytes: 10485760, retainedFileCountLimit: 7) // 10 MB file size
                .CreateLogger();
        }
    }
}