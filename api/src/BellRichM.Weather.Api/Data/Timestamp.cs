using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The weather timestamp.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Timestamp
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