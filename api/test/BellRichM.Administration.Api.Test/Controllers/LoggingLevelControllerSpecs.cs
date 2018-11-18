using System.Reflection;
using AutoMapper;
using BellRichM.Administration.Api.Controllers;
using BellRichM.Administration.Api.Models;
using BellRichM.Api.Models;
using BellRichM.Logging;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Administration.Test.Controllers
{
    internal class LoggingLevelControllerSpecs
    {
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";

        protected static int debugTimes;
        protected static int informationTimes;
        protected static int errorTimes;
        protected static int eventTimes;

        protected static LoggingLevelSwitchesModel loggingLevelSwitchesModel;

        protected static Mock<ILoggerAdapter<LoggingLevelController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<ILogManager> logManagerMock;

        protected static LoggingLevelController loggingLevelController;

        Establish context = () =>
        {
            debugTimes = 0;
            informationTimes = 0;
            errorTimes = 0;
            eventTimes = 1;

            loggingLevelSwitchesModel = new LoggingLevelSwitchesModel
            {
                DefaultLoggingLevelSwitch = new LoggingLevelSwitchModel { MinimumLevel = "Verbose" }
            };

            loggerMock = new Mock<ILoggerAdapter<LoggingLevelController>>();
            mapperMock = new Mock<IMapper>();
            logManagerMock = new Mock<ILogManager>();

            mapperMock.Setup(x => x.Map<LoggingLevelSwitchesModel>(IT.IsAny<LoggingLevelSwitches>())).Returns(loggingLevelSwitchesModel);

            loggingLevelController = new LoggingLevelController(loggerMock.Object, logManagerMock.Object, mapperMock.Object);
            loggingLevelController.ControllerContext.HttpContext = new DefaultHttpContext();
            loggingLevelController.ControllerContext.HttpContext.TraceIdentifier = "traceIdentifier";
        };
    }

    internal class When_LoggingLevelSwitchesModel_is_not_valid : LoggingLevelControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            informationTimes = 1;
            loggingLevelController.ModelState.AddModelError(ErrorCode, ErrorMessage);
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<LoggingLevelController>> correct_logging;
#pragma warning restore 169

        It should_log_correct_information_messages = () =>
           loggerMock.Verify(x => x.LogDiagnosticInformation(IT.IsAny<string>(), IT.IsAny<ModelStateDictionary>()), Times.Once);

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<EventId>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Once);

        Cleanup after = () =>
            loggingLevelController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)loggingLevelController.Update(loggingLevelSwitchesModel);

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_updating_logging_level_succeeds : LoggingLevelControllerSpecs
    {
        private static ObjectResult result;

        Because of = () =>
            result = (ObjectResult)loggingLevelController.Update(loggingLevelSwitchesModel);

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<LoggingLevelController>> correct_logging;
#pragma warning restore 169

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<EventId>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Once);

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_logging_level_switches_model = () =>
            result.Value.Should().BeOfType<LoggingLevelSwitchesModel>();

        It should_return_a_logging_level_switches_model = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_logging_level_switches_model = () =>
        {
            var updatedLoggingLevelSwitchesModel = (LoggingLevelSwitchesModel)result.Value;
            updatedLoggingLevelSwitchesModel.Should().Equals(loggingLevelSwitchesModel);
        };
    }

    internal class When_decorating_LoggingLevel_update_method : LoggingLevelControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(LoggingLevelController).GetMethod("Update");

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanUpdateLoggingLevels");
    }

    internal class When_getting_logging_level_succeeds : LoggingLevelControllerSpecs
    {
        private static ObjectResult result;

        Because of = () =>
            result = (ObjectResult)loggingLevelController.Get();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<LoggingLevelController>> correct_logging;
#pragma warning restore 169

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<EventId>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Once);

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_logging_level_switches_model = () =>
            result.Value.Should().BeOfType<LoggingLevelSwitchesModel>();

        It should_return_a_logging_level_switches_model = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_logging_level_switches_model = () =>
        {
            var retrievedLoggingLevelSwitchesModel = (LoggingLevelSwitchesModel)result.Value;
            retrievedLoggingLevelSwitchesModel.Should().Equals(loggingLevelSwitchesModel);
        };
    }

    internal class When_decorating_LoggingLevel_get_method : LoggingLevelControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(LoggingLevelController).GetMethod("Get");

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewLoggingLevels");
    }
}