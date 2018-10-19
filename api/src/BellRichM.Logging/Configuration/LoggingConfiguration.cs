using BellRichM.Attribute.CodeCoverage;

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

        /// <summary>
        /// The serilog level switches.
        /// </summary>
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
            /// The microsoft level switch.
            /// </value>
            public LevelSwitch Microsoft { get; set; }

            /// <summary>
            /// Gets or sets the system.
            /// </summary>
            /// <value>
            /// The system level switch.
            /// </value>
            public LevelSwitch System { get; set; }

            /// <summary>
            /// Gets or sets the console sink.
            /// </summary>
            /// <value>
            /// The console sink level switch.
            /// </value>
            public LevelSwitch ConsoleSink { get; set; }

            /// <summary>
            /// The serilog level switch
            /// </summary>
            public class LevelSwitch
            {
                /// <summary>
                /// Gets or sets the level.
                /// </summary>
                /// <value>
                /// The log level.
                /// </value>
                public string Level { get; set; }
            }
        }

        /// <summary>
        /// The serilog filter switches.
        /// </summary>
        public class FilterSwitchDefinitions
        {
            /// <summary>
            /// Gets or sets the console sink.
            /// </summary>
            /// <value>
            /// The filter switch for the console sink.
            /// </value>
            public FilterSwitch ConsoleSink { get; set; }

            /// <summary>
            /// The serilog filter switch
            /// </summary>
            public class FilterSwitch
            {
                /// <summary>
                /// Gets or sets the expression.
                /// </summary>
                /// <value>
                /// The filter switch expression.
                /// </value>
                public string Expression { get; set; }
            }
        }

        /// <summary>
        /// The serilog sinks to be configured.
        /// </summary>
        public class SinkDefinitions
        {
            /// <summary>
            /// Gets or sets the debug log.
            /// </summary>
            /// <value>
            /// The debug log sink.
            /// </value>
            public Sink DebugLog { get; set; }

            /// <summary>
            /// Gets or sets the diagnostic log.
            /// </summary>
            /// <value>
            /// The diagnostic log sink.
            /// </value>
            public Sink DiagnosticLog { get; set; }

            /// <summary>
            /// Gets or sets the event log.
            /// </summary>
            /// <value>
            /// The event log sink.
            /// </value>
            public Sink EventLog { get; set; }

            /// <summary>
            /// The serilog sink
            /// </summary>
            public class Sink
          {
                /// <summary>
                /// Gets or sets the log path.
                /// </summary>
                /// <value>
                /// The path of the log.
                /// </value>
                public string LogPath { get; set; }

                /// <summary>
                /// Gets or sets the name of the log.
                /// </summary>
                /// <value>
                /// The name of the log.
                /// </value>
                public string LogName { get; set; }

                /// <summary>
                /// Gets or sets the size of the log.
                /// </summary>
                /// <value>
                /// The maximum size of the log.
                /// </value>
                public int LogSize { get; set; }

                /// <summary>
                /// Gets or sets the log retention.
                /// </summary>
                /// <value>
                /// The length of time to keep the log.
                /// </value>
                public int LogRetention { get; set; }

                /// <summary>
                /// Gets or sets the output template.
                /// </summary>
                /// <value>
                /// The template that controls formatting the output.
                /// </value>
                public string OutputTemplate { get; set; }
          }
        }
    }
}