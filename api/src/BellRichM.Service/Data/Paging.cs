using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Service.Data
{
    /// <summary>
    /// The paging.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Paging
    {
        /// <summary>
        /// Gets or sets the total count.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
        public int TotalCount { get; set; }

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