using BellRichM.Attribute.CodeCoverage;

namespace InitObservations
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class WeeWXRepositoryConfiguration : IWeeWXRepositoryConfiguration
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string Provider { get; set; }

        /// <inheritdoc/>
        public string ConnectionString { get; set; }

        /// <inheritdoc/>
        public int MaximumObservations { get; set; }
    }
}