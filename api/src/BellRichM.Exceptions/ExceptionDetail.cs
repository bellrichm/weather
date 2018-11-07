using System;
using System.Runtime.Serialization;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Exceptions
{
    /// <summary>
    /// Exception detail.
    /// </summary>
    [Serializable]
    public class ExceptionDetail
    {
#pragma warning disable CA2235
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

    #pragma warning restore CA2235
    }
}