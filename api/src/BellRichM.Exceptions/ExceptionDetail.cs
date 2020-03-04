using System;

namespace BellRichM.Exceptions
{
    /// <summary>
    /// Exception detail.
    /// </summary>
    [Serializable]
    public class ExceptionDetail
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The human readable error text.
        /// </value>
        public string Text { get; set; }
    }
}
