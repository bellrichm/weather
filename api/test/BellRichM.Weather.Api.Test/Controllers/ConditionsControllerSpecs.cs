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
    internal abstract class ConditionsControllerSpecs
    {
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";
        protected static LoggingData loggingData;
        protected static int year;
        protected static int month;
        protected static int day;
        protected static int hour;
        protected static int offset;
        protected static int limit;

        protected static ConditionPageModel conditionPageModel;

        protected static ConditionsController conditionsController;

        protected static Mock<ILoggerAdapter<ConditionsController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<IConditionService> conditionServiceMock;

        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>(),
            ErrorLoggingMessages = new List<string>()
            };

            year = 2001;
            month = 6;
            day = 1;
            hour = 11;

            offset = 0;
            limit = 3;

            var conditions = new List<Condition>
            {
                new Condition
                {
                    Year = 2018,
                    Month = 9,
                    Day = 1,
                    Hour = 1,
                    MaxTemp = "67.2",
                    MinTemp = "65.6",
                    MaxHumidity = "83.0",
                    MinHumidity = "80.0",
                    MaxDewpoint = "60.8725771445071",
                    MinDewpoint = "60.0932637870109",
                    MaxHeatIndex = "67.2",
                    MinWindchill = "65.6",
                    MaxBarometer = "29.694",
                    MinBarometer = "29.687",
                    MaxET = "0.001",
                    MinET = "0.0",
                    MaxUV = "0.0",
                    MinUV = "0.0",
                    MaxRadiation = "0.0",
                    MinRadiation = "0.0",
                    MaxRainRate = "0.0",
                    MaxWindGust = "4.00000994196379"
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

            var conditionModels = new List<ConditionModel>
            {
                new ConditionModel
                {
                    Year = 2018,
                    Month = 9,
                    Day = 1,
                    Hour = 1,
                    MaxTemp = "67.2",
                    MinTemp = "65.6",
                    MaxHumidity = "83.0",
                    MinHumidity = "80.0",
                    MaxDewpoint = "60.8725771445071",
                    MinDewpoint = "60.0932637870109",
                    MaxHeatIndex = "67.2",
                    MinWindchill = "65.6",
                    MaxBarometer = "29.694",
                    MinBarometer = "29.687",
                    MaxET = "0.001",
                    MinET = "0.0",
                    MaxUV = "0.0",
                    MinUV = "0.0",
                    MaxRadiation = "0.0",
                    MinRadiation = "0.0",
                    MaxRainRate = "0.0",
                    MaxWindGust = "4.00000994196379"
                }
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

            loggerMock = new Mock<ILoggerAdapter<ConditionsController>>();
            mapperMock = new Mock<IMapper>();
            conditionServiceMock = new Mock<IConditionService>();

            mapperMock.Setup(x => x.Map<ConditionPageModel>(conditionPage)).Returns(conditionPageModel);

            conditionServiceMock.Setup(x => x.GetYearWeatherPage(offset, limit)).ReturnsAsync(conditionPage);

            conditionsController = new ConditionsController(loggerMock.Object, mapperMock.Object, conditionServiceMock.Object);
            conditionsController.ControllerContext.HttpContext = new DefaultHttpContext();
            conditionsController.ControllerContext.HttpContext.TraceIdentifier = "traceIdentifier";
        };

        Cleanup after = () =>
            conditionsController.Dispose();
    }

    internal class When_getting_conditions_for_years : ConditionsControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.ConditionsController_GetYearsConditionPage,
                        "{@offset} {@limit}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (ObjectResult)conditionsController.GetYearsConditionPage(offset, limit).Await();

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_a_conditionPageModel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_an_object_of_type_ConditionPageModel = () =>
            result.Value.Should().BeOfType<ConditionPageModel>();

        It should_return_the_conditionPageModel = () =>
        {
            var conditionPage = (ConditionPageModel)result.Value;
            conditionPage.Should().Equals(conditionPageModel);
        };
    }

    internal class When_model_state_is_not_valid : ConditionsControllerSpecs
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
                        EventId.ConditionsController_GetYearsConditionPage,
                        "{@offset} {@limit}")
                },
                ErrorLoggingMessages = new List<string>()
            };
            conditionsController.ModelState.AddModelError(ErrorCode, ErrorMessage);
        };

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        Cleanup after = () =>
            conditionsController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)conditionsController.GetYearsConditionPage(offset, limit).Await();

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_decorating_GetYearsConditionPage_method : ConditionsControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(ConditionsController).GetMethod("GetYearsConditionPage");

        It should_have_ValidateConditionLimitAttribute_attribute = () =>
            methodInfo.Should().BeDecoratedWith<ValidateConditionLimitAttribute>();
    }

    internal class When_getting_condition_detail_for_a_year : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetYearDetail,
            "{@year}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetYearDetail(year).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_conditions_for_months_in_a_year : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetMonthsConditionPage,
            "{@year} {@offset} {@limit}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetMonthsConditionPage(year, offset, limit).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_condition_detail_for_a_month_in_a_year : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetMonthDetail,
            "{@year} {@month}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetMonthDetail(year, month).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_conditions_for_days_in_a_month_and_year : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetDaysConditionPage,
            "{@year} {@month} {@offset} {@limit}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetDaysConditionPage(year, month, offset, limit).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_condition_detail_for_a_day_in_a_month_and_year : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetDayDetail,
            "{@year} {@month} {@day}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetDayDetail(year, month, day).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_conditions_for_hours_in_a_day_and_month_and_year : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetHoursConditionPage,
            "{@year} {@month} {@day} {@offset} {@limit}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetHoursConditionPage(year, month, day, offset, limit).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_condition_detail_for_an_hour_in_a_day_and_month_and_year : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetHourDetail,
            "{@year} {@month} {@day} {@hour}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetHourDetail(year, month, day, hour).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_conditions_for_a_month_across_years : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetYearsMonthConditionPage,
            "{@month} {@offset} {@limit}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetYearsMonthConditionPage(month, offset, limit).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_conditions_for_a_day_in_a_month_across_years : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetYearsDayConditionPage,
            "{@month} {@day} {@offset} {@limit}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetYearsDayConditionPage(month, day, offset, limit).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }

    internal class When_getting_conditions_for_an_hour_in_a_day_and_month_across_years : ConditionsControllerSpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
            EventLoggingData = new List<EventLoggingData>
            {
            new EventLoggingData(
            EventId.ConditionsController_GetYearsHourConditionPage,
            "{@month} {@day} {@hour} {@offset} {@limit}")
            },
            ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            exception = Catch.Exception(() => conditionsController.GetYearsHourConditionPage(month, day, hour, offset, limit).Await());

        Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging = () => { };

        It should_throw_expected_exception = () =>
            exception.ShouldBeOfExactType<NotImplementedException>();
    }
}