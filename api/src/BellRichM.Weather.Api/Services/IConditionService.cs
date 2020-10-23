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
        /// Gets the conditions grouped (averaged) by day and within a time period.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>        
        /// <param name="timePeriodModel">The time period.</param>
        /// <returns>The <see cref="ConditionPage"/>.</returns>
        Task<ConditionPage> GetConditionsByDay(int offset, int limit, TimePeriodModel timePeriodModel);
    }
}