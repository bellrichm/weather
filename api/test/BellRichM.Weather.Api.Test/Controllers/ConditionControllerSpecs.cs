using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using BellRichM.Api.Models;
using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Service.Data;
using BellRichM.Weather.Api.Controllers;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Filters;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Services;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Test.Controllers
{
    internal abstract class ConditionControllerSpecs
    {
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";
        protected static LoggingData loggingData;
        protected static ConditionsController conditionsController;
        protected static Mock<ILoggerAdapter<ConditionsController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<IConditionService> conditionServiceMock;

        protected static TimePeriodModel timePeriodModel;

        protected static int offset;
        protected static int limit;
        protected static ConditionPageModel conditionPageModel;
        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            loggerMock = new Mock<ILoggerAdapter<ConditionsController>>();
            mapperMock = new Mock<IMapper>();
            conditionServiceMock = new Mock<IConditionService>();

            offset = 0;
            limit = 3;

            var conditionModels = new List<ConditionModel>
            {
                new ConditionModel
                {
                    Year = 2018,
                    Month = 9,
                    Day = 1,
                    Hour = 1,
                    WindGustDirection = 61.8725771445071,
                    WindGust = 4.00000994196379,
                    WindDirection = 59.8725771445071,
                    WindSpeed = 2.00000994196379,
                    OutsideTemperature = 67.2,
                    HeatIndex = 65.6,
                    Windchill = 83.0,
                    DewPoint = 60.8725771445071,
                    Barometer = 29.694,
                    RainRate = 0.0,
                    Rain = 4.00000994196379,
                    OutsideHumidity = 29.687
                }
            };

            var conditions = new List<Condition>
            {
                new Condition
                {
                    Year = 2018,
                    Month = 9,
                    Day = 1,
                    Hour = 1,
                    WindGustDirection = 61.8725771445071,
                    WindGust = 4.00000994196379,
                    WindDirection = 59.8725771445071,
                    WindSpeed = 2.00000994196379,
                    OutsideTemperature = 67.2,
                    HeatIndex = 65.6,
                    Windchill = 83.0,
                    DewPoint = 60.8725771445071,
                    Barometer = 29.694,
                    RainRate = 0.0,
                    Rain = 4.00000994196379,
                    OutsideHumidity = 29.687
                }
            };

            var paging = new Paging
            {
                Offset = offset,
                Limit = limit,
                TotalCount = 1
            };

            var conditionPage = new ConditionPage
            {
                Paging = paging,
                Conditions = conditions
            };

            var pagingModel = new PagingModel
            {
                Offset = offset,
                Limit = limit,
                TotalCount = 1
            };

            conditionPageModel = new ConditionPageModel
            {
                Paging = pagingModel,
                Conditions = conditionModels
            };

            mapperMock.Setup(x => x.Map<ConditionPageModel>(conditionPage)).Returns(conditionPageModel);

            conditionServiceMock.Setup(x => x.GetConditionsByDay(offset, limit, timePeriodModel)).ReturnsAsync(conditionPage);

            conditionsController = new ConditionsController(loggerMock.Object, mapperMock.Object, conditionServiceMock.Object);
            conditionsController.ControllerContext.HttpContext = new DefaultHttpContext();
            conditionsController.ControllerContext.HttpContext.TraceIdentifier = "traceIdentifier";
        };

        Cleanup after = () =>
            conditionsController.Dispose();
    }

    internal class When_GetConditionsByDay_decorating_method : ConditionControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(ConditionsController).GetMethod("GetConditionsByDay");

        It should_have_ValidateConditionLimitAttribute_attribute = () =>
            methodInfo.Should().BeDecoratedWith<ValidateConditionLimitAttribute>();
    }

    internal class When_GetConditionsByDay_model_state_is_not_valid : ConditionControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.ConditionsController_GetConditionsByDay,
                        "{@offset} {@limit} {@timePeriod}")
                },
                ErrorLoggingMessages = new List<string>()
            };
            conditionsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
        };

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        Cleanup after = () =>
            conditionsController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)conditionsController.GetConditionsByDay(offset, limit, timePeriodModel).Await();

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_GetConditionsByDay_is_successful : ConditionControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.ConditionsController_GetConditionsByDay,
                        "{@offset} {@limit} {@timePeriod}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (ObjectResult)conditionsController.GetConditionsByDay(offset, limit, timePeriodModel).Await();

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_a_conditionPageModel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_an_object_of_type_ConditionPageModel = () =>
            result.Value.Should().BeOfType<ConditionPageModel>();

        It should_return_the_minMaxConditionPageModel = () =>
        {
            var conditionPage = (ConditionPageModel)result.Value;
            conditionPage.Should().Equals(conditionPageModel);
        };
    }
}