using AutoMapper;
using BellRichM.Identity.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        public Startup(IHostingEnvironment env)
            : this(env, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The <see cref="IHostingEnvironment"/>.</param>
        /// <param name="dir">The content root path.</param>
        public Startup(IHostingEnvironment env, string dir)
        {
            if (dir == null)
            {
                dir = env.ContentRootPath;
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The <see cref="IConfigurationRoot"/>.
        /// </value>
        public IConfigurationRoot Configuration { get; }

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
            app.UseExceptionHandlerMiddleware(); // TODO: Rename

            app.UseDefaultFiles()
               .UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
