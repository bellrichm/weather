namespace BellRichM.Weather.Api.Configuration
{
    /// <summary>
    /// The observation repository configuration.
    /// </summary>
        public interface IObservationRepositoryConfiguration
    {
        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        /// <value>darabase name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the database provider.
        /// </summary>
        /// <value>darabase provider.</value>
        string Provider { get; set; }

        /// <summary>
        /// Gets or sets the connecrion string.
        /// </summary>
        /// <value>connection string.</value>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the maximim observations to be returned.
        /// </summary>
        /// <value>The maximum observations to be returned for a call.</value>
        int MaximumObservations { get; set; }
    }
}