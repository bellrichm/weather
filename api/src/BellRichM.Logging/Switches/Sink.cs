using BellRichM.Attribute.CodeCoverage;
using Serilog;

namespace BellRichM.Logging.Switches
{
    /// <summary>
    /// The serilog sink.
    /// </summary>
    [ExcludeFromCodeCoverage]

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
        /// Gets or sets the log rolling interval.
        /// </summary>
        /// <value>
        /// The <see cref="RollingInterval"/> class..
        /// </value>
        public RollingInterval RollingInterval { get; set; }

        /// <summary>
        /// Gets or sets the output template.
        /// </summary>
        /// <value>
        /// The template that controls formatting the output.
        /// </value>
        public string OutputTemplate { get; set; }
    }
}