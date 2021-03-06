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
        /// <param name="paging">The <see cref="PagingModel"/>.</param>
        /// <returns>The navigation links.</returns>
        protected IEnumerable<LinkModel> GetNavigationLinks(string routeName, PagingModel paging)
        {
            var links = new List<LinkModel>();
            if (paging is null)
            {
                return links;
            }

            if (paging.TotalCount > paging.Limit)
            {
                links.Add(
                    new LinkModel
                    {
                        Href = "first",
                        Rel = Url.Link(
                            routeName,
                            new PagingParmModel { Offset = 0, Limit = paging.Limit })
                    });

                var prev = paging.Offset - paging.Limit;
                if (prev >= 0)
                {
                    links.Add(
                        new LinkModel
                        {
                            Href = "prev",
                            Rel = Url.Link(
                                routeName,
                                new PagingParmModel { Offset = prev, Limit = paging.Limit })
                        });
                }

                var next = paging.Offset + paging.Limit;
                if (next < paging.TotalCount)
                {
                    links.Add(
                        new LinkModel
                        {
                            Href = "next",
                            Rel = Url.Link(
                                routeName,
                                new PagingParmModel { Offset = next, Limit = paging.Limit })
                        });
                }

                int last;
                if (paging.TotalCount % paging.Limit == 0)
                {
                    last = paging.TotalCount - paging.Limit;
                }
                else
                {
                    last = (paging.TotalCount / paging.Limit) * paging.Limit;
                }

                links.Add(
                    new LinkModel
                    {
                        Href = "last",
                        Rel = Url.Link(
                            routeName,
                            new PagingParmModel { Offset = last, Limit = paging.Limit })
                    });
            }

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
                ErrorMsg = "Invalid input",
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
            string code = string.Empty;
            string message = string.Empty;
            if (businessException != null)
            {
                code = businessException.Code;
                message = businessException.Message;
                foreach (var errorDetail in businessException.ErrorDetails)
                {
                    errorDetails.Add(
                        new ErrorDetailModel
                        {
                            Code = errorDetail.Code,
                            Text = errorDetail.Text
                        });
                }
            }

            var errrorResponseModel = new ErrorResponseModel
            {
                CorrelationId = HttpContext.TraceIdentifier,
                Code = code,
                ErrorMsg = message,
                ErrorDetails = errorDetails
            };

            return errrorResponseModel;
        }
    }
}