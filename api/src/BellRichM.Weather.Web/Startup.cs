using AutoMapper;
using BellRichM.Identity.Api.Extensions;
using BellRichM.Weather.Api.Extensions;
using BellRichM.Weather.Api.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

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
        /// <param name="env">The <see cref="IHostEnvironment"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public Startup(IHostEnvironment env, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

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
        /// Called by the rutntime to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The <see cref="IHostEnvironment"/>.</param>
        public static void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseExceptionLoggingMiddleware();

            app.UseHttpsRedirection(); // todo what is this?
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../../../app";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        /// <summary>
        /// Called by the runtime to add services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            DbProviderFactories.RegisterFactory("Sqlite", SqliteFactory.Instance);
            DbProviderFactories.RegisterFactory("SqlServer", SqlClientFactory.Instance);
            services.AddIdentityServices(Configuration);
            services.AddWeatherServices(Configuration);

            // needed for testserver to find controllers
            services.AddMvc(
                options => options.OutputFormatters.Add(new ObservationDataOutputFormatter()))
                .AddApplicationPart(Assembly.Load(new AssemblyName("BellRichM.Identity.Api")));

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "app"; // todo fix
            });
        }
    }
}
