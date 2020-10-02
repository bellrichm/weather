using AutoMapper;
using BellRichM.Configuration;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Extensions;
using BellRichM.Weather.Api.Mapping;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
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
    /// <summary>
    /// The mainline program to initialize the observation database.
    /// </summary>
    #pragma warning disable CA1812
    class Program
    #pragma warning restore CA1812
    {
        static ILoggerAdapter<Program> logger;
        static IObservationRepository observationRepository;

        /// <summary>
        /// Initialize the identity db.static.static.
        /// </summary>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        static async Task Main(string start = "", string end = "")
        {
            DateTime startDate;
            if (string.IsNullOrEmpty(start))
            {
                startDate = DateTime.UtcNow;
            }
            else
            {
                startDate = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            DateTime endDate;
            if (string.IsNullOrEmpty(end))
            {
                endDate = startDate;
            }
            else
            {
                endDate = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var startTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
            var startTimestamp = ToUnixEpochDate(startTime);

            var endTime = new DateTime(endDate.Year, endDate.Month, endDate.Day + 1, 0, 0, 0);
            var endTimestamp = ToUnixEpochDate(endTime);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ObservationProfile>();
            });
            var mapper = configuration.CreateMapper();

            var serviceProvider = Configure();
            logger = serviceProvider.GetService<ILoggerAdapter<Program>>();
            observationRepository = serviceProvider.GetService<IObservationRepository>();

            var weeWXRepository = serviceProvider.GetService<IWeeWXRepository>();
            var weatherModel = await weeWXRepository.GetWeather(startTimestamp, endTimestamp).ConfigureAwait(true);
            var weather = mapper.Map<List<Observation>>(weatherModel);

            var i = 1;
            foreach (var record in weather)
            {
                Console.WriteLine($"Processing record {i} of {weather.Count}.");
                var observation = await observationRepository.GetObservation(record.DateTime).ConfigureAwait(true);

                if (observation == null)
                {
                    var count = await observationRepository.CreateObservation(record).ConfigureAwait(true);
                    logger.LogDiagnosticInformation("Created Record. {@dateTime}", record.DateTime);
                    Console.WriteLine($"\tCreated Record. {record.DateTime}");
                }
                else
                {
                    logger.LogDiagnosticInformation("Record exists, skipping. {@dateTime}", record.DateTime);
                }

                i += 1;
            }
        }

        private static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();

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
