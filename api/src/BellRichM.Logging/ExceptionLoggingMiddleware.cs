using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace BellRichM.Logging
{
  /// <summary>
  /// The logging and exception handling middleware
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
      /// The logging and exception handling middleware
      /// </summary>
      /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
      /// <returns>The <see cref="Task"/> that represents the completion of request processing.</returns>
    public async Task Invoke(HttpContext httpContext)
    {
      const string MessageTemplate =
        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
      Tuple<string, string> authData = Tuple.Create(string.Empty, string.Empty);
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

          await next(httpContext);

          sw.Stop();
          _logger.LogEvent((int)EventIds.EndRequest, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Response?.StatusCode, sw.Elapsed.TotalMilliseconds);
          _logger.LogDiagnosticDebug("{RequestHeaders}", httpContext.Request.Headers);
        }
      }
      catch (Exception ex)
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
          Errors = new List<ErrorResponseModel.Error>
          {
            new ErrorResponseModel.Error() { Text = "Severe error. Please contact support." }
          }
        };

        string jsonResponse = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        using (LogContext.PushProperty("authData", authorization))
        {
          _logger.LogEvent((int)EventIds.EndRequest, MessageTemplate, httpContext.Request.Method, httpContext.Request.Path, httpContext.Response?.StatusCode, sw.Elapsed.TotalMilliseconds);
          _logger.LogDiagnosticError("Unhandled exception {RequestHeaders}\n {exception}", httpContext.Request.Headers, ex);

          await httpContext.Response.WriteAsync(jsonResponse).ConfigureAwait(false);
        }
      }
    }

    private Tuple<string, string> GetAuthData(HttpContext httpContext)
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
