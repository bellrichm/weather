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
        /// <summary>
        /// Gets the navigation links.
        /// </summary>
       /// <param name="routeName">The route name.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="total">The total.</param>
        /// <returns>The navigation links.</returns>
        protected IEnumerable<LinkModel> GetNavigationLinks(string routeName, int offset, int limit, int total)
        {
            var links = new List<LinkModel>();

            links.Add(
                new LinkModel
                {
                    Href = "first",
                    Rel = Url.Link(
                        routeName,
                        new { Offset = 0, Limit = limit })
                });

            var prev = offset - limit;
            if (prev >= 0)
            {
                links.Add(
                    new LinkModel
                    {
                        Href = "prev",
                        Rel = Url.Link(
                            routeName,
                            new { Offset = prev, Limit = limit })
                    });
            }

            var next = offset + limit;
            if (next < total)
            {
                links.Add(
                    new LinkModel
                    {
                        Href = "next",
                        Rel = Url.Link(
                            routeName,
                            new { Offset = next, Limit = limit })
                    });
            }

            links.Add(
                new LinkModel
                {
                    Href = "last",
                    Rel = Url.Link(
                        routeName,
                        new { Offset = (total / limit) * limit, Limit = limit })
                });

            return links;
        }

        /// <summary>
        /// Creates a <see cref="ErrorResponseModel"/> from a <see cref="ModelStateDictionary"/>.
        /// </summary>
        /// <returns>The <see cref="ErrorResponseModel"/>.</returns>
        protected ErrorResponseModel CreateModel()
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
                Code = "InvalidInput",
                Text = "Invalid input",
                ErrorDetails = errorDetails
            };

            return errrorResponseModel;
        }

        /// <summary>
        /// Creates a <see cref="ErrorResponseModel"/> from a <see cref="BusinessException"/>.
        /// </summary>
        /// <param name="businessException">The <see cref="BusinessException"/>.</param>
        /// <returns>The <see cref="ErrorResponseModel"/>.</returns>
        protected ErrorResponseModel CreateModel(BusinessException businessException)
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
                Code = businessException.Code,
                Text = businessException.Message,
                ErrorDetails = errorDetails
            };

            return errrorResponseModel;
        }
    }
}