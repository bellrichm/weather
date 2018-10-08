﻿using AutoMapper;
using BellRichM.Identity.Api.Extensions;
using BellRichM.Weather.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Integration
{
    public class StartupIntegration
    {
        private Startup startup;

        public StartupIntegration(IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            startup = new Startup(env, configuration, loggerFactory);
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            startup.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<InitTestMiddleware>();
            startup.Configure(app, env);
        }
    }
}
