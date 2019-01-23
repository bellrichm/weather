using BellRichM.Weather.Api.Data;
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
        /// <returns>The <see cref="Condition"/>.</returns>
        Task<IEnumerable<Condition>> GetYear(int offset, int limit);

        /// <summary>
        /// Get the condition for detail for an hour.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The mont.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <returns>The <see cref="Condition"/>.</returns>
        Task<Condition> GetHourDetail(int year, int month, int day, int hour);

        /// <summary>
        /// Gets the total count of year records.
        /// </summary>
        /// <returns>The count.</returns>
        Task<int> GetYearCount();
    }
}