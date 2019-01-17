using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Weather.Api.Configuration
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class ObservationRepositoryConfiguration : IObservationRepositoryConfiguration
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