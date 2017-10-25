using FluentAssertions;
using Machine.Specifications;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

using Microsoft.Extensions.Logging;
using System;

using BellRichM.Weather.Api.Controllers;

namespace BellRichM.Weather.Api.Test.Controllers
{
	internal abstract class ConditionsControllerSpecs 
	{
		[Subject("Get Conditions")]
		internal class when_getting_current_condition
		{
			static ConditionsController conditionsController;
    		static Exception exception;
    		protected static Mock<ILogger<ConditionsController>> loggerMock;

			Establish context = () => 
			{
				loggerMock = new Mock<ILogger<ConditionsController>>();
				conditionsController = new ConditionsController(loggerMock.Object);
			};
		
    		Because of = () => 
				exception = Catch.Exception(() => conditionsController.Get());

    		It should_throw_expected_exception = () =>    	
				exception.ShouldBeOfExactType<NotImplementedException>();	
		}

		[Subject("Post Conditions")]
		internal class when_adding_conditions
		{
			It should_do_something_cool;
		}
	}
}