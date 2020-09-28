using AutoMapper;
using BellRichM.Api.Models;
using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Controllers;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Services;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.TestControllers.Test
{
    public class ObservationsControllerSpecs
    {
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";
        protected const int InvalidDateTime = 0;

        protected static LoggingData loggingData;

        protected static ObservationModel observationModel;
        protected static Observation observation;
        protected static ObservationModel notFoundObservationModel;
        protected static Observation notFoundObservation;
        protected static ObservationModel invalidObservationModel;
        protected static List<Observation> observations;
        protected static List<ObservationModel> observationModels;
        protected static List<Timestamp> timestamps;
        protected static List<TimestampModel> timestampModels;
        protected static TimePeriodModel observationsExistTimePeriod;

        protected static ObservationsController observationsController;
        protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<IObservationService> observationServiceMock;

        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            observationModel = new ObservationModel
            {
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

            notFoundObservationModel = new ObservationModel
            {
                DateTime = 1
            };

            notFoundObservation = new Observation
            {
                DateTime = 1
            };

            invalidObservationModel = new ObservationModel();

            observations = new List<Observation>
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

            observationModels = new List<ObservationModel>
            {
                new ObservationModel
                {
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
                new ObservationModel
                {
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
                new ObservationModel
                {
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

            timestamps = new List<Timestamp>
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

            timestampModels = new List<TimestampModel>
            {
                new TimestampModel
                {
                    DateTime = 1472688000
                },
                new TimestampModel
                {
                    DateTime = 1472688300
                },
                new TimestampModel
                {
                    DateTime = 1472688600
                }
            };

            observationsExistTimePeriod = new TimePeriodModel
            {
                StartDateTime = 1472688000,
                EndDateTime = 1472688600
            };

            loggerMock = new Mock<ILoggerAdapter<ObservationsController>>();
            mapperMock = new Mock<IMapper>();
            observationServiceMock = new Mock<IObservationService>();

            mapperMock.Setup(x => x.Map<ObservationModel>(observation)).Returns(observationModel);
            mapperMock.Setup(x => x.Map<Observation>(observationModel)).Returns(observation);
            mapperMock.Setup(x => x.Map<Observation>(notFoundObservationModel)).Returns(notFoundObservation);
            mapperMock.Setup(x => x.Map<List<ObservationModel>>(observations)).Returns(observationModels);
            mapperMock.Setup(x => x.Map<List<TimestampModel>>(timestamps)).Returns(timestampModels);

            observationServiceMock.Setup(x => x.GetObservation(notFoundObservationModel.DateTime)).Returns(Task.FromResult<Observation>(null));
            observationServiceMock.Setup(x => x.GetObservation(observationModel.DateTime)).Returns(Task.FromResult(observation));

            observationServiceMock.Setup(x => x.GetObservations(observationsExistTimePeriod)).Returns(Task.FromResult(observations));

            observationServiceMock.Setup(x => x.GetTimestamps(observationsExistTimePeriod)).Returns(Task.FromResult(timestamps));

            observationServiceMock.Setup(x => x.CreateObservation(notFoundObservation)).Returns(Task.FromResult<Observation>(null));
            observationServiceMock.Setup(x => x.CreateObservation(observation)).Returns(Task.FromResult(observation));

            observationServiceMock.Setup(x => x.UpdateObservation(notFoundObservation)).Returns(Task.FromResult<Observation>(null));
            observationServiceMock.Setup(x => x.UpdateObservation(observation)).Returns(Task.FromResult(observation));

            observationServiceMock.Setup(x => x.DeleteObservation(notFoundObservationModel.DateTime)).Returns(Task.FromResult(0));
            observationServiceMock.Setup(x => x.DeleteObservation(observationModel.DateTime)).Returns(Task.FromResult(1));

            observationsController = new ObservationsController(loggerMock.Object, mapperMock.Object, observationServiceMock.Object);
            observationsController.ControllerContext.HttpContext = new DefaultHttpContext();
            observationsController.ControllerContext.HttpContext.TraceIdentifier = "traceIdentifier";
        };

        internal class When_getting_an_Existing_observation
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Get,
                            "{@dateTime}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (ObjectResult)observationsController.GetObservation(observationModel.DateTime).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_success_status_code = () =>
                result.StatusCode.Should().Equals(200);

            It should_return_the_observation_model = () =>
            {
                var retrievedObservationModel = (ObservationModel)result.Value;
                retrievedObservationModel.Should().BeEquivalentTo(observationModel);
            };
        }

        internal class When_getting_a_nonexisting_observation_model
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;
            private static NotFoundResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Get,
                            "{@dateTime}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (NotFoundResult)observationsController.GetObservation(notFoundObservationModel.DateTime).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_not_found_status_code = () =>
                result.StatusCode.Should().Equals(404);
        }

        internal class When_getting_an_observation_with_an_invalid_modelState
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    InformationTimes = 1,
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Get,
                            "{@dateTime}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
                observationsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
            };

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            Cleanup after = () =>
                observationsController.ModelState.Clear();

            Because of = () =>
                result = (ObjectResult)observationsController.GetObservation(InvalidDateTime).Await();

            It should_return_correct_result_type = () =>
                result.Should().BeOfType<BadRequestObjectResult>();

            It should_return_correct_status_code = () =>
                result.StatusCode.ShouldEqual(400);

            It should_return_a_ErrorResponseModel = () =>
                result.Value.Should().BeOfType<ErrorResponseModel>();
        }

        internal class When_decorating_Observation_GetObservation_method
        {
            private static MethodInfo methodInfo;

            Because of = () =>
            {
                methodInfo = typeof(ObservationsController).GetMethod("GetObservation");
            };

            It should_have_CanCreateRoles_policy = () =>
                methodInfo.Should()
                .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewObservations");
        }

        internal class When_getting_observations
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_GetObservations,
                            "{@timePeriod}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (ObjectResult)observationsController.GetObservations(observationsExistTimePeriod).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_success_status_code = () =>
                result.StatusCode.Should().Equals(200);

            It should_return_the_observation_model = () =>
            {
                var retrievedObservations = (List<ObservationModel>)result.Value;

                retrievedObservations.Should().BeEquivalentTo(observationModels);
            };
        }

        internal class When_getting_observations_with_invalid_model
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    InformationTimes = 1,
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_GetObservations,
                            "{@timePeriod}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
                observationsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
            };

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            Cleanup after = () =>
                observationsController.ModelState.Clear();

            Because of = () =>
                result = (ObjectResult)observationsController.GetObservations(new TimePeriodModel()).Await();

            It should_return_correct_result_type = () =>
                result.Should().BeOfType<BadRequestObjectResult>();

            It should_return_correct_status_code = () =>
                result.StatusCode.ShouldEqual(400);

            It should_return_a_ErrorResponseModel = () =>
                result.Value.Should().BeOfType<ErrorResponseModel>();
        }

        internal class When_decorating_Observation_GetObservations_method
        {
            Because of = () => { };
        }

        internal class When_getting_observation_datetimes
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_GetTimestamps,
                            "{@timePeriod}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (ObjectResult)observationsController.GetTimestamps(observationsExistTimePeriod).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_success_status_code = () =>
                result.StatusCode.Should().Equals(200);

            It should_return_the_observation_model = () =>
            {
                var retrievedTimestamps = (List<TimestampModel>)result.Value;

                retrievedTimestamps.Should().BeEquivalentTo(timestampModels);
            };
        }

        internal class When_getting_observation_datetimes_with_invalid_model
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    InformationTimes = 1,
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_GetTimestamps,
                            "{@timePeriod}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
                observationsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
            };

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            Cleanup after = () =>
                observationsController.ModelState.Clear();

            Because of = () =>
                result = (ObjectResult)observationsController.GetTimestamps(new TimePeriodModel()).Await();

            It should_return_correct_result_type = () =>
                result.Should().BeOfType<BadRequestObjectResult>();

            It should_return_correct_status_code = () =>
                result.StatusCode.ShouldEqual(400);

            It should_return_a_ErrorResponseModel = () =>
                result.Value.Should().BeOfType<ErrorResponseModel>();
        }

        internal class When_decorating_Observation_GetTimestamps_method
        {
            Because of = () => { };
        }

        internal class When_creating_an_observation_succeeds
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Create,
                            "{@observationCreate}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (ObjectResult)observationsController.Create(observationModel).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_success_status_code = () =>
                result.StatusCode.Should().Equals(200);

            It should_return_the_observation_model = () =>
            {
                var retrievedObservationModel = (ObservationModel)result.Value;
                retrievedObservationModel.Should().BeEquivalentTo(observationModel);
            };
        }

        internal class When_creating_an_observation_fails
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static NotFoundResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Create,
                            "{@observationCreate}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (NotFoundResult)observationsController.Create(notFoundObservationModel).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_not_found_status_code = () =>
                result.StatusCode.Should().Equals(404);
        }

        internal class When_vreating_an_observation_with_an_invalid_modelState
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    InformationTimes = 1,
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Create,
                            "{@observationCreate}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
                observationsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
            };

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            Cleanup after = () =>
                observationsController.ModelState.Clear();

            Because of = () =>
                result = (ObjectResult)observationsController.Create(invalidObservationModel).Await();

            It should_return_correct_result_type = () =>
                result.Should().BeOfType<BadRequestObjectResult>();

            It should_return_correct_status_code = () =>
                result.StatusCode.ShouldEqual(400);

            It should_return_a_ErrorResponseModel = () =>
                result.Value.Should().BeOfType<ErrorResponseModel>();
        }

        internal class When_decorating_Observation_Create_method
        {
            private static MethodInfo methodInfo;

            Because of = () =>
            {
                methodInfo = typeof(ObservationsController).GetMethod("Create");
            };

            It should_have_CanCreateRoles_policy = () =>
                methodInfo.Should()
                .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanCreateObservations");
        }

        internal class When_updating_an_observation_succeeds
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;
            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Update,
                            "{@observationUpdateModel}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (ObjectResult)observationsController.Update(observationModel).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_success_status_code = () =>
                result.StatusCode.Should().Equals(200);

            It should_return_the_observation_model = () =>
            {
                var retrievedObservationModel = (ObservationModel)result.Value;
                retrievedObservationModel.Should().BeEquivalentTo(observationModel);
            };
        }

        internal class When_updating_an_observation_fails
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static NotFoundResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Update,
                            "{@observationUpdateModel}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (NotFoundResult)observationsController.Update(notFoundObservationModel).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_not_found_status_code = () =>
                result.StatusCode.Should().Equals(404);
        }

        internal class When_updating_an_observation_with_an_invalid_modelState
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    InformationTimes = 1,
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Update,
                            "{@observationUpdateModel}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
                observationsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
            };

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            Cleanup after = () =>
                observationsController.ModelState.Clear();

            Because of = () =>
                result = (ObjectResult)observationsController.Update(invalidObservationModel).Await();

            It should_return_correct_result_type = () =>
                result.Should().BeOfType<BadRequestObjectResult>();

            It should_return_correct_status_code = () =>
                result.StatusCode.ShouldEqual(400);

            It should_return_a_ErrorResponseModel = () =>
                result.Value.Should().BeOfType<ErrorResponseModel>();
        }

        internal class When_decorating_Observation_Update_method
        {
            private static MethodInfo methodInfo;

            Because of = () =>
            {
                methodInfo = typeof(ObservationsController).GetMethod("Update");
            };

            It should_have_CanCreateRoles_policy = () =>
                methodInfo.Should()
                .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanUpdateObservations");
        }

        internal class When_deleting_an_observation_succeeds
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static NoContentResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Delete,
                            "{@dateTime}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (NoContentResult)observationsController.Delete(observationModel.DateTime).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_no_content_code = () =>
                result.StatusCode.ShouldEqual(204);
        }

        internal class When_deleting_an_observation_fails
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static NotFoundResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Delete,
                            "{@dateTime}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
            };

            Because of = () =>
                result = (NotFoundResult)observationsController.Delete(notFoundObservationModel.DateTime).Await();

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            It should_return_not_found_status_code = () =>
                result.StatusCode.Should().Equals(404);
        }

        internal class When_deleting_an_observation_with_an_invalid_modelState
        {
            protected static Mock<ILoggerAdapter<ObservationsController>> loggerMock;
            protected static LoggingData loggingData;

            private static ObjectResult result;

            Establish context = () =>
            {
                loggerMock = ObservationsControllerSpecs.loggerMock;
                loggingData = new LoggingData
                {
                    InformationTimes = 1,
                    EventLoggingData = new List<EventLoggingData>
                    {
                        new EventLoggingData(
                            EventId.ObservationsController_Delete,
                            "{@dateTime}")
                    },
                    ErrorLoggingMessages = new List<string>()
                };
                observationsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
            };

            Behaves_like<LoggingBehaviors<ObservationsController>> correct_logging = () => { };

            Cleanup after = () =>
                observationsController.ModelState.Clear();

            Because of = () =>
                result = (ObjectResult)observationsController.Delete(InvalidDateTime).Await();

            It should_return_correct_result_type = () =>
                result.Should().BeOfType<BadRequestObjectResult>();

            It should_return_correct_status_code = () =>
                result.StatusCode.ShouldEqual(400);

            It should_return_a_ErrorResponseModel = () =>
                result.Value.Should().BeOfType<ErrorResponseModel>();
        }

        internal class When_decorating_Observation_Delete_method
        {
            private static MethodInfo methodInfo;

            Because of = () =>
            {
                methodInfo = typeof(ObservationsController).GetMethod("Delete");
            };

            It should_have_CanCreateRoles_policy = () =>
                methodInfo.Should()
                .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanDeleteObservations");
        }
    }
}
