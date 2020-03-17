using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The weather observation.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ObservationDateTime
    {
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public int DateTime { get; set; }
    }
}