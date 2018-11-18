using BellRichM.Attribute.CodeCoverage;
using Serilog.Filters.Expressions;

namespace BellRichM.Administration.Api.Models
{
    /// <summary>
    /// The logging filter switches.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggingFilterSwitchesModel
    {
        /// <summary>
        /// Gets or sets the <see cref="LoggingFilterSwitch"/>that dynamically controls the filtering of the console sink.
        /// </summary>
        /// <value>
        /// The expression used to filter the event.
        /// </value>
        public LoggingFilterSwitchModel ConsoleSinkFilterSwitch { get; set; }
    }
}
