using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Api.Models
{
    /// <summary>
    /// The paging parm model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PagingParmModel
    {
        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>
        /// The limit.
        /// </value>
        public int Limit { get; set; }
    }
}
