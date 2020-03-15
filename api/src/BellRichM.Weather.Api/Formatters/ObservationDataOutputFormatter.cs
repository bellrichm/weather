using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Formatters
{
    /// <summary>
    /// The observation date output formatter.
    /// </summary>
    public class ObservationDataOutputFormatter : TextOutputFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationDataOutputFormatter"/> class.
        /// </summary>
        public ObservationDataOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("foo/json"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        /// <inheritdoc/>
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (selectedEncoding == null)
            {
                throw new ArgumentNullException(nameof(selectedEncoding));
            }

            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService(typeof(ILogger<ObservationDataOutputFormatter>));

            var response = context.HttpContext.Response;
            var contact = context.Object;

            string jsonResponse = JsonConvert.SerializeObject(contact, Formatting.Indented);
            return response.WriteAsync(jsonResponse);
        }
    }
}