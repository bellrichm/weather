using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InitObservations
{
    /// <summary>
    /// The WeeWX repository.
    /// </summary>
    public interface IWeeWXRepository
    {
        /// <summary>
        /// Get it.
        /// </summary>
        /// <param name="startTimestamp">The start date.</param>
        /// <param name="endTimestamp">The end date.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="ObservationModel"/>.</returns>
        Task<List<ObservationModel>> GetWeather(long startTimestamp, long endTimestamp);
    }
}
