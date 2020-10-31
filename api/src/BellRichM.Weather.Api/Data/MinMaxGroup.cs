using BellRichM.Attribute.CodeCoverage;
using System.Collections.Generic;

namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The groupings of min max observation.
    /// </summary>
    /// <seealso cref="MinMaxCondition" />
    [ExcludeFromCodeCoverage]
    public class MinMaxGroup
    {
        /// <summary>
        /// Tthe min/max conditions.
        /// </summary>
        /// <value>
        /// The <see cref="List{MinMaxCondition}"/>.
        /// </value>
        private readonly List<MinMaxCondition> minMaxConditions = new List<MinMaxCondition>();

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
        /// Gets or sets the day of the year.
        /// </summary>
        /// <value>
        /// The dayof the year.
        /// </value>
        public int? DayOfYear { get; set; }

        /// <summary>
        /// Gets the min/max conditions.
        /// </summary>
        /// <value>
        /// The <see cref="List{MinMaxCondition}"/>.
        /// </value>
        public List<MinMaxCondition> MinMaxConditions
                                                    {
                                                        get { return minMaxConditions; }
                                                    }
    }
}