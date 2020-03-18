using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The time period.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TimePeriodModel
    {
        /// <summary>
        /// Gets or sets the start of the time period.
        /// </summary>
        public int StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end of the time period.
        /// </summary>
        public int EndDateTime { get; set; }
    }
}