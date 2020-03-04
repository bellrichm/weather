using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BellRichM.Attribute.Extensions
{
    /// <summary>
    /// An extension method to recursively validate an instance.
    /// </summary>
    public static class ValidateObjectExtension
    {
        /// <summary>
        /// Recursively validates the object.
        /// </summary>
        /// <param name="instance">The instance to be validated.</param>
        public static void ValidateObject(this object instance)
        {
            var validator = new DataAnnotationsValidator.DataAnnotationsValidator();
            var validationResults = new List<ValidationResult>();

            validator.TryValidateObjectRecursive(instance, validationResults);
            if (validationResults.Count > 0)
            {
                var stringBuilder = new System.Text.StringBuilder();
                foreach (var validationResult in validationResults)
                {
                    stringBuilder.AppendLine(validationResult.ErrorMessage);
                }

                throw new ValidationException(stringBuilder.ToString());
            }
        }
    }
}
