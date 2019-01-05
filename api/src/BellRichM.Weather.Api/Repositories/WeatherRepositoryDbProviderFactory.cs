using System.Data.Common;

namespace BellRichM.Weather.Api.Repositories
{
    /// <summary>
    /// Provides the DbProviderFactory to the WeatherRepository.
    /// </summary>
    public class WeatherRepositoryDbProviderFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherRepositoryDbProviderFactory"/> class.
        /// </summary>
        /// <param name="dbProviderFactory">The <see cref="DbProviderFactory"/>.</param>
        public WeatherRepositoryDbProviderFactory(DbProviderFactory dbProviderFactory)
        {
            WeatherDbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Gets the <see cref="DbProviderFactory"/>.
        /// </summary>
        /// <value>The db provider factory.</value>
        public DbProviderFactory WeatherDbProviderFactory { get; }
    }
}