using AutoMapper;
using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Controllers;
using BellRichM.Weather.Api.Services;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System;
using System.Collections.Generic;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Test.Controllers
{
  internal abstract class ConditionsControllerSpecs
  {
    protected static LoggingData loggingData;
    protected static int year;
    protected static int month;
    protected static int day;
    protected static int hour;
    protected static int offset;
    protected static int limit;

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

      loggerMock = new Mock<ILoggerAdapter<ConditionsController>>();
      mapperMock = new Mock<IMapper>();
      conditionServiceMock = new Mock<IConditionService>();
      conditionsController = new ConditionsController(loggerMock.Object, mapperMock.Object, conditionServiceMock.Object);
    };

    Cleanup after = () =>
      conditionsController.Dispose();
  }

  internal class When_getting_conditions_for_years : ConditionsControllerSpecs
  {
    private static Exception exception;

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
      exception = Catch.Exception(() => conditionsController.GetYearsConditionPage(offset, limit));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

    It should_throw_expected_exception = () =>
      exception.ShouldNotBeNull();
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
      exception = Catch.Exception(() => conditionsController.GetYearDetail(year));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetMonthsConditionPage(year, offset, limit));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetMonthDetail(year, month));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetDaysConditionPage(year, month, offset, limit));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetDayDetail(year, month, day));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetHoursConditionPage(year, month, day, offset, limit));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetHourDetail(year, month, day, hour));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetYearsMonthConditionPage(month, offset, limit));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetYearsDayConditionPage(month, day, offset, limit));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

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
      exception = Catch.Exception(() => conditionsController.GetYearsHourConditionPage(month, day, hour, offset, limit));

#pragma warning disable 169
    Behaves_like<LoggingBehaviors<ConditionsController>> correct_logging;
#pragma warning restore 169

    It should_throw_expected_exception = () =>
      exception.ShouldBeOfExactType<NotImplementedException>();
  }
}