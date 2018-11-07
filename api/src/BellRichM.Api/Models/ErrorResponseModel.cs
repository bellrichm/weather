using System.Collections.Generic;
using BellRichM.Attribute.CodeCoverage;
using BellRichM.Exceptions;

namespace BellRichM.Api.Models
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

        /// <summary>
        /// Gets or sets the errors details
        /// </summary>
        /// <value>
        /// The error details
        /// </value>
        public IEnumerable<ErrorDetailModel> ErrorDetails { get; set; }
    }
}