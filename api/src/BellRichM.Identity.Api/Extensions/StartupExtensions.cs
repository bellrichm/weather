using System;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BellRichM.Identity.Api.Configuration;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityServices(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("(identityDb)")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            var jwtConfiguration = new JwtConfiguration();
            jwtConfiguration.SecretKey = Configuration["Identity:SecretKey"];                

            var jwtConfigurationSection = Configuration.GetSection("Identity:JwtConfiguration");    
            new ConfigureFromConfigurationOptions<JwtConfiguration>(jwtConfigurationSection)
                .Configure(jwtConfiguration);
            
            jwtConfiguration.Validate();
            services.AddSingleton(jwtConfiguration);
        }
    }
}
