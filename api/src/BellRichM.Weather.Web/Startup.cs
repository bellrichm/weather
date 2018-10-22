using AutoMapper;
using BellRichM.Identity.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BellRichM.Weather.Web
{
    /// <summary>
    /// The startup class.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The <see cref="IHostingEnvironment"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            var identityConnectionString = Configuration.GetSection("ConnectionStrings:(identityDb)");

            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                Log.Information("*** Starting: {@env}", env);
                Log.Information("*** Starting: configurationEnvironment {configurationEnvironment}", Configuration.GetValue<string>("Environment"));
                Log.Information("*** Starting: configurationBasePath {configurationBasePath}", Configuration.GetValue<string>("BasePath"));
                Log.Information("*** Starting: {@identityConnectionString}", identityConnectionString);
            }
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The <see cref="IConfigurationRoot"/>.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Called by the runtime to add services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddIdentityServices(Configuration);

            // needed for testserver to find controllers
            services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("BellRichM.Identity.Api")));
        }

        /// <summary>
        /// Called by the rutntime to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The <see cref="IHostingEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionLoggingMiddleware();

            app.UseDefaultFiles()
               .UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
