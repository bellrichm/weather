using BellRichM.Attribute.CodeCoverage;
using System.Collections.Generic;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The groupings of min max observation.
    /// </summary>
    /// <seealso cref="MinMaxConditionModel" />
    [ExcludeFromCodeCoverage]
    public class MinMaxGroupModel
    {
        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public int? Month { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public int? Day { get; set; }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>
        /// The hour.
        /// </value>
        public int? Hour { get; set; }

        /// <summary>
        /// Gets or sets the min/max conditions.
        /// </summary>
        /// <value>
        /// The <see cref="List{MinMaxConditionModel}"/>.
        /// </value>
        public IEnumerable<MinMaxConditionModel> MinMaxConditions { get; set; }
    }
}