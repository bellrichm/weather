using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Logging.Switches
{
    /// <summary>
    /// The serilog filter switches.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class FilterSwitchDefinitions
    {
        /// <summary>
        /// Gets or sets the console sink.
        /// </summary>
        /// <value>
        /// The filter switch for the console sink.
        /// </value>
        public FilterSwitch ConsoleSink { get; set; }
    }
}