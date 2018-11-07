using BellRichM.Api.Models;
using BellRichM.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace BellRichM.Api.Controllers
{
    /// <summary>
    /// The base api controller.
    /// </summary>
    public abstract class ApiController : Controller
    {
        private ErrorResponseModel CreateModel(ModelStateDictionary modelStateDictionary)
        {
            var errorDetails = new List<ErrorDetailModel>();
            foreach (KeyValuePair<string, ModelStateEntry> modelStateEntry in ModelState)
            {
                var modelErrors = modelStateEntry.Value.Errors;
                foreach (var modelError in modelErrors)
                {
                    errorDetails.Add(
                        new ErrorDetailModel
                        {
                            Code = modelStateEntry.Key + "Invalid",
                            Text = modelError.ErrorMessage
                        });
                }
            }

            var errrorResponseModel = new ErrorResponseModel
            {
                CorrelationId = HttpContext.TraceIdentifier,
                /* Code = string.Empty, */
                Text = string.Empty,
                ErrorDetails = errorDetails
            };

            return errrorResponseModel;
        }

        private ErrorResponseModel CreateModel(BusinessException businessException)
        {
            var errorDetails = new List<ErrorDetailModel>();
            foreach (var errorDetail in businessException.ErrorDetails)
            {
                errorDetails.Add(
                    new ErrorDetailModel
                    {
                        Code = errorDetail.Code,
                        Text = errorDetail.Text
                    });
            }

            var errrorResponseModel = new ErrorResponseModel
            {
                CorrelationId = HttpContext.TraceIdentifier,
                /* Code = string.Empty, */
                Text = string.Empty,
                ErrorDetails = errorDetails
            };

            return errrorResponseModel;
        }
    }
}