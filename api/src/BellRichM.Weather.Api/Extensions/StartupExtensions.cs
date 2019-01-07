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
            var weatherRepositoryConfiguration = new WeatherRepositoryConfiguration();
            var weatherRepositoryConfigurationSection = configuration.GetSection("WeatherApi:Repository");
            new ConfigureFromConfigurationOptions<WeatherRepositoryConfiguration>(weatherRepositoryConfigurationSection)
                .Configure(weatherRepositoryConfiguration);

            services.AddSingleton<IWeatherRepositoryConfiguration>(weatherRepositoryConfiguration);

            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                Log.Information("*** Starting: weatherRepositoryConfiguration.Provider {weatherRepositoryConfiguration.Provider}", weatherRepositoryConfiguration.Provider);
                Log.Information("*** Starting: weatherRepositoryConfiguration.MaximumConditions {weatherRepositoryConfiguration.MaximumConditions}", weatherRepositoryConfiguration.MaximumConditions);
            }

            // Get the DbProviderFactory early so that any errors are found at start up
            var dbProviderFactory = DbProviderFactories.GetFactory(weatherRepositoryConfiguration.Provider);
            var weatherDbProviderFactory = new WeatherRepositoryDbProviderFactory(dbProviderFactory);
            services.AddSingleton(weatherDbProviderFactory);

            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IWeatherRepository, WeatherRepository>();
        }
    }
}