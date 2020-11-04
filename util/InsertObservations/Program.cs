using AutoMapper;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Mapping;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;

#pragma warning disable CA1303
namespace Insertbservations
{
    /// <summary>
    /// The mainline program to insert into the observation database.
    /// </summary>
    #pragma warning disable CA1812
    class Program
    #pragma warning restore CA1812
    {
        static ILoggerAdapter<Program> logger;
        static IObservationRepository observationRepository;

        /// <summary>
        /// Add observations to a local db.
        /// </summary>
        /// <param name="jsonFile">The file of observations.</param>
        static async Task Main(string jsonFile = "temp.json")
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ObservationProfile>();
            });
            var mapper = configuration.CreateMapper();

            var serviceProvider = Configure();
            logger = serviceProvider.GetService<ILoggerAdapter<Program>>();
            observationRepository = serviceProvider.GetService<IObservationRepository>();

            string jsonText = File.ReadAllText(jsonFile);

            var observationModels = new List<ObservationModel>();
            using (StreamReader file = File.OpenText(jsonFile))
            {
                var serializer = new JsonSerializer();
                observationModels = (List<ObservationModel>)serializer.Deserialize(file, typeof(List<ObservationModel>));
            }

            var observations = mapper.Map<List<Observation>>(observationModels);

            // var count = await observationRepository.CreateObservations(observations).ConfigureAwait(true);
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

            return services.BuildServiceProvider();
        }
    }
    #pragma warning restore CA1303
}
