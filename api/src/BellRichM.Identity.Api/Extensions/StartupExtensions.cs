using System;
using System.Security.Claims;
using System.Text;
using BellRichM.Attribute.Extensions;
using BellRichM.Identity.Api.Configuration;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;

namespace BellRichM.Identity.Api.Extensions
{
    /// <summary>
    /// Startup extenstion methods.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Adds the services needed for Identity API.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfigurationRoot"/>.</param>
        public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("(identityDb)")));

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            var jwtConfiguration = new JwtConfiguration();

            jwtConfiguration.SecretKey = configuration["Identity:SecretKey"];

            var jwtConfigurationSection = configuration.GetSection("Identity:JwtConfiguration");
            new ConfigureFromConfigurationOptions<JwtConfiguration>(jwtConfigurationSection)
                .Configure(jwtConfiguration);

            using (LogContext.PushProperty("Type", "INFORMATION"))
            {
                Log.Information("*** Starting: {@jwtConfiguration}", jwtConfiguration);
            }

            jwtConfiguration.ValidateObject();
            services.AddSingleton<IJwtConfiguration>(jwtConfiguration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanViewUsers", policy => policy.RequireClaim(ClaimTypes.Role, "CanViewUsers"));
                options.AddPolicy("CanCreateUsers", policy => policy.RequireClaim(ClaimTypes.Role, "CanCreateUsers"));
                options.AddPolicy("CanDeleteUsers", policy => policy.RequireClaim(ClaimTypes.Role, "CanDeleteUsers"));
                options.AddPolicy("CanViewRoles", policy => policy.RequireClaim(ClaimTypes.Role, "CanViewRoles"));
                options.AddPolicy("CanCreateRoles", policy => policy.RequireClaim(ClaimTypes.Role, "CanCreateRoles"));
                options.AddPolicy("CanDeleteRoles", policy => policy.RequireClaim(ClaimTypes.Role, "CanDeleteRoles"));
                options.AddPolicy("CanViewLoggingLevels", policy => policy.RequireClaim(ClaimTypes.Role, "CanViewLoggingLevels"));
                options.AddPolicy("CanUpdateLoggingLevels", policy => policy.RequireClaim(ClaimTypes.Role, "CanUpdateLoggingLevels"));
                options.AddPolicy("CanViewLoggingFilters", policy => policy.RequireClaim(ClaimTypes.Role, "CanViewLoggingLevels"));
                options.AddPolicy("CanUpdateLoggingFilters", policy => policy.RequireClaim(ClaimTypes.Role, "CanUpdateLoggingLevels"));
                options.AddPolicy("CanViewObservations", policy => policy.RequireClaim(ClaimTypes.Role, "CanViewObservations"));
                options.AddPolicy("CanCreateObservations", policy => policy.RequireClaim(ClaimTypes.Role, "CanCreateObservations"));
                options.AddPolicy("CanUpdateObservations", policy => policy.RequireClaim(ClaimTypes.Role, "CanUpdateObservations"));
                options.AddPolicy("CanDeleteObservations", policy => policy.RequireClaim(ClaimTypes.Role, "CanDeleteObservations"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfiguration.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IJwtManager, JwtManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}