using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Repositories;
using BellRichM.Weather.Api.Services;
using System;
using System.Data.Common;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;

namespace BellRichM.Weather.Api.Extensions
{
    /// <summary>
    /// Startup extenstion methods.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds the services needed for Weather API.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public static void AddWeatherServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var conditionRepositoryConfiguration = new ConditionRepositoryConfiguration();
            var conditionRepositoryConfigurationSection = configuration.GetSection("WeatherApi:ConditionRepository");
            new ConfigureFromConfigurationOptions<ConditionRepositoryConfiguration>(conditionRepositoryConfigurationSection)
                .Configure(conditionRepositoryConfiguration);
            services.AddSingleton<IConditionRepositoryConfiguration>(conditionRepositoryConfiguration);

            var observationRepositoryConfiguration = new ObservationRepositoryConfiguration();
            var observationRepositoryConfigurationSection = configuration.GetSection("WeatherApi:ObservationRepository");
            new ConfigureFromConfigurationOptions<ObservationRepositoryConfiguration>(observationRepositoryConfigurationSection)
                .Configure(observationRepositoryConfiguration);
            services.AddSingleton<IObservationRepositoryConfiguration>(observationRepositoryConfiguration);

            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                Log.Information("*** Starting: conditionRepositoryConfiguration.Provider {conditionRepositoryConfiguration.Provider}", conditionRepositoryConfiguration.Provider);
                Log.Information("*** Starting: conditionRepositoryConfiguration.MaximumConditions {conditionRepositoryConfiguration.MaximumConditions}", conditionRepositoryConfiguration.MaximumConditions);
                Log.Information("*** Starting: observationRepositoryConfiguration.Provider {observationepositoryConfiguration.Provider}", observationRepositoryConfiguration.Provider);
                Log.Information("*** Starting: observationRepositoryConfiguration.MaximumConditions {observationRepositoryConfiguration.MaximumConditions}", observationRepositoryConfiguration.MaximumObservations);
            }

            // Get the DbProviderFactory early so that any errors are found at start up
            var conditionDbProviderFactory = DbProviderFactories.GetFactory(conditionRepositoryConfiguration.Provider);
            services.AddSingleton(new ConditionRepositoryDbProviderFactory(conditionDbProviderFactory));

            var observationDbProviderFactory = DbProviderFactories.GetFactory(observationRepositoryConfiguration.Provider);
            services.AddSingleton(new ObservationRepositoryDbProviderFactory(observationDbProviderFactory));

            services.AddScoped<IConditionService, ConditionService>();
            services.AddScoped<IConditionRepository, ConditionRepository>();

            if (observationRepositoryConfiguration.Provider == "Sqlite")
            {
                services.AddScoped<IObservationService, ObservationSqliteService>();
            }
            else
            {
                throw new NotSupportedException(observationRepositoryConfiguration.Provider + " database is not supported for the observation repository.");
            }

            services.AddScoped<IObservationRepository, ObservationRepository>();
        }
    }
}