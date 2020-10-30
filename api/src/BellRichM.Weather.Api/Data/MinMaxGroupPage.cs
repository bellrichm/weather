using BellRichM.Attribute.CodeCoverage;
using BellRichM.Service.Data;
using System.Collections.Generic;

namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The min max group page.
    /// </summary>
    /// <seealso cref="MinMaxGroup" />
    [ExcludeFromCodeCoverage]
    public class MinMaxGroupPage
    {
        /// <summary>
        /// Gets or sets the paging.
        /// </summary>
        /// <value>
        /// The <see cref="Paging"/>.
        /// </value>
        public Paging Paging { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{MinMaxCondition}"/>.
        /// </value>
        public IEnumerable<MinMaxGroup> MinMaxGroups { get; set; }
    }
}