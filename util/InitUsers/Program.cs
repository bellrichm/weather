using System;
using System.Collections.Generic;
using System.IO;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Extensions;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Serilog.Filters;

namespace InitUsers
{
    class Program
    {
        static void Main()
        {
            var rolesInit = "roles.json";
            var usersInit = "users.json";

            var serviceProvider = Configure();
            var logger = serviceProvider.GetService<ILoggerAdapter<Program>>();
            var roleRepository = serviceProvider.GetService<IRoleRepository>();
            var userRepository = serviceProvider.GetService<IUserRepository>();

            var roles = new List<Role>();
            using (StreamReader streamReader = new StreamReader(rolesInit))
            {
                roles = JsonConvert.DeserializeObject<List<Role>>(streamReader.ReadToEnd());
            }

            var users = new List<UserPassword>();
            using (StreamReader streamReader = new StreamReader(usersInit))
            {
                users = JsonConvert.DeserializeObject<List<UserPassword>>(streamReader.ReadToEnd());
            }

            foreach (var role in roles)
            {
                try
                {
                    roleRepository.Create(role).Wait();
                }
                catch (CreateRoleException ex)
                {
                    logger.LogDiagnosticInformation("{@ex}", ex);
                }
                catch (Exception ex)
                {
                    logger.LogDiagnosticInformation("{@ex}", ex);
                    throw ex;
                }
            }

            foreach (var user in users)
            {
                try
                {
                    userRepository.Create(user.User, user.Password).Wait();
                }
                catch (CreateUserException ex)
                {
                    logger.LogDiagnosticInformation("{@ex}", ex);
                }
                catch (Exception ex)
                {
                    logger.LogDiagnosticInformation("{@ex}", ex);
                    throw ex;
                }                
            }
        }

        private static IServiceProvider Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory + "../../..")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var logManager = new LogManager(configuration);
            Log.Logger = logManager.Create();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddIdentityServices(configuration);
            return services.BuildServiceProvider();
        }

        private class UserPassword
        {
            public string Password { get; set; }

            public User User { get; set; }
        }
    }
}
