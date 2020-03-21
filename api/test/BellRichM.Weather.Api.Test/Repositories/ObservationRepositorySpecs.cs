using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Repositories;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Data.Sqlite;
using Moq;

using System.Collections.Generic;

using It = Machine.Specifications.It;

namespace BellRichM.Dummy
{
    public class ObservationRepositorySpecs
    {
        protected static LoggingData loggingData;

        protected static Observation originalObservation;
        protected static Observation updatedObservation;

        protected static Mock<ILoggerAdapter<ObservationRepository>> loggerMock;

        protected static ObservationRepository observationRepository;

        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            originalObservation = new Observation
            {
                Year = 2001,
                Month = 9,
                Day = 1,
                Hour = 1,
                Minute = 5,
                DateTime = 999306300,
                USUnits = 1,
                Interval = 5,
                Barometer = 29.688,
                Pressure = 29.172642123245641,
                Altimeter = 29.686688005475741,
                OutsideTemperature = 67.0,
                OutsideHumidity = 80.0,
                WindSpeed = 1.0000024854909466,
                WindDirection = 135.0,
                WindGust = 2.0000049709818932,
                WindGustDirection = 135.0,
                RainRate = 0.0,
                Rain = 0.0,
                DewPoint = 60.619405344958267,
                Windchill = 67.0,
                HeatIndex = 67.0,
                Evapotranspiration = 0.0,
                Radiation = 0.0,
                Ultraviolet = 0.0
            };

            updatedObservation = new Observation
            {
                Year = 2001,
                Month = 9,
                Day = 1,
                Hour = 1,
                Minute = 5,
                DateTime = 999306300,
                USUnits = 1,
                Interval = 5,
                Barometer = 30.688,
                Pressure = 30.172642123245641,
                Altimeter = 30.686688005475741,
                OutsideTemperature = 68.0,
                OutsideHumidity = 81.0,
                WindSpeed = 2.0000024854909466,
                WindDirection = 136.0,
                WindGust = 3.0000049709818932,
                WindGustDirection = 136.0,
                RainRate = 1.0,
                Rain = 1.0,
                DewPoint = 61.619405344958267,
                Windchill = 68.0,
                HeatIndex = 68.0,
                Evapotranspiration = 1.0,
                Radiation = 1.0,
                Ultraviolet = 1.0
            };

            loggerMock = new Mock<ILoggerAdapter<ObservationRepository>>();

            var dbProviderFactory = SqliteFactory.Instance;
            var observationRepositoryDbProviderFactory = new ObservationRepositoryDbProviderFactory(dbProviderFactory);
            var observationRepositoryConfiguration = new ObservationRepositoryConfiguration
            {
                ConnectionString = "Data Source=TestObservationRepository;Mode=Memory;Cache=Shared"
            };

            observationRepository = new ObservationRepository(loggerMock.Object, observationRepositoryDbProviderFactory, observationRepositoryConfiguration);
        };
    }

    internal class When_creating_an_observation : ObservationRepositorySpecs
    {
        protected static int rowCount;
        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                DebugTimes = 2, // because also calling GetObservation method
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            rowCount = observationRepository.CreateObservation(originalObservation).Result;

        Behaves_like<LoggingBehaviors<ObservationRepository>> correct_logging = () => { };

        It should_insert_one_row = () =>
            rowCount.Should().Equals(1);

        It should_persist_the_data = () =>
        {
            var obs = observationRepository.GetObservation(originalObservation.DateTime).Result;
            obs.Should().BeEquivalentTo(originalObservation);
        };
    }

    internal class When_getting_an_observation : ObservationRepositorySpecs
    {
        protected static Observation observation;
        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                DebugTimes = 1,
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            observation = observationRepository.GetObservation(originalObservation.DateTime).Result;

        Behaves_like<LoggingBehaviors<ObservationRepository>> correct_logging = () => { };

        It should_return_the_data = () =>
            observation.Should().BeEquivalentTo(originalObservation);
    }

    internal class When_getting_observations : ObservationRepositorySpecs
    {
        Because of = () => { };
    }

    internal class When_getting_observation_datetimes : ObservationRepositorySpecs
    {
        Because of = () => { };
    }

    internal class When_updating_an_observation : ObservationRepositorySpecs
    {
        protected static int rowCount;
        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                DebugTimes = 2, // because also calling GetObservation method
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            rowCount = observationRepository.UpdateObservation(updatedObservation).Result;

        Behaves_like<LoggingBehaviors<ObservationRepository>> correct_logging = () => { };

        It should_update_one_row = () =>
            rowCount.Should().Equals(1);

        It should_persist_the_data = () =>
        {
            var observation = observationRepository.GetObservation(updatedObservation.DateTime).Result;
            observation.Should().BeEquivalentTo(updatedObservation);
        };
    }

    internal class When_deleting_an_observation : ObservationRepositorySpecs
    {
        protected static int rowCount;
        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                DebugTimes = 2, // because also calling GetObservation method
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            rowCount = observationRepository.DeleteObservation(originalObservation.DateTime).Result;

        Behaves_like<LoggingBehaviors<ObservationRepository>> correct_logging = () => { };

        It should_delete_one_row = () =>
            rowCount.Should().Equals(1);

        It should_persist_the_data = () =>
        {
            var observation = observationRepository.GetObservation(originalObservation.DateTime).Result;
            observation.Should().BeNull();
        };
    }
}