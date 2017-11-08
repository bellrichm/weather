using System;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        }
    }
}
