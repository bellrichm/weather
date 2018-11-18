using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Administration.Api.Models
{
    /// <summary>
    /// The logging level switch.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggingLevelSwitchModel
    {
        /// <summary>
        /// Gets or sets minimim logging level.
        /// </summary>
        /// <value>
        /// The minimum logging level.
        /// </value>
        public string MinimumLevel { get; set; }
    }
}