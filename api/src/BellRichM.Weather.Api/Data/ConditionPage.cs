using System.Collections.Generic;

namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The condition page.
    /// </summary>
    /// <seealso cref="Condition" />
    public class ConditionPage
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
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{Condition}"/>.
        /// </value>
        public IEnumerable<Condition> Conditions { get; set; }
    }
}