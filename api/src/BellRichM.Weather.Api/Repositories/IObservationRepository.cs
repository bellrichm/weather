using BellRichM.Weather.Api.Data;
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
        /// Create the observation.
        /// </summary>
        /// <param name="observation">The <see cref="Observation"/>.</param>
        /// <returns>The number of rows inserted.</returns>
        Task<int> CreateObservation(Observation observation);

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
