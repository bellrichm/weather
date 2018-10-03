using BellRichM.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
            var logManager = new LogManager();
            logManager.Create("logs");

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
