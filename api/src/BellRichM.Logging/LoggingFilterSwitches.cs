using Serilog.Filters.Expressions;

namespace BellRichM.Logging
{
    /// <summary>
    /// The logging filter switches.
    /// </summary>
    public class LoggingFilterSwitches
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingFilterSwitches"/> class.
        /// </summary>
        public LoggingFilterSwitches()
        {
            ConsoleSinkFilterSwitch = new LoggingFilterSwitch("false");
        }

        /// <summary>
        /// Gets or sets the <see cref="LoggingFilterSwitch"/>that dynamically controls the filtering of the console sink.>
        /// </summary>
        /// <value>
        /// The expression used to filter the event.
        /// </value>
        public LoggingFilterSwitch ConsoleSinkFilterSwitch { get; set; }
    }
}
