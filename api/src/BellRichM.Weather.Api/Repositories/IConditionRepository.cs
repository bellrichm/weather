using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Repositories
{
    /// <summary>
    /// The condition repository.
    /// </summary>
    public interface IConditionRepository
    {
        /// <summary>
        /// Get the condition for years.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="MinMaxCondition"/>.</returns>
        Task<IEnumerable<MinMaxCondition>> GetYear(int offset, int limit);

        /// <summary>
        /// Get the condition for detail for an hour.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The mont.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <returns>The <see cref="MinMaxCondition"/>.</returns>
        Task<MinMaxCondition> GetHourDetail(int year, int month, int day, int hour);

        /// <summary>
        /// Gets the total count of year records.
        /// </summary>
        /// <returns>The count.</returns>
        Task<int> GetYearCount();

        /// <summary>
        /// Gets the total count of 'day' records.
        /// </summary>
        /// <returns>The count.</returns>
        Task<int> GetDayCount();

        /// <summary>
        /// Gets the min/max conditions by day and within a time period.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <param name="timePeriodModel">The time period.</param>
        /// <returns>The <see cref="MinMaxGroup"/>.</returns>
        Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByDay(int offset, int limit, TimePeriodModel timePeriodModel);

        /// <summary>
        /// Gets the conditions grouped (averaged) by day and within a time period.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <param name="timePeriodModel">The time period.</param>
        /// <returns>The observations.</returns>
        Task<IEnumerable<Condition>> GetConditionsByDay(int offset, int limit, TimePeriodModel timePeriodModel);
    }
}
