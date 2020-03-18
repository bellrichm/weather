using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.TestRunner;
using BellRichM.Weather.Api.Data;
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

            loggerMock = new Mock<ILoggerAdapter<ObservationSqliteService>>();
            observationRepositoryMock = new Mock<IObservationRepository>();

            observationRepositoryMock.Setup(x => x.GetObservation(notFoundObservation.DateTime)).Returns(Task.FromResult<Observation>(null));
            observationRepositoryMock.Setup(x => x.GetObservation(observation.DateTime)).Returns(Task.FromResult(observation));

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

    class Program
    {
        static void Main()
        {
            var embeddedRunner = new EmbeddedRunner(typeof(ObservationSqliteServiceSpecs));
            embeddedRunner.OnAssemblyStart();
            embeddedRunner.RunTests();
            embeddedRunner.OnAssemblyComplete();
        }
    }
}
