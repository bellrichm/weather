namespace InitObservations
{
    /// <summary>
    /// The WeeWX repository configuration.
    /// </summary>
        public interface IWeeWXRepositoryConfiguration
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
    }
}