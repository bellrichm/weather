using BellRichM.Logging;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class ExceptionLoggingMiddlewareExtension
    {
        /// <summary>
        /// Add the <see cref="ExceptionLoggingMiddleware"/>to the HTTP request pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseExceptionLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggingMiddleware>();
        }
    }
}