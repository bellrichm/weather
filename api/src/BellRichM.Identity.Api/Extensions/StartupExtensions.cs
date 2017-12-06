using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BellRichM.Identity.Api.Configuration;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;

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
            // TODO: put the signing key instead of the secret key into the configuration              
                            

            var jwtConfigurationSection = Configuration.GetSection("Identity:JwtConfiguration");    
            new ConfigureFromConfigurationOptions<JwtConfiguration>(jwtConfigurationSection)
                .Configure(jwtConfiguration);
            
            jwtConfiguration.Validate();
            services.AddSingleton<IJwtConfiguration>(jwtConfiguration);
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanViewUsers", policy => policy.RequireClaim(ClaimTypes.Role, "CanViewUsers"));
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
            services.AddScoped<IIdentityDbContext, IdentityDbContext>();                                                                  
        }
    }
}
