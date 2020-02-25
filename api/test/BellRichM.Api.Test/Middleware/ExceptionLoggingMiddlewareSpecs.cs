using BellRichM.Api.Models;
using BellRichM.Helpers.Test;
using BellRichM.Logging;
using Machine.Specifications;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using It = Machine.Specifications.It;

namespace BellRichM.Api.Middleware.Test
{
    internal class ExceptionLoggingMiddlewareSpecs
    {
        protected static LoggingData loggingData;

        protected static ExceptionLoggingMiddleware exceptionLoggingMiddleware;
        protected static Mock<ILoggerAdapter<ExceptionLoggingMiddleware>> loggerMock;
        protected static DefaultHttpContext httpContext;
        protected static RequestDelegate requestDelegate;

        protected static string errorText = "Severe error. Please contact support.";
        protected static string errorCode = "SevereError";

        protected static string notImplementedText = "Call is not implemented";
        protected static string notImplementedCode = "NotImplemented";

        Establish context = () =>
        {
            loggerMock = new Mock<ILoggerAdapter<ExceptionLoggingMiddleware>>();

            httpContext = new DefaultHttpContext();
            httpContext.Request.Protocol = "HTTP/1.1";
            httpContext.Request.Host = new HostString("localhost", 5000);
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            httpContext.TraceIdentifier = "foobar";
            httpContext.Request.Method = "Get";
            httpContext.Request.Path = "/foo/bar";
            httpContext.Request.Headers["Authorization"] = "foo bar";
            httpContext.Response.Body = new MemoryStream();
        };
    }

    internal class When_exception_is_caught : ExceptionLoggingMiddlewareSpecs
    {
        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.EndRequest,
                        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms")
                },
                ErrorLoggingMessages = new List<string>
                {
                    "Unhandled exception {RequestHeaders}\n {exception}"
                }
            };

            Task ExceptionRequestDelegate(HttpContext innerHttpContext)
            {
              throw new Exception();
            }

            requestDelegate = ExceptionRequestDelegate;
            exceptionLoggingMiddleware = new ExceptionLoggingMiddleware(requestDelegate, loggerMock.Object);
        };

        Because of = () =>
          exceptionLoggingMiddleware.Invoke(httpContext).Await();

 #pragma warning disable 169
        Behaves_like<LoggingBehaviors<ExceptionLoggingMiddleware>> correct_logging;
 #pragma warning restore 169

        It should_add_identifer_header = () =>
            httpContext.Response.Headers["X-Request-Id"].ToString().ShouldEqual(httpContext.TraceIdentifier);

        It should_return_error_body = () =>
        {
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContext.Response.Body);
            var responseString = reader.ReadToEnd();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseModel>(responseString);

            errorResponse.CorrelationId.ShouldEqual(httpContext.TraceIdentifier);
            errorResponse.ErrorDetails.ShouldBeNull();
            errorResponse.Code.ShouldEqual(errorCode);
            errorResponse.ErrorMsg.ShouldEqual(errorText);
        };
    }

    internal class When_NotImplementedException_is_caught : ExceptionLoggingMiddlewareSpecs
    {
        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                DebugTimes = 1,
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.EndRequest,
                        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms")
                },
                ErrorLoggingMessages = new List<string>()
            };

            Task ExceptionRequestDelegate(HttpContext innerHttpContext)
            {
              throw new NotImplementedException();
            }

            requestDelegate = ExceptionRequestDelegate;
            exceptionLoggingMiddleware = new ExceptionLoggingMiddleware(requestDelegate, loggerMock.Object);
        };

        Because of = () =>
          exceptionLoggingMiddleware.Invoke(httpContext).Await();

 #pragma warning disable 169
        Behaves_like<LoggingBehaviors<ExceptionLoggingMiddleware>> correct_logging;
 #pragma warning restore 169

        It should_add_identifer_header = () =>
            httpContext.Response.Headers["X-Request-Id"].ToString().ShouldEqual(httpContext.TraceIdentifier);

        It should_return_error_body = () =>
        {
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(httpContext.Response.Body);
            var responseString = reader.ReadToEnd();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponseModel>(responseString);

            errorResponse.CorrelationId.ShouldEqual(httpContext.TraceIdentifier);
            errorResponse.ErrorDetails.ShouldBeNull();
            errorResponse.Code.ShouldEqual(notImplementedCode);
            errorResponse.ErrorMsg.ShouldEqual(notImplementedText);
        };
    }

    internal class When_request_processing_is_successful : ExceptionLoggingMiddlewareSpecs
    {
        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                DebugTimes = 1,
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                    EventId.EndRequest,
                    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms")
                },
                ErrorLoggingMessages = new List<string>()
            };

            Task SuccessRequestDelegate(HttpContext innerHttpContext)
            {
              return Task.FromResult(0);
            }

            requestDelegate = new RequestDelegate(SuccessRequestDelegate);
            exceptionLoggingMiddleware = new ExceptionLoggingMiddleware(requestDelegate, loggerMock.Object);
        };

        Because of = () =>
          exceptionLoggingMiddleware.Invoke(httpContext).Await();

 #pragma warning disable 169
        Behaves_like<LoggingBehaviors<ExceptionLoggingMiddleware>> correct_logging;
 #pragma warning restore 169

        It should_add_identifer_header = () =>
            httpContext.Response.Headers["X-Request-Id"].ToString().ShouldEqual(httpContext.TraceIdentifier);
    }
}