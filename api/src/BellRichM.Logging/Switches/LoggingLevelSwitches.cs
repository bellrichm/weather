using BellRichM.Attribute.CodeCoverage;
using Serilog.Core;

namespace BellRichM.Logging
{
    /// <summary>
    /// The logging level switches.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggingLevelSwitches
    {
        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the default logging level.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitch DefaultLoggingLevelSwitch { get; set; }

        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the logging level of microsoft messages.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitch MicrosoftLoggingLevelSwitch { get; set; }

        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the logging level of system messages.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitch SystemLoggingLevelSwitch { get; set; }

        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the logging level of the console sink.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitch ConsoleSinkLevelSwitch { get; set; }
    }
}
