namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class ExceptionHandlerMiddleWareExtensions
    {
        /// <summary>
        /// Add the <see cref="ExceptionHandlerMiddleware"/>to the HTTP request pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>A <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}