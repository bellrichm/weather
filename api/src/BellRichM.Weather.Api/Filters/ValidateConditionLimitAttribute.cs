using BellRichM.Weather.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Filters
{
      /// <summary>
      /// Validates that the limit query parameter values.
      /// </summary>
    public class ValidateConditionLimitAttribute : TypeFilterAttribute
    {
      /// <summary>
      /// Initializes a new instance of the <see cref="ValidateConditionLimitAttribute"/> class.
      /// </summary>
      public ValidateConditionLimitAttribute()
        : base(typeof(ValidateConditionLimitAttributeImplementation))
      {
      }

#pragma warning disable S1144, S3376
      private class ValidateConditionLimitAttributeImplementation : ActionFilterAttribute
      {
          private readonly string limitMissingMessage = "The query paramenter limit is required.";
          private readonly string limitMinMessage = "The limit query paramenter must be greater than 0.";
          private readonly string limitMaxMessage = "The limit query parameter must be less than or equal to {0}.";

          private readonly string limitMissingCode = "LimitMissing";
          private readonly string limitMinCode = "LimitMin";
          private readonly string limitMaxCode = "LimitMax";

          private readonly IConditionRepositoryConfiguration _conditonRepositoryConfiguration;

          /// <summary>
          /// Initializes a new instance of the <see cref="ValidateConditionLimitAttributeImplementation"/> class.
          /// </summary>
          /// <param name="conditionRepositoryConfiguration">The <see cref="IConditionRepositoryConfiguration"/>.</param>
          public ValidateConditionLimitAttributeImplementation(IConditionRepositoryConfiguration conditionRepositoryConfiguration)
          {
            _conditonRepositoryConfiguration = conditionRepositoryConfiguration;
          }

          /// <inheritdoc/>
          public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
          {
            int? limit = null;
            if (context.ActionArguments.ContainsKey("limit"))
            {
              limit = context.ActionArguments["limit"] as int?;
            }

            if (limit == null)
            {
              context.ModelState.AddModelError(limitMissingCode, limitMissingMessage);
            }

            if (limit <= 0)
            {
              context.ModelState.AddModelError(limitMinCode, limitMinMessage);
            }

            if (limit > _conditonRepositoryConfiguration.MaximumConditions)
            {
              context.ModelState.AddModelError(limitMaxCode, string.Format(CultureInfo.InvariantCulture, limitMaxMessage, _conditonRepositoryConfiguration.MaximumConditions));
            }

            await base.OnActionExecutionAsync(context, next).ConfigureAwait(true);
          }
      }
#pragma warning restore S1144, S3376      
    }
}