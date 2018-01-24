using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

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
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.RollingFile("logs/logTrace-{Date}.txt", fileSizeLimitBytes: 10485760, retainedFileCountLimit: 7) // 10 MB file size
                .CreateLogger();

            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Build the web host.
        /// </summary>
        /// <param name="args">The startup paramenters.</param>
        /// <returns>The <see cref="IWebHost"/></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
