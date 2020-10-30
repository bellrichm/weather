using BellRichM.Attribute.CodeCoverage;
using BellRichM.Api.Models;
using System.Collections.Generic;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The condition page.
    /// </summary>
    /// <seealso cref="MinMaxGroupModel" />
    [ExcludeFromCodeCoverage]
    public class MinMaxGroupPageModel
    {
        /// <summary>
        /// Gets or sets the paging.
        /// </summary>
        /// <value>
        /// The <see cref="Paging"/>.
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
        /// The <see cref="IEnumerable{MinMaxGroupModel}"/>.
        /// </value>
        public IEnumerable<MinMaxGroupModel> MinMaxGroups { get; set; }
    }
}