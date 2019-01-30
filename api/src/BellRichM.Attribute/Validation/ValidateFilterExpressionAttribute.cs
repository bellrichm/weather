using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BellRichM.Attribute.Validation
{
    /// <summary>
    /// Validate the expression property. <seealso cref="ValidationAttribute"/>
    /// </summary>
    public class ValidateFilterExpressionAttribute : ValidationAttribute
    {
        /// <inheritdoc/>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string filterExpression = value.ToString();
            if (filterExpression.Equals("true", StringComparison.Ordinal) || filterExpression.Equals("false", StringComparison.Ordinal))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} of {1} must be 'true' or 'false'.",
                    validationContext.DisplayName,
                    validationContext.ObjectType.ToString()));
        }
    }
}