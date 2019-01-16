using System.Data.Common;

namespace BellRichM.Weather.Api.Repositories
{
    /// <summary>
    /// Provides the DbProviderFactory to the ConditionRepository.
    /// </summary>
    public class ConditionRepositoryDbProviderFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionRepositoryDbProviderFactory"/> class.
        /// </summary>
        /// <param name="dbProviderFactory">The <see cref="DbProviderFactory"/>.</param>
        public ConditionRepositoryDbProviderFactory(DbProviderFactory dbProviderFactory)
        {
            ConditionDbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// Gets the <see cref="DbProviderFactory"/>.
        /// </summary>
        /// <value>The db provider factory.</value>
        public DbProviderFactory ConditionDbProviderFactory { get; }
    }
}