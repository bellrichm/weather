using BellRichM.Attribute.CodeCoverage;
using BellRichM.Service.Data;
using System.Collections.Generic;

namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The condition page.
    /// </summary>
    /// <seealso cref="MinMaxCondition" />
    [ExcludeFromCodeCoverage]
    public class MinMaxConditionPage
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
        /// The <see cref="IEnumerable{Condition}"/>.
        /// </value>
        public IEnumerable<MinMaxCondition> MinMaxConditions { get; set; }
    }
}