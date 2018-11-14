using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Logging.Switches
{
    /// <summary>
    /// The serilog filter switch
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
        public string Expression { get; set; }
    }
}