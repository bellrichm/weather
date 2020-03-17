using BellRichM.Attribute.CodeCoverage;
using System.ComponentModel.DataAnnotations;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The weather observation.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ObservationDateTimeModel
    {
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        [Range(1, int.MaxValue)]
        public int DateTime { get; set; }
    }
}