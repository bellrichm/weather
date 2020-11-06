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
        /// Gets the min/max conditions by minute.
        /// </summary>
        /// <param name="startMinute">The minute to start at.</param>
        /// <param name="endMinute">The minute to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of minutes to return.</param>
        /// <returns>The <see cref="MinMaxGroupPage"/>.</returns>
        Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByMinute(int startMinute, int endMinute, int offset, int limit);

        /// <summary>
        /// Gets the min/max conditions by hour.
        /// </summary>
        /// <param name="startHour">The hour to start at.</param>
        /// <param name="endHour">The hour to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of hours to return.</param>
        /// <returns>The <see cref="MinMaxGroupPage"/>.</returns>
        Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByHour(int startHour, int endHour, int offset, int limit);

        /// <summary>
        /// Gets the min/max conditions by day.
        /// </summary>
        /// <param name="startDayOfYear">The day of the year to start at.</param>
        /// <param name="endDayOfYear">The day of the year to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of days to return.</param>
        /// <returns>The <see cref="MinMaxGroup"/>.</returns>
        Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByDay(int startDayOfYear, int endDayOfYear, int offset, int limit);

        /// <summary>
        /// Gets the min/max conditions by week.
        /// </summary>
        /// <param name="startWeekOfYear">The week of the year to start at.</param>
        /// <param name="endWeekOfYear">The week of the year to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of weeks to return.</param>
        /// <returns>The <see cref="MinMaxGroupPage"/>.</returns>
        Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByWeek(int startWeekOfYear, int endWeekOfYear, int offset, int limit);

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
