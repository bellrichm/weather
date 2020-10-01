using System.Data.Common;

namespace InitObservations
{
    /// <summary>
    /// Provides the DbProviderFactory to the WeeWXRepository.
    /// </summary>
    public class WeeWXRepositoryDbProviderFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeeWXRepositoryDbProviderFactory"/> class.
        /// </summary>
        /// <param name="dbProviderFactory">The <see cref="DbProviderFactory"/>.</param>
        public WeeWXRepositoryDbProviderFactory(DbProviderFactory dbProviderFactory)
        {
            WeeWXDbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Gets the <see cref="DbProviderFactory"/>.
        /// </summary>
        /// <value>The db provider factory.</value>
        public DbProviderFactory WeeWXDbProviderFactory { get; }
    }
}