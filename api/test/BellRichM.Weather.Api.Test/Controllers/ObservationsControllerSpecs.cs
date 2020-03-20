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

            loggerMock = new Mock<ILoggerAdapter<ObservationsController>>();
            mapperMock = new Mock<IMapper>();
            observationServiceMock = new Mock<IObservationService>();

            mapperMock.Setup(x => x.Map<ObservationModel>(observation)).Returns(observationModel);
            mapperMock.Setup(x => x.Map<Observation>(observationModel)).Returns(observation);
            mapperMock.Setup(x => x.Map<Observation>(notFoundObservationModel)).Returns(notFoundObservation);

            observationServiceMock.Setup(x => x.GetObservation(notFoundObservationModel.DateTime)).Returns(Task.FromResult<Observation>(null));
            observationServiceMock.Setup(x => x.GetObservation(observationModel.DateTime)).Returns(Task.FromResult(observation));

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
            Because of = () => { };
        }

        internal class When_getting_nonexisting_observations
        {
            Because of = () => { };
        }

        internal class When_getting_observations_with_invalid_model
        {
            Because of = () => { };
        }

        internal class When_decorating_Observation_GetObservations_method
        {
            Because of = () => { };
        }

        internal class When_getting_observation_datetimes
        {
            Because of = () => { };
        }

        internal class When_getting_nonexisting_observation_datetimes
        {
            Because of = () => { };
        }

        internal class When_getting_observation_datetimes_with_invalid_model
        {
            Because of = () => { };
        }

        internal class When_decorating_Observation_GetObservationDateTimes_method
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
