using AutoMapper;
using BellRichM.Logging;
using BellRichM.Weather.Api.Controllers;
using BellRichM.Weather.Api.Services;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Test.Controllers
{
  internal abstract class ConditionsControllerSpecs
  {
    [Subject("Get Conditions")]
    internal class When_getting_current_condition
    {
      static ConditionsController conditionsController;
      static Exception exception;
      static Mock<ILoggerAdapter<ConditionsController>> loggerMock;
      static Mock<IMapper> mapperMock;
      static Mock<IWeatherService> weatherServiceMock;

      Establish context = () =>
      {
        loggerMock = new Mock<ILoggerAdapter<ConditionsController>>();
        mapperMock = new Mock<IMapper>();
        weatherServiceMock = new Mock<IWeatherService>();
        conditionsController = new ConditionsController(loggerMock.Object, mapperMock.Object, weatherServiceMock.Object);
      };

      Cleanup after = () =>
        conditionsController.Dispose();

      Because of = () =>
        exception = Catch.Exception(() => conditionsController.GetYearsConditionPage(0, 5));

      It should_throw_expected_exception = () =>
        exception.ShouldNotBeNull();
    }
  }
}