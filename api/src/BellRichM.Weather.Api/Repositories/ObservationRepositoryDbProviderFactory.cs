using System.Data.Common;

namespace BellRichM.Weather.Api.Repositories
{
    /// <summary>
    /// Provides the DbProviderFactory to the ObservationRepository.
    /// </summary>
    public class ObservationRepositoryDbProviderFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationRepositoryDbProviderFactory"/> class.
        /// </summary>
        /// <param name="dbProviderFactory">The <see cref="DbProviderFactory"/>.</param>
        public ObservationRepositoryDbProviderFactory(DbProviderFactory dbProviderFactory)
        {
            ObservationDbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Gets the <see cref="DbProviderFactory"/>.
        /// </summary>
        /// <value>The db provider factory.</value>
        public DbProviderFactory ObservationDbProviderFactory { get; }
    }
}