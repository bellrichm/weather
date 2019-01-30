using BellRichM.Attribute.CodeCoverage;
using BellRichM.Attribute.Validation;
using System.ComponentModel.DataAnnotations;

namespace BellRichM.Logging.Switches
{
    /// <summary>
    /// The serilog filter switch.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class FilterSwitch
    {
        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The filter switch expression.
        /// </value>
        [ValidateFilterExpression]
        public string Expression { get; set; }
    }
}