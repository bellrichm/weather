using BellRichM.Attribute.Validation;
using FluentAssertions;
using Machine.Specifications;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using It = Machine.Specifications.It;

namespace BellRichM.Attribute.Test
{
    public class ValidateFilterExpressionAttributeSpecs
    {
        protected const string MessageTemplate = "{0} of {1} must be 'true' or 'false'.";
        protected const string DisplayName = "Bar";
        protected static ValidationContext validationContext;
        protected static ValidateFilterExpressionAttribute validateFilterExpressionAttribute;

        Establish context = () =>
        {
            validationContext = new ValidationContext("Foo")
            {
                DisplayName = DisplayName
            };
            validateFilterExpressionAttribute = new ValidateFilterExpressionAttribute();
        };
    }

    internal class When_value_is_not_valid : ValidateFilterExpressionAttributeSpecs
    {
        protected static ValidationResult validationResult;

        Because of = () =>
            validationResult = validateFilterExpressionAttribute.GetValidationResult("foobar", validationContext);

        It should_not_be_successful = () =>
            validationResult.Should().NotBe(ValidationResult.Success);

        It should_have_correct_message = () =>
        {
            validationResult.ErrorMessage.Should().Be(
                string.Format(
                    CultureInfo.InvariantCulture,
                    MessageTemplate,
                    DisplayName,
                    validationContext.ObjectType.ToString()));
        };
    }

    internal class When_value_is_true : ValidateFilterExpressionAttributeSpecs
    {
        protected static ValidationResult validationResult;

        Because of = () =>
            validationResult = validateFilterExpressionAttribute.GetValidationResult("true", validationContext);

        It should_be_successful = () =>
            validationResult.Should().Be(ValidationResult.Success);
    }

    internal class When_value_is_false : ValidateFilterExpressionAttributeSpecs
    {
        protected static ValidationResult validationResult;

        Because of = () =>
            validationResult = validateFilterExpressionAttribute.GetValidationResult("false", validationContext);

        It should_be_successful = () =>
            validationResult.Should().Be(ValidationResult.Success);
    }
}
