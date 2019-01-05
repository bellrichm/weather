namespace BellRichM.Weather.Api.Configuration
{
    /// <summary>
    /// The weathwe db configuration.
    /// </summary>
    public interface IWeatherRepositoryConfiguration
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
        /// Gets or sets the maximim conditions to be returned.
        /// </summary>
        /// <value>The maximum conditions to be returned for a call.</value>
        int MaximumConditions { get; set; }
    }
}