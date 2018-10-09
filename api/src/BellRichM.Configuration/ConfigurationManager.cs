using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BellRichM.Configuration
{
    /// <summary>
    /// Manage the configuration
    /// </summary>
    public class ConfigurationManager
    {
        private readonly string _environment;
        private readonly string _basePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManager"/> class.
        /// </summary>
        /// <param name="environment">The runtime environment.</param>
        /// <param name="basePath">The base path.</param>
        public ConfigurationManager(string environment, string basePath)
        {
            _environment = environment;
            _basePath = basePath;
        }

        /// <summary>
        /// Creates the <see cref="IConfiguration" />
        /// </summary>
        /// <returns>A <see cref="IConfiguration"/></returns>
        public IConfiguration Create()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(_basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{_environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}
