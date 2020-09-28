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
        protected static List<Observation> expectedObservations;
        protected static List<Timestamp> expectedTimestamps;

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

            expectedObservations = new List<Observation>
            {
                new Observation
                {
                    Year = 2016,
                    Month = 9,
                    Day = 1,
                    Hour = 0,
                    Minute = 0,
                    DateTime = 1472688000,
                    USUnits = 1,
                    Interval = 5,
                    Barometer = 29.237,
                    Pressure = 28.713194886270053,
                    Altimeter = 29.220571639586623,
                    OutsideTemperature = 71.2,
                    OutsideHumidity = 84.0,
                    WindSpeed = 0.0,
                    WindDirection = null,
                    WindGust = 1.9999999981632,
                    WindGustDirection = 270.0,
                    RainRate = 0.0,
                    Rain = 0.0,
                    DewPoint = 66.10878229422875,
                    Windchill = 71.2,
                    HeatIndex = 71.2,
                    Evapotranspiration = 0.0,
                    Radiation = 0.0,
                    Ultraviolet = 0.0,
                    ExtraTemperature1 = null,
                    ExtraTemperature2 = null,
                    ExtraTemperature3 = null,
                    SoilTemperature1 = null,
                    SoilTemperature2 = null,
                    SoilTemperature3 = null,
                    SoilTemperature4 = null,
                    LeafTemperature1 = null,
                    LeafTemperature2 = null,
                    ExtraHumidity1 = null,
                    ExtraHumidity2 = null,
                    SoilMoisture1 = null,
                    SoilMoisture2 = null,
                    SoilMoisture3 = null,
                    SoilMoisture4 = null,
                    LeafWetness1 = null,
                    LeafWetness2 = null
                },
                new Observation
                {
                    Year = 2016,
                    Month = 9,
                    Day = 1,
                    Hour = 0,
                    Minute = 5,
                    DateTime = 1472688300,
                    USUnits = 1,
                    Interval = 5,
                    Barometer = 29.241,
                    Pressure = 28.717161004534812,
                    Altimeter = 29.224595415207432,
                    OutsideTemperature = 71.1,
                    OutsideHumidity = 85.0,
                    WindSpeed = 0.0,
                    WindDirection = null,
                    WindGust = 0.9999999990816,
                    WindGustDirection = 270.0,
                    RainRate = 0.0,
                    Rain = 0.0,
                    DewPoint = 66.35286439744459,
                    Windchill = 71.1,
                    HeatIndex = 71.1,
                    Evapotranspiration = 0.0,
                    Radiation = 0.0,
                    Ultraviolet = 0.0,
                    ExtraTemperature1 = null,
                    ExtraTemperature2 = null,
                    ExtraTemperature3 = null,
                    SoilTemperature1 = null,
                    SoilTemperature2 = null,
                    SoilTemperature3 = null,
                    SoilTemperature4 = null,
                    LeafTemperature1 = null,
                    LeafTemperature2 = null,
                    ExtraHumidity1 = null,
                    ExtraHumidity2 = null,
                    SoilMoisture1 = null,
                    SoilMoisture2 = null,
                    SoilMoisture3 = null,
                    SoilMoisture4 = null,
                    LeafWetness1 = null,
                    LeafWetness2 = null
                },
                new Observation
                {
                    Year = 2016,
                    Month = 9,
                    Day = 1,
                    Hour = 0,
                    Minute = 10,
                    DateTime = 1472688600,
                    USUnits = 1,
                    Interval = 5,
                    Barometer = 29.242,
                    Pressure = 28.718112682295047,
                    Altimeter = 29.225560927735184,
                    OutsideTemperature = 70.9,
                    OutsideHumidity = 85.0,
                    WindSpeed = 0.0,
                    WindDirection = null,
                    WindGust = 0.9999999990816,
                    WindGustDirection = 270.0,
                    RainRate = 0.0,
                    Rain = 0.0,
                    DewPoint = 66.15690929188126,
                    Windchill = 70.9,
                    HeatIndex = 70.9,
                    Evapotranspiration = 0.0,
                    Radiation = 0.0,
                    Ultraviolet = 0.0,
                    ExtraTemperature1 = null,
                    ExtraTemperature2 = null,
                    ExtraTemperature3 = null,
                    SoilTemperature1 = null,
                    SoilTemperature2 = null,
                    SoilTemperature3 = null,
                    SoilTemperature4 = null,
                    LeafTemperature1 = null,
                    LeafTemperature2 = null,
                    ExtraHumidity1 = null,
                    ExtraHumidity2 = null,
                    SoilMoisture1 = null,
                    SoilMoisture2 = null,
                    SoilMoisture3 = null,
                    SoilMoisture4 = null,
                    LeafWetness1 = null,
                    LeafWetness2 = null
                }
            };

            expectedTimestamps = new List<Timestamp>
            {
                new Timestamp
                {
                    DateTime = 1472688000
                },
                new Timestamp
                {
                    DateTime = 1472688300
                },
                new Timestamp
                {
                    DateTime = 1472688600
                }
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
        protected static List<Observation> observations;

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
            observations = observationRepository.GetObservations(
                new Weather.Api.Models.TimePeriodModel
                {
                    StartDateTime = 1472688000,
                    EndDateTime = 1472688600
                })
            .Await();

        Behaves_like<LoggingBehaviors<ObservationRepository>> correct_logging = () => { };

        It should_return_the_data = () =>
            observations.Should().BeEquivalentTo(expectedObservations);
    }

    internal class When_getting_observation_datetimes : ObservationRepositorySpecs
    {
        protected static List<Timestamp> timestamps;

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
            timestamps = observationRepository.GetTimestamps(
                new Weather.Api.Models.TimePeriodModel
                {
                    StartDateTime = 1472688000,
                    EndDateTime = 1472688600
                })
            .Await();

        Behaves_like<LoggingBehaviors<ObservationRepository>> correct_logging = () => { };

        It should_return_the_data = () =>
            timestamps.Should().BeEquivalentTo(expectedTimestamps);
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