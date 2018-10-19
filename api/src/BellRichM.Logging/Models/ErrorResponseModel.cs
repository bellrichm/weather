using System.Collections.Generic;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Logging
{
    /// <summary>
    /// The reponse when the request has an error.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ErrorResponseModel
    {
        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier of the quest that has the error.
        /// </value>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<Error> Errors { get; set; }

        /// <summary>
        /// Error detail.
        /// </summary>
        public class Error
        {
            /// <summary>
            /// Gets or sets the code.
            /// </summary>
            /// <value>
            /// The error code.
            /// </value>
            public int Code { get; set; }

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>
            /// The human readable error text.
            /// </value>
            public string Text { get; set; }
        }
    }
}