using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Filters;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Test.Filters
{
    public class ValidateConditionLimitAttributeSpecs
    {
        protected const int ConfiguredLimit = 3;
        protected static MethodInfo methodInfo;
        protected static object validateConditionLimitAttributeImplementationTypeInstance;
        protected static ActionContext actionContext;
        protected static ActionExecutionDelegate actionExecutionDelegate;

        protected static LoggingData loggingData;

        protected static Mock<ILoggerAdapter<ValidateConditionLimitAttributeSpecs>> loggerMock;

        protected static ValidateConditionLimitAttributeSpecs validateConditionLimitAttribute;

        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            loggerMock = new Mock<ILoggerAdapter<ValidateConditionLimitAttributeSpecs>>();

            actionContext = new ActionContext
                {
                    HttpContext = Mock.Of<HttpContext>(),
                    RouteData = Mock.Of<RouteData>(),
                    ActionDescriptor = new ActionDescriptor(),
                };

            var config = new ConditionRepositoryConfiguration
            {
                MaximumConditions = ConfiguredLimit
            };

            Task<ActionExecutedContext> ActionExecute()
            {
              return Task.FromResult<ActionExecutedContext>(null);
            }

            actionExecutionDelegate = ActionExecute;

            validateConditionLimitAttribute = new ValidateConditionLimitAttributeSpecs();
            var validateConditionLimitAttributeType = typeof(ValidateConditionLimitAttribute);
            var validateConditionLimitAttributeImplementationType = validateConditionLimitAttributeType.GetNestedType("ValidateConditionLimitAttributeImplementation", BindingFlags.NonPublic);

            validateConditionLimitAttributeImplementationTypeInstance = Activator.CreateInstance(validateConditionLimitAttributeImplementationType, config);
            methodInfo = validateConditionLimitAttributeImplementationType.GetMethod("OnActionExecutionAsync");
        };
    }

    internal class When_limit_is_valid : ValidateConditionLimitAttributeSpecs
    {
        protected const int LimitQueryParameter = 2;
        protected static object[] parametersArray;
        protected static ActionExecutingContext actionExecutingContext;

        Establish context = () =>
        {
            actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>()
                {
                    { "limit", LimitQueryParameter }
                },
                Mock.Of<Controller>());

            parametersArray = new object[] { actionExecutingContext, actionExecutionDelegate };
        };

        Because of = () =>
            methodInfo.Invoke(validateConditionLimitAttributeImplementationTypeInstance, parametersArray);

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ValidateConditionLimitAttributeSpecs>> correct_logging;
#pragma warning restore 169

        It should_have_one_ModelStateDictionary_entry = () =>
            actionExecutingContext.ModelState.Count.Should().Equals(0);
    }

    internal class When_limit_is_missing : ValidateConditionLimitAttributeSpecs
    {
        protected const string ErrorMessage = "The query paramenter limit is required.";
        protected const string ErrorCode = "LimitMin";
        protected static object[] parametersArray;
        protected static ActionExecutingContext actionExecutingContext;

        Establish context = () =>
        {
            actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>());

            parametersArray = new object[] { actionExecutingContext, actionExecutionDelegate };
        };

        Because of = () =>
            methodInfo.Invoke(validateConditionLimitAttributeImplementationTypeInstance, parametersArray);

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ValidateConditionLimitAttributeSpecs>> correct_logging;
#pragma warning restore 169

        It should_have_one_ModelStateDictionary_entry = () =>
            actionExecutingContext.ModelState.Count.Should().Equals(1);

        It should_have_correct_key = () =>
            actionContext.ModelState.First().Key.Should().Equals(ErrorCode);

        It should_have_one_ModelError = () =>
        {
            var modelErrors = actionContext.ModelState.First().Value.Errors;
            modelErrors.Count.Should().Equals(1);
        };

        It should_have_correct_message = () =>
        {
            var errorMessage = actionContext.ModelState.First().Value.Errors[0].ErrorMessage;
            errorMessage.Should().BeEquivalentTo(ErrorMessage);
        };
    }

    internal class When_limit_is_smaller_than_minimum_allowed : ValidateConditionLimitAttributeSpecs
    {
        protected const int LimitQueryParameter = 0;
        protected const string ErrorMessage = "The limit query paramenter must be greater than 0.";
        protected const string ErrorCode = "LimitMin";
        protected static object[] parametersArray;
        protected static ActionExecutingContext actionExecutingContext;

        Establish context = () =>
        {
            actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>()
                {
                    { "limit", LimitQueryParameter }
                },
                Mock.Of<Controller>());

            parametersArray = new object[] { actionExecutingContext, actionExecutionDelegate };
        };

        Because of = () =>
            methodInfo.Invoke(validateConditionLimitAttributeImplementationTypeInstance, parametersArray);

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ValidateConditionLimitAttributeSpecs>> correct_logging;
#pragma warning restore 169

        It should_have_one_ModelStateDictionary_entry = () =>
            actionExecutingContext.ModelState.Count.Should().Equals(1);

        It should_have_correct_key = () =>
            actionContext.ModelState.First().Key.Should().Equals(ErrorCode);

        It should_have_one_ModelError = () =>
        {
            var modelErrors = actionContext.ModelState.First().Value.Errors;
            modelErrors.Count.Should().Equals(1);
        };

        It should_have_correct_message = () =>
        {
            var errorMessage = actionContext.ModelState.First().Value.Errors[0].ErrorMessage;
            errorMessage.Should().BeEquivalentTo(ErrorMessage);
        };
    }

    internal class When_limit_is_bigger_than_maximimum_allowed : ValidateConditionLimitAttributeSpecs
    {
        protected const int LimitQueryParameter = 5;
        protected const string ErrorMessage = "The limit query parameter must be less than or equal to {0}.";
        protected const string ErrorCode = "LimitMax";
        protected static object[] parametersArray;
        protected static ActionExecutingContext actionExecutingContext;

        Establish context = () =>
        {
            actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>()
                {
                    { "limit", LimitQueryParameter }
                },
                Mock.Of<Controller>());

            parametersArray = new object[] { actionExecutingContext, actionExecutionDelegate };
        };

        Because of = () =>
            methodInfo.Invoke(validateConditionLimitAttributeImplementationTypeInstance, parametersArray);

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<ValidateConditionLimitAttributeSpecs>> correct_logging;
#pragma warning restore 169

        It should_have_one_ModelStateDictionary_entry = () =>
            actionExecutingContext.ModelState.Count.Should().Equals(1);

        It should_have_correct_key = () =>
            actionContext.ModelState.First().Key.Should().Equals(ErrorCode);

        It should_have_one_ModelError = () =>
        {
            var modelErrors = actionContext.ModelState.First().Value.Errors;
            modelErrors.Count.Should().Equals(1);
        };

        It should_have_correct_message = () =>
        {
            var errorMessage = actionContext.ModelState.First().Value.Errors[0].ErrorMessage;
            errorMessage.Should().BeEquivalentTo(string.Format(CultureInfo.InvariantCulture, ErrorMessage, ConfiguredLimit));
        };
    }
}
