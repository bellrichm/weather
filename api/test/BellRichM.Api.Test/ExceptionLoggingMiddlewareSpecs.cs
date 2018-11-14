using BellRichM.Api.Middleware;
using BellRichM.Api.Models;
using BellRichM.Logging;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Api.Middleware.Test
{
    internal class ExceptionLoggingMiddlewareSpecs
    {
        protected static int debugTimes;
        protected static int informationTimes;
        protected static int errorTimes;
        protected static int eventTimes;

        protected static ExceptionLoggingMiddleware exceptionLoggingMiddleware;
        protected static Mock<ILoggerAdapter<ExceptionLoggingMiddleware>> loggerMock;
        protected static DefaultHttpContext httpContext;
        protected static RequestDelegate requestDelegate;

        protected static string errorText = "Severe error. Please contact support.";
        protected static string errorCode = "SevereError";

        Establish context = () =>
        {
            debugTimes = 0;
            informationTimes = 0;
            errorTimes = 0;
            eventTimes = 0;

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
            informationTimes = 1;
            errorTimes = 1;
            eventTimes = 1;

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

        It should_log_correct_information_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticInformation(IT.IsAny<string>(), httpContext.Request.Protocol, httpContext.Request.Host, httpContext.Connection.RemoteIpAddress.ToString()), Times.Exactly(1));

        It should_log_correct_error_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticError(IT.IsAny<string>(), IT.IsAny<IHeaderDictionary>(), IT.IsAny<Exception>()), Times.Once);

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent((int)EventId.EndRequest, IT.IsAny<string>(), httpContext.Request.Method, httpContext.Request.Path, 500, IT.IsAny<double>()), Times.Once);

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
            errorResponse.Text.ShouldEqual(errorText);
        };
    }

    internal class When_request_processing_is_successful : ExceptionLoggingMiddlewareSpecs
    {
        Establish context = () =>
        {
            debugTimes = 1;
            informationTimes = 1;
            eventTimes = 1;

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

        It should_log_correct_debug_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticDebug(IT.IsAny<string>(), IT.IsAny<IHeaderDictionary>()), Times.Once);

        It should_log_correct_information_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticInformation(IT.IsAny<string>(), httpContext.Request.Protocol, httpContext.Request.Host, httpContext.Connection.RemoteIpAddress.ToString()), Times.Exactly(1));

        It should_log_correct_events = () =>
            loggerMock.Verify(x => x.LogEvent((int)EventId.EndRequest, IT.IsAny<string>(), httpContext.Request.Method, httpContext.Request.Path, IT.IsAny<int>(), IT.IsAny<double>()), Times.Once);

        It should_add_identifer_header = () =>
            httpContext.Response.Headers["X-Request-Id"].ToString().ShouldEqual(httpContext.TraceIdentifier);
    }
}