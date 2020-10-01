using BellRichM.Logging;
using System;
using System.Data.Common;

namespace InitObservations
{
    /// <inheritdoc/>
    public class WeeWXRepository : IWeeWXRepository
    {
        private readonly ILoggerAdapter<WeeWXRepository> _logger;
        private readonly string _connectionString;
        private readonly DbProviderFactory _weeWXDbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeWXRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="weeWXDbProviderFactory">The <see cref="WeeWXRepositoryDbProviderFactory"/>.</param>
        /// <param name="weeWXRepositoryConfiguration">The config.</param>
        public WeeWXRepository(ILoggerAdapter<WeeWXRepository> logger, WeeWXRepositoryDbProviderFactory weeWXDbProviderFactory, IWeeWXRepositoryConfiguration weeWXRepositoryConfiguration)
        {
            if (weeWXDbProviderFactory == null)
            {
                throw new ArgumentNullException(nameof(weeWXDbProviderFactory));
            }

            if (weeWXRepositoryConfiguration == null)
            {
                throw new ArgumentNullException(nameof(weeWXRepositoryConfiguration));
            }

            _logger = logger;
            _weeWXDbProviderFactory = weeWXDbProviderFactory.WeeWXDbProviderFactory;
            _connectionString = weeWXRepositoryConfiguration.ConnectionString;
        }     

        /// <inheritdoc/>
        public void Get()   
        {
            System.Console.WriteLine("get");
        }
    }
}