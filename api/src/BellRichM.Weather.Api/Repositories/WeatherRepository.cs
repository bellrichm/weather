using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using System.Data.Common;

namespace BellRichM.Weather.Api.Repositories
{
    /// <inheritdoc/>
    public class WeatherRepository : IWeatherRepository
    {
        private readonly ILoggerAdapter<WeatherRepository> _logger;
        private readonly string _connectionString;
        private DbProviderFactory _weatherDbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="weatherDbProviderFactory">The <see cref="WeatherRepositoryDbProviderFactory"/>.</param>
        /// <param name="weatherRepositoryConfiguration">The config.</param>
        public WeatherRepository(ILoggerAdapter<WeatherRepository> logger, WeatherRepositoryDbProviderFactory weatherDbProviderFactory, IWeatherRepositoryConfiguration weatherRepositoryConfiguration)
        {
            _logger = logger;
            _weatherDbProviderFactory = weatherDbProviderFactory.WeatherDbProviderFactory;
            _connectionString = weatherRepositoryConfiguration.ConnectionString;
        }
    }
}