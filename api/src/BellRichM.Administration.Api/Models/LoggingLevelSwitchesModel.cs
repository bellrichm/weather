using BellRichM.Attribute.CodeCoverage;
using Serilog.Core;

namespace BellRichM.Administration.Api.Models
{
    /// <summary>
    /// The logging level switches.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggingLevelSwitchesModel
    {
        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the default logging level.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitchModel DefaultLoggingLevelSwitch { get; set; }

        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the logging level of microsoft messages.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitchModel MicrosoftLoggingLevelSwitch { get; set; }

        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the logging level of system messages.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitchModel SystemLoggingLevelSwitch { get; set; }

        /// <summary>
        /// Gets or sets <see cref="LoggingLevelSwitch"/>that dynamically controls the logging level of the console sink.
        /// </summary>
        /// <value>
        /// The logging level to filter on.
        /// </value>
        public LoggingLevelSwitchModel ConsoleSinkLevelSwitch { get; set; }
    }
}
