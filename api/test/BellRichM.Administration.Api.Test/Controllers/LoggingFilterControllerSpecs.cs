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

namespace BellRichM.Administration.Api.Test.Controllers
{
    public class LoggingFilterControllerSpecs
    {
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";

        protected static int debugTimes;
        protected static int informationTimes;
        protected static int errorTimes;
        protected static int eventTimes;

        protected static LoggingFilterSwitchesModel loggingFilterSwitchesModel;

        protected static Mock<ILoggerAdapter<LoggingFilterController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<ILogManager> logManagerMock;

        protected static LoggingFilterController loggingFilterController;

        Establish context = () =>
        {
            debugTimes = 0;
            informationTimes = 0;
            errorTimes = 0;
            eventTimes = 1;

            loggingFilterSwitchesModel = new LoggingFilterSwitchesModel
            {
                ConsoleSinkFilterSwitch = new LoggingFilterSwitchModel { Expression = "true" }
            };

            loggerMock = new Mock<ILoggerAdapter<LoggingFilterController>>();
            mapperMock = new Mock<IMapper>();
            logManagerMock = new Mock<ILogManager>();

            mapperMock.Setup(x => x.Map<LoggingFilterSwitchesModel>(IT.IsAny<LoggingFilterSwitches>())).Returns(loggingFilterSwitchesModel);

            loggingFilterController = new LoggingFilterController(loggerMock.Object, logManagerMock.Object, mapperMock.Object);
            loggingFilterController.ControllerContext.HttpContext = new DefaultHttpContext();
            loggingFilterController.ControllerContext.HttpContext.TraceIdentifier = "traceIdentifier";
        };
    }

    internal class When_LoggingFilterSwitchesModel_is_not_valid : LoggingFilterControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            informationTimes = 1;
            loggingFilterController.ModelState.AddModelError(ErrorCode, ErrorMessage);
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<LoggingFilterController>> correct_logging;
#pragma warning restore 169

        It should_log_correct_information_messages = () =>
           loggerMock.Verify(x => x.LogDiagnosticInformation(IT.IsAny<string>(), IT.IsAny<ModelStateDictionary>()), Times.Once);

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<EventId>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Once);

        Cleanup after = () =>
            loggingFilterController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)loggingFilterController.Update(loggingFilterSwitchesModel);

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_updating_logging_filter_succeeds : LoggingFilterControllerSpecs
    {
        private static ObjectResult result;

        Because of = () =>
            result = (ObjectResult)loggingFilterController.Update(loggingFilterSwitchesModel);

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<LoggingFilterController>> correct_logging;
#pragma warning restore 169

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<EventId>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Once);

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_logging_filter_switches_model = () =>
            result.Value.Should().BeOfType<LoggingFilterSwitchesModel>();

        It should_return_a_logging_filter_switches_model = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_logging_filter_switches_model = () =>
        {
            var updatedLoggingFilterSwitchesModel = (LoggingFilterSwitchesModel)result.Value;
            updatedLoggingFilterSwitchesModel.Should().Equals(loggingFilterSwitchesModel);
        };
    }

    internal class When_decorating_LoggingFilter_update_method : LoggingFilterControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(LoggingFilterController).GetMethod("Update");

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanUpdateLoggingFilters");
    }

    internal class When_getting_logging_filter_succeeds : LoggingFilterControllerSpecs
    {
        private static ObjectResult result;

        Because of = () =>
            result = (ObjectResult)loggingFilterController.Get();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<LoggingFilterController>> correct_logging;
#pragma warning restore 169

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<EventId>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Once);

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_logging_filter_switches_model = () =>
            result.Value.Should().BeOfType<LoggingFilterSwitchesModel>();

        It should_return_a_logging_filter_switches_model = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_logging_filter_switches_model = () =>
        {
            var retrievedLoggingFilterSwitchesModel = (LoggingFilterSwitchesModel)result.Value;
            retrievedLoggingFilterSwitchesModel.Should().Equals(loggingFilterSwitchesModel);
        };
    }

    internal class When_decorating_LoggingFilter_get_method : LoggingFilterControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(LoggingFilterController).GetMethod("Get");

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewLoggingFilters");
    }
}