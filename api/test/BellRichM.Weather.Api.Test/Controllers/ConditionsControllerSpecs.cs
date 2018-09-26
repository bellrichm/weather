using BellRichM.Weather.Api.Controllers;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Extensions.Logging;
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
      static Mock<ILogger<ConditionsController>> loggerMock;

      Establish context = () =>
      {
        loggerMock = new Mock<ILogger<ConditionsController>>();
        conditionsController = new ConditionsController(loggerMock.Object);
      };

      Cleanup after = () =>
        conditionsController.Dispose();

      Because of = () =>
        exception = Catch.Exception(() => conditionsController.Get());

      It should_throw_expected_exception = () =>
        exception.ShouldBeOfExactType<NotImplementedException>();
    }
  }
}