using BellRichM.Weather.Api.Data;

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
        /// <returns>The <see cref="ConditionPage"/>.</returns>
         ConditionPage GetYearWeatherPage(int offset, int limit);
    }
}