using BellRichM.Configuration;
using BellRichM.Logging;
using BellRichM.Weather.Api.Extensions;
using BellRichM.Weather.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace InitObservations
{
    class Program
    {
        static ILoggerAdapter<Program> logger;
        static IObservationRepository observationRepository;

        /// <summary>
        /// Initialize the identity db.static.static.
        /// </summary>
        /// <param name="weewxDB">A json file containing the roles to add.</param>
        /// <param name="weatherDB">A json file containing the users to add.</param>
        static void Main(string weewxDB = "roles.json", string weatherDB = "users.json")
        {
            System.Console.WriteLine("start");
            System.Console.WriteLine(weewxDB);
            System.Console.WriteLine(weatherDB);

            var serviceProvider = Configure();
            logger = serviceProvider.GetService<ILoggerAdapter<Program>>();
            observationRepository = serviceProvider.GetService<IObservationRepository>();

            var observation = observationRepository.GetObservation(1);

            var weeWXRepository = serviceProvider.GetService<IWeeWXRepository>();
            weeWXRepository.Get();
            
            System.Console.WriteLine("end");
        }

        private static IServiceProvider Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory + "../../..")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var logManager = new LogManager(configuration);
            Log.Logger = logManager.Create();

            DbProviderFactories.RegisterFactory("Sqlite", SqliteFactory.Instance);

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddWeatherServices(configuration);

            var weeWXRepositoryConfiguration = new WeeWXRepositoryConfiguration();
            var weeWXRepositoryConfigurationSection = configuration.GetSection("WeeWXRepository");
            new ConfigureFromConfigurationOptions<WeeWXRepositoryConfiguration>(weeWXRepositoryConfigurationSection)
                .Configure(weeWXRepositoryConfiguration);
            services.AddSingleton<IWeeWXRepositoryConfiguration>(weeWXRepositoryConfiguration);

            var weeWXDbProviderFactory = DbProviderFactories.GetFactory(weeWXRepositoryConfiguration.Provider);
            services.AddSingleton(new WeeWXRepositoryDbProviderFactory(weeWXDbProviderFactory));

            services.AddScoped<IWeeWXRepository, WeeWXRepository>();

            return services.BuildServiceProvider();
        }
    }
}
