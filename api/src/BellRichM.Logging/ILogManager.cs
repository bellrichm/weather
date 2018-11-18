using Serilog.Core;

namespace BellRichM.Logging
{
    /// <summary>
    /// Manage the logging.
    /// </summary>
    public interface ILogManager
    {
        /// <summary>
        /// Gets or sets the logging level switches.
        /// </summary>
        /// <value>
        /// The logging level switches.
        /// </value>
        LoggingLevelSwitches LoggingLevelSwitches { get; set; }

        /// <summary>
        /// Creates the <see cref="Logger" />.
        /// </summary>
        /// <returns>The configured <see cref="Logger" />.</returns>
        Logger Create();
    }
}