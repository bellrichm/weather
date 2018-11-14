using BellRichM.Attribute.CodeCoverage;
using Serilog.Events;

namespace BellRichM.Logging.Switches
{
    /// <summary>
    /// The serilog level switch.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class LevelSwitch
    {
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The <see cref="LogEventLevel"/> class.
        /// </value>
        public LogEventLevel Level { get; set; }
    }
}