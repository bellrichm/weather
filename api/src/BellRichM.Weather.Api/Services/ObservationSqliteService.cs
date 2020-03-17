using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Services
{
    /// <summary>
    /// The Sqlite implementation of the Observation service.
    /// </summary>
    public class ObservationSqliteService : IObservationService
    {
        private readonly IObservationRepository _observationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationSqliteService"/> class.
        /// </summary>
        /// <param name="observationRepository">The <see cref="IObservationRepository"/>.</param>
        public ObservationSqliteService(IObservationRepository observationRepository)
        {
            _observationRepository = observationRepository;
        }

        /// <inheritdoc/>
        public async Task<Observation> CreateObservation(Observation observation)
        {
            if (observation == null)
            {
                throw new ArgumentNullException(nameof(observation));
            }

            var count = await _observationRepository.CreateObservation(observation).ConfigureAwait(true);
            if (count == 0)
            {
                return null;
            }

            var updatedObservation = await _observationRepository.GetObservation(observation.DateTime).ConfigureAwait(true);
            return updatedObservation;
        }

        /// <inheritdoc/>
        public Task<int> DeleteObservation(int dateTime)
        {
            return _observationRepository.DeleteObservation(dateTime);
        }

        /// <inheritdoc/>
        public Task<Observation> GetObservation(int dateTime)
        {
            return _observationRepository.GetObservation(dateTime);
        }

        /// <inheritdoc/>
        public Task<List<Observation>> GetObservations(TimePeriodModel timePeriod)
        {
            return _observationRepository.GetObservations(timePeriod);
        }

        /// <inheritdoc/>
        public Task<List<ObservationDateTime>> GetObservationDateTimes(TimePeriodModel timePeriod)
        {
            return _observationRepository.GetObservationDateTimes(timePeriod);
        }

        /// <inheritdoc/>
        public async Task<Observation> UpdateObservation(Observation observation)
        {
            if (observation == null)
            {
                throw new ArgumentNullException(nameof(observation));
            }

            var count = await _observationRepository.UpdateObservation(observation).ConfigureAwait(true);
            if (count == 0)
            {
                return null;
            }

            var newObservation = await _observationRepository.GetObservation(observation.DateTime).ConfigureAwait(true);
            return newObservation;
        }
    }
}