using BellRichM.Attribute.CodeCoverage;
using BellRichM.Logging.Switches;

namespace BellRichM.Logging
{
    /// <summary>
    /// The logging configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggingConfiguration
    {
        /// <summary>
        /// Gets or sets the level switches.
        /// </summary>
        /// <value>
        /// The level switches to be configured.
        /// </value>
        public LevelSwitchDefinitions LevelSwitches { get; set; }

        /// <summary>
        /// Gets or sets the filter switches.
        /// </summary>
        /// <value>
        /// The filter switches to be configured.
        /// </value>
        public FilterSwitchDefinitions FilterSwitches { get; set; }

        /// <summary>
        /// Gets or sets the sinks.
        /// </summary>
        /// <value>
        /// The sinks to be configured.
        /// </value>
        public SinkDefinitions Sinks { get; set; }
     }
}
