using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Services
{
    /// <summary>
    /// The condition service.
    /// </summary>
    public interface IConditionService
    {
        /// <summary>
        /// Get the condition page for years.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="MinMaxConditionPage"/>.</returns>
        Task<MinMaxConditionPage> GetYearWeatherPage(int offset, int limit);

        /// <summary>
        /// Gets the min/max conditions by minute.
        /// </summary>
        /// <param name="dayOfYear">The day of the year to get data for.</param>
        /// <param name="startHour">The hour to start at.</param>
        /// <param name="endHour">The hoyr to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of minutes to return.</param>
        /// <returns>The <see cref="MinMaxGroupPage"/>.</returns>
        Task<MinMaxGroupPage> GetMinMaxConditionsByMinute(int dayOfYear, int startHour, int endHour, int offset, int limit);

        /// <summary>
        /// Gets the min/max conditions by hour.
        /// </summary>
        /// <param name="startHour">The hour to start at.</param>
        /// <param name="endHour">The hour to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of hours to return.</param>
        /// <returns>The <see cref="MinMaxGroupPage"/>.</returns>
        Task<MinMaxGroupPage> GetMinMaxConditionsByHour(int startHour, int endHour, int offset, int limit);

        /// <summary>
        /// Gets the min/max conditions by day.
        /// </summary>
        /// <param name="startDayOfYear">The day of the year to start at.</param>
        /// <param name="endDayOfYear">The day of the year to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of days to return.</param>
        /// <returns>The <see cref="MinMaxGroupPage"/>.</returns>
        Task<MinMaxGroupPage> GetMinMaxConditionsByDay(int startDayOfYear, int endDayOfYear, int offset, int limit);

        /// <summary>
        /// Gets the min/max conditions by week.
        /// </summary>
        /// <param name="startWeekOfYear">The week of the year to start at.</param>
        /// <param name="endWeekOfYear">The week of the year to end at.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of weeks to return.</param>
        /// <returns>The <see cref="MinMaxGroupPage"/>.</returns>
        Task<MinMaxGroupPage> GetMinMaxConditionsByWeek(int startWeekOfYear, int endWeekOfYear, int offset, int limit);

        /// <summary>
        /// Gets the conditions grouped (averaged) by day and within a time period.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <param name="timePeriodModel">The time period.</param>
        /// <returns>The <see cref="ConditionPage"/>.</returns>
        Task<ConditionPage> GetConditionsByDay(int offset, int limit, TimePeriodModel timePeriodModel);
    }
}