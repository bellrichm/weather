using AutoMapper;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System;

namespace BellRichM.Weather.Api.Mapping
{
    /// <summary>
    /// Converts the unix time on the observation model to a date and time on the observation.
    /// </summary>
    /// <remarks>
    /// <seealso cref="IMappingAction{src, dest}" />
    /// </remarks>
    public class ConvertEpochTime : IMappingAction<ObservationModel, Observation>
    {
        /// <summary>
        /// Converts the unix time on the observation model to a date and time on the observation.
        /// </summary>
        /// <param name="source">The observation model.</param>
        /// <param name="destination">The observation.</param>
        public void Process(ObservationModel source, Observation destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // UTC vs Eastern...???
            var dateTime = epoch.AddSeconds(destination.DateTime);
            destination.Year = dateTime.Year;
            destination.Month = dateTime.Month;
            destination.Day = dateTime.Day;
            destination.Hour = dateTime.Hour;
            destination.Minute = dateTime.Minute;
        }
    }
}