using System;
using BellRichM.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Context;

namespace BellRichM.Weather.Web
{
    /// <summary>
    /// The startup program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The startup parameters.</param>
        /// <returns>Returns 0 on success and 1 on failure</returns>
        public static int Main(string[] args)
        {
            var logManager = new LogManager();
            logManager.Create("logs");
            try
            {
                BuildWebHost(args, logManager).Run();
                return 0;
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("Type", "CRITICAL"))
                {
                    Log.Fatal("Host terminated unexpectedly\n {@exception}", ex);
                }

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Build the web host.
        /// </summary>
        /// <param name="args">The startup paramenters.</param>
        /// <param name="logManager">The <see cref="LogManager"/>.</param>
        /// <returns>
        /// The <see cref="IWebHost" />
        /// </returns>
        public static IWebHost BuildWebHost(string[] args, LogManager logManager) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseSerilog()
            .ConfigureServices(s => s.AddSingleton<LoggingLevelSwitches>(logManager.LoggingLevelSwitches))
            .ConfigureServices(s => s.AddSingleton<LoggingFilterSwitches>(logManager.LoggingFilterSwitches))
            .Build();
    }
}