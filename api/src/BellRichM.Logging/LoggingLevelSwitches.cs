using Serilog.Core;
using Serilog.Events;

namespace BellRichM.Logging
{
    /// <summary>
    /// The logging level switches.
    /// </summary>
    public class LoggingLevelSwitches
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingLevelSwitches"/> class.
        /// </summary>
        public LoggingLevelSwitches()
      {
        DefaultLoggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Information);
        MicrosoftLoggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Warning);
        SystemLoggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Warning);
        ConsoleSinkLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Fatal);
      }

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
