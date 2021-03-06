using BellRichM.Attribute.CodeCoverage;
using Serilog.Filters.Expressions;

namespace BellRichM.Logging
{
    /// <summary>
    /// The logging filter switches.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggingFilterSwitches
    {
        /// <summary>
        /// Gets or sets the <see cref="LoggingFilterSwitch"/>that dynamically controls the filtering of the console sink.
        /// </summary>
        /// <value>
        /// The expression used to filter the event.
        /// </value>
        public LoggingFilterSwitch ConsoleSinkFilterSwitch { get; set; }
    }
}
