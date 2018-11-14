using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Logging.Switches
{
    /// <summary>
    /// The serilog sinks to be configured.
    /// </summary>
    [ExcludeFromCodeCoverage]
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
    }
}