using BellRichM.Weather.Api.Data;
using System.Collections.Generic;

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
        /// <returns>The <see cref="Condition"/>.</returns>
        IEnumerable<Condition> GetYear(int offset, int limit);

        /// <summary>
        /// Gets the total count of year records.
        /// </summary>
        /// <returns>The count.</returns>
        int GetYearCount();
    }
}