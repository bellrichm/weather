using BellRichM.Weather.Api.Data;

namespace BellRichM.Weather.Api.Repositories
{
    /// <summary>
    /// The weather repository.
    /// </summary>
    public interface IWeatherRepository
    {
        /// <summary>
        /// Get the condition for years.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPage"/>.</returns>
        ConditionPage GetYear(int offset, int limit);
    }
}