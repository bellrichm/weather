using BellRichM.Attribute.CodeCoverage;
using System.ComponentModel.DataAnnotations;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The weather timestamp.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TimestampModel
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