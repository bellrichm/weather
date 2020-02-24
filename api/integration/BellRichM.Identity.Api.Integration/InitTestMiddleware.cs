using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BellRichM.Identity.Api.Integration
{
    public class InitTestMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IPAddress fakeIpAddress = IPAddress.Parse("127.0.0.1");

        public InitTestMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Connection.RemoteIpAddress = fakeIpAddress;

            await this.next(httpContext).ConfigureAwait(true);
        }
    }
}