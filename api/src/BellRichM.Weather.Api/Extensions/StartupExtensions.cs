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
            var conditionRepositoryConfiguration = new ConditionRepositoryConfiguration();
            var conditionRepositoryConfigurationSection = configuration.GetSection("WeatherApi:Repository");
            new ConfigureFromConfigurationOptions<ConditionRepositoryConfiguration>(conditionRepositoryConfigurationSection)
                .Configure(conditionRepositoryConfiguration);

            services.AddSingleton<IConditionRepositoryConfiguration>(conditionRepositoryConfiguration);

            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                Log.Information("*** Starting: conditionRepositoryConfiguration.Provider {conditionRepositoryConfiguration.Provider}", conditionRepositoryConfiguration.Provider);
                Log.Information("*** Starting: conditionRepositoryConfiguration.MaximumConditions {conditionRepositoryConfiguration.MaximumConditions}", conditionRepositoryConfiguration.MaximumConditions);
            }

            // Get the DbProviderFactory early so that any errors are found at start up
            var dbProviderFactory = DbProviderFactories.GetFactory(conditionRepositoryConfiguration.Provider);
            var conditionDbProviderFactory = new ConditionRepositoryDbProviderFactory(dbProviderFactory);
            services.AddSingleton(conditionDbProviderFactory);

            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IConditionRepository, ConditionRepository>();
        }
    }
}