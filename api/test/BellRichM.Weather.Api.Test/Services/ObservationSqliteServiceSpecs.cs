using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Repositories;
using BellRichM.Weather.Api.Services;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Test
{
    public class ObservationSqliteServiceSpecs
    {
        protected static LoggingData loggingData;
        protected static Observation observation;
        protected static Observation notFoundObservation;
        protected static List<Observation> expectedObservations;
        protected static List<Timestamp> expectedTimestamps;
        protected static TimePeriodModel observationsExistTimePeriod;

        protected static ObservationSqliteService observationSqliteService;

        protected static Mock<ILoggerAdapter<ObservationSqliteService>> loggerMock;
        protected static Mock<IObservationRepository> observationRepositoryMock;

        protected Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            observation = new Observation
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

            notFoundObservation = new Observation
            {
                DateTime = 0
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

            observationsExistTimePeriod = new TimePeriodModel
            {
                StartDateTime = 1472688000,
                EndDateTime = 1472688600
            };

            loggerMock = new Mock<ILoggerAdapter<ObservationSqliteService>>();
            observationRepositoryMock = new Mock<IObservationRepository>();

            observationRepositoryMock.Setup(x => x.GetObservation(notFoundObservation.DateTime)).Returns(Task.FromResult<Observation>(null));
            observationRepositoryMock.Setup(x => x.GetObservation(observation.DateTime)).Returns(Task.FromResult(observation));

            observationRepositoryMock.Setup(x => x.GetObservations(observationsExistTimePeriod)).Returns(Task.FromResult(expectedObservations));

            observationRepositoryMock.Setup(x => x.GetTimestamps(observationsExistTimePeriod)).Returns(Task.FromResult(expectedTimestamps));

            observationRepositoryMock.Setup(x => x.CreateObservation(notFoundObservation)).Returns(Task.FromResult(0));
            observationRepositoryMock.Setup(x => x.CreateObservation(observation)).Returns(Task.FromResult(1));

            observationRepositoryMock.Setup(x => x.UpdateObservation(notFoundObservation)).Returns(Task.FromResult(0));
            observationRepositoryMock.Setup(x => x.UpdateObservation(observation)).Returns(Task.FromResult(1));

            observationRepositoryMock.Setup(x => x.DeleteObservation(notFoundObservation.DateTime)).Returns(Task.FromResult(0));
            observationRepositoryMock.Setup(x => x.DeleteObservation(observation.DateTime)).Returns(Task.FromResult(1));

            observationSqliteService = new ObservationSqliteService(observationRepositoryMock.Object);
        };
    }

    internal class When_getting_an_existing_observation : ObservationSqliteServiceSpecs
    {
        private static Observation retrievedObservation;

        Because of = () =>
            retrievedObservation = observationSqliteService.GetObservation(observation.DateTime).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            retrievedObservation.Should().BeEquivalentTo(observation);
    }

    internal class When_getting_a_nonexisting_observation : ObservationSqliteServiceSpecs
    {
        private static Observation retrievedObservation;

        Because of = () =>
            retrievedObservation = observationSqliteService.GetObservation(notFoundObservation.DateTime).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            retrievedObservation.Should().BeNull();
    }

    internal class When_getting_existing_observations : ObservationSqliteServiceSpecs
    {
        protected static List<Observation> observations;

        Because of = () =>
            observations = observationSqliteService.GetObservations(observationsExistTimePeriod).Await();

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_data = () =>
            observations.Should().BeEquivalentTo(expectedObservations);
    }

    internal class When_getting_existing_observation_datetime : ObservationSqliteServiceSpecs
    {
        protected static List<Timestamp> timestamps;

        Because of = () =>
            timestamps = observationSqliteService.GetTimestamps(observationsExistTimePeriod).Await();

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_data = () =>
            timestamps.Should().BeEquivalentTo(expectedTimestamps);
    }

    internal class When_creating_an_observation_succeeds : ObservationSqliteServiceSpecs
    {
        private static Observation createdObservation;

        Because of = () =>
            createdObservation = observationSqliteService.CreateObservation(observation).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            createdObservation.Should().BeEquivalentTo(observation);
    }

    internal class When_creating_an_observation_fails : ObservationSqliteServiceSpecs
    {
        private static Observation createdObservation;

        Because of = () =>
            createdObservation = observationSqliteService.CreateObservation(notFoundObservation).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            createdObservation.Should().BeNull();
    }

    internal class When_updating_an_observation_succeeds : ObservationSqliteServiceSpecs
    {
        private static Observation updatedObservation;

        Because of = () =>
            updatedObservation = observationSqliteService.UpdateObservation(observation).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            updatedObservation.Should().BeEquivalentTo(observation);
    }

    internal class When_updating_an_observation_fails : ObservationSqliteServiceSpecs
    {
        private static Observation updatedObservation;

        Because of = () =>
            updatedObservation = observationSqliteService.UpdateObservation(notFoundObservation).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            updatedObservation.Should().BeNull();
    }

    internal class When_deleting_an_observation_succeeds : ObservationSqliteServiceSpecs
    {
        private static int count;

        Because of = () =>
            count = observationSqliteService.DeleteObservation(observation.DateTime).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            count.Should().Equals(1);
    }

    internal class When_deleting_an_observation_fails : ObservationSqliteServiceSpecs
    {
        private static int count;

        Because of = () =>
            count = observationSqliteService.DeleteObservation(notFoundObservation.DateTime).Result;

        Behaves_like<LoggingBehaviors<ObservationSqliteService>> correct_logging = () => { };

        It should_return_the_Observation = () =>
            count.Should().Equals(0);
    }
}
