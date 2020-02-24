using BellRichM.Weather.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BellRichM.Identity.Api.Integration
{
    public class StartupIntegration
    {
        private Startup startup;

        public StartupIntegration(IHostEnvironment env, IConfiguration configuration)
        {
            startup = new Startup(env, configuration);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseMiddleware<InitTestMiddleware>();
            Startup.Configure(app, env);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            startup.ConfigureServices(services);
        }
    }
}
