using BellRichM.Api.Models;
using BellRichM.Attribute.CodeCoverage;
using System.Collections.Generic;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The condition page.
    /// </summary>
    /// <seealso cref="MinMaxConditionModel" />
    [ExcludeFromCodeCoverage]
    public class MinMaxConditionPageModel
    {
        /// <summary>
        /// Gets or sets the paging.
        /// </summary>
        /// <value>
        /// The <see cref="PagingModel"/>.
        /// </value>
        public PagingModel Paging { get; set; }

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
        public IEnumerable<MinMaxConditionModel> MinMaxConditions { get; set; }
    }
}