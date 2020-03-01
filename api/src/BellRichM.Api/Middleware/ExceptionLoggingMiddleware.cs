using BellRichM.Api.Models;
using BellRichM.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace BellRichM.Api.Middleware
{
  /// <summary>
  /// The logging and exception handling middleware.
  /// </summary>
  public class ExceptionLoggingMiddleware
  {
    private readonly RequestDelegate next;
    private readonly ILoggerAdapter<ExceptionLoggingMiddleware> _logger;

      /// <summary>
      /// Initializes a new instance of the <see cref="ExceptionLoggingMiddleware"/> class.
      /// </summary>
      /// <param name="next">The next <see cref="RequestDelegate"/>in the middleware pipeline.</param>
      /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
    public ExceptionLoggingMiddleware(RequestDelegate next, ILoggerAdapter<ExceptionLoggingMiddleware> logger)
    {
      this.next = next;
      _logger = logger;
    }

      /// <summary>
      /// The logging and exception handling middleware.
      /// </summary>
      /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
      /// <returns>The <see cref="Task"/> that represents the completion of request processing.</returns>
    public async Task Invoke(HttpContext httpContext)
    {
      if (httpContext == null)
      {
        throw new ArgumentNullException(nameof(httpContext));
      }

      const string MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
      #pragma warning disable S1854 // TODO: Investigate, seems to be a false positive
      Tuple<string, string> authData = Tuple.Create(string.Empty, string.Empty);
      #pragma warning restore S1854
      var sw = Stopwatch.StartNew();

      try
      {
        httpContext.Response.Headers.Add("X-Request-Id", httpContext.TraceIdentifier);

        authData = GetAuthData(httpContext);
        var authorization = new
        {
          authTokenType = authData.Item1,
          authTokenData = authData.Item2
        };

        using (LogContext.PushProperty("authData", authorization))
        {
          _logger.LogDiagnosticInformation("Request Started {RequestProtocol} {RequestHost} {ClientIp}", httpContext.Request.Protocol, httpContext.Request.Host, httpContext.Connection.RemoteIpAddress.ToString());

          await next(httpContext).ConfigureAwait(true);

          sw.Stop();
          _logger.LogEvent((int)EventId.EndRequest, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Response?.StatusCode, sw.Elapsed.TotalMilliseconds);
          _logger.LogDiagnosticDebug("{RequestHeaders}", httpContext.Request.Headers);
        }
      }
      catch (NotImplementedException)
      {
          sw.Stop();

          var authorization = new
          {
            authTokenType = authData.Item1,
            authTokenData = authData.Item2
          };

          var errorResponse = new ErrorResponseModel
          {
            CorrelationId = httpContext.TraceIdentifier,
            ErrorMsg = "Call is not implemented",
            Code = "NotImplemented"
          };

          string jsonResponse = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);

          httpContext.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
          httpContext.Response.ContentType = "application/json";

          using (LogContext.PushProperty("authData", authorization))
          {
            _logger.LogEvent((int)EventId.EndRequest, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Response?.StatusCode, sw.Elapsed.TotalMilliseconds);
            _logger.LogDiagnosticDebug("{RequestHeaders}", httpContext.Request.Headers);
          }

          await httpContext.Response.WriteAsync(jsonResponse).ConfigureAwait(false);
      }
      #pragma warning disable CA1031 // ToDo - investigate
      catch (Exception ex)
      #pragma warning restore CA1031
      {
        sw.Stop();

        var authorization = new
        {
          authTokenType = authData.Item1,
          authTokenData = authData.Item2
        };

        var errorResponse = new ErrorResponseModel
        {
          CorrelationId = httpContext.TraceIdentifier,
          ErrorMsg = "Severe error. Please contact support.",
          Code = "SevereError"
        };

        string jsonResponse = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        using (LogContext.PushProperty("authData", authorization))
        {
          _logger.LogEvent((int)EventId.EndRequest, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Response?.StatusCode, sw.Elapsed.TotalMilliseconds);
          _logger.LogDiagnosticError("Unhandled exception {RequestHeaders}\n {exception}", httpContext.Request.Headers, ex);

          await httpContext.Response.WriteAsync(jsonResponse).ConfigureAwait(false);
        }
      }
    }

    private static Tuple<string, string> GetAuthData(HttpContext httpContext)
    {
      string authTokenType = string.Empty;
      string authTokenData = string.Empty;
      string authHeader = httpContext.Request.Headers["Authorization"];
      if (!string.IsNullOrWhiteSpace(authHeader))
      {
        var array = authHeader.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        authTokenType = array[0];
        if (array.Length > 1)
        {
          authTokenData = array[1];
        }
      }

      return Tuple.Create(authTokenType, authTokenData);
    }
  }
}
