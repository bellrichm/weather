using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Administration.Api.Models
{
    /// <summary>
    /// The serilog filter switch.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class LoggingFilterSwitchModel
    {
        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The filter switch expression.
        /// </value>
        public string Expression { get; set; }
    }
}