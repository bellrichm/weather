using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Logging.Switches
{
    /// <summary>
    /// The serilog level switches.
    /// </summary>
    [ExcludeFromCodeCoverage]

    public class LevelSwitchDefinitions
    {
        /// <summary>
        /// Gets or sets the default.
        /// </summary>
        /// <value>
        /// The default level switch.
        /// </value>
        public LevelSwitch Default { get; set; }

        /// <summary>
        /// Gets or sets the microsoft.
        /// </summary>
        /// <value>
        /// The microsoft <see cref="LevelSwitch"/> class.
        /// </value>
        public LevelSwitch Microsoft { get; set; }

        /// <summary>
        /// Gets or sets the system.
        /// </summary>
        /// <value>
        /// The system <see cref="LevelSwitch"/> class.
        /// </value>
        public LevelSwitch System { get; set; }

        /// <summary>
        /// Gets or sets the console sink.
        /// </summary>
        /// <value>
        /// The console sink <see cref="LevelSwitch"/> class.
        /// </value>
        public LevelSwitch ConsoleSink { get; set; }
    }
}