using System;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Extensions
{
    public static class StartupExtensions
    {
         public static void AddIdentityServices(this IServiceCollection services)
        {
            // TODO: Move connection string to config file
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlite("DataSource=Data/Identity.db"));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}