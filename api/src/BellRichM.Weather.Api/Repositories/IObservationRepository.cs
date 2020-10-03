using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Repositories
{
    /// <summary>
    /// The observatiom repository.
    /// </summary>
    public interface IObservationRepository
    {
        /// <summary>
        /// Get the observation.
        /// </summary>
        /// <param name="dateTime">The date time of the observation to retrieve.</param>
        /// <returns>The <see cref="Observation"/>.</returns>
        Task<Observation> GetObservation(int dateTime);

        /// <summary>
        /// Gets the observations for a time period.
        /// </summary>
        /// <param name="timePeriodModel">The time period.</param>
        /// <returns>The observations.</returns>
        Task<List<Observation>> GetObservations(TimePeriodModel timePeriodModel);

        /// <summary>
        /// Gets the timestamps for a time period.
        /// </summary>
        /// <param name="timePeriodModel">The time period.</param>
        /// <returns>The timestamps.</returns>
        Task<List<Timestamp>> GetTimestamps(TimePeriodModel timePeriodModel);

        /// <summary>
        /// Create the observation.
        /// </summary>
        /// <param name="observation">The <see cref="Observation"/>.</param>
        /// <returns>The number of rows inserted.</returns>
        Task<int> CreateObservation(Observation observation);

                /// <summary>
        /// Create the observations.
        /// </summary>
        /// <param name="observations">The <see cref="Observation"/>.</param>
        /// <returns>The number of rows inserted.</returns>
        Task<int> CreateObservations(List<Observation> observations);

        /// <summary>
        /// Update the observation.
        /// </summary>
        /// <param name="observation">The <see cref="Observation"/>.</param>
        /// <returns>The number of rows updated.</returns>
        Task<int> UpdateObservation(Observation observation);

        /// <summary>
        /// Delete the observation.
        /// </summary>
        /// <param name="dateTime">The date time of the observation.</param>
        /// <returns>The number of rows deleted.</returns>
        Task<int> DeleteObservation(int dateTime);
    }
}
