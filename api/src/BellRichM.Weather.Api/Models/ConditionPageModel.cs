using BellRichM.Api.Models;
using BellRichM.Attribute.CodeCoverage;
using System.Collections.Generic;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The condition page.
    /// </summary>
    /// <seealso cref="ConditionModel" />
    [ExcludeFromCodeCoverage]
    public class ConditionPageModel
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

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public IEnumerable<LinkModel> Links { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{ConditionModel}"/>.
        /// </value>
        public IEnumerable<ConditionModel> Conditions { get; set; }
    }
}