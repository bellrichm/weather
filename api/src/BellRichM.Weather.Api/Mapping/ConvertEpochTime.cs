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
        /// <param name="observationModel">The observation model.</param>
        /// <param name="observation">The observation.</param>
        public void Process(ObservationModel observationModel, Observation observation)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // UTC vs Eastern...???
            var dateTime = epoch.AddSeconds(observation.DateTime);
            observation.Year = dateTime.Year;
            observation.Month = dateTime.Month;
            observation.Day = dateTime.Day;
            observation.Hour = dateTime.Hour;
            observation.Minute = dateTime.Minute;
        }
    }
}