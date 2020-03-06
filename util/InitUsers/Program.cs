using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Extensions;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;

namespace InitUsers
{
    class Program
    {
        static ILoggerAdapter<Program> logger;
        static IRoleRepository roleRepository;
        static IUserRepository userRepository;

        /// <summary>
        /// Initialize the identity db.static.static.
        /// </summary>
        /// <param name="rolesInit">A json file containing the roles to add.</param>
        /// <param name="usersInit">A json file containing the users to add.</param>
        static void Main(string rolesInit = "roles.json", string usersInit = "users.json")
        {
            var serviceProvider = Configure();
            logger = serviceProvider.GetService<ILoggerAdapter<Program>>();
            roleRepository = serviceProvider.GetService<IRoleRepository>();
            userRepository = serviceProvider.GetService<IUserRepository>();

            var roles = new List<Role>();
            using (StreamReader streamReader = new StreamReader(rolesInit))
            {
                roles = JsonConvert.DeserializeObject<List<Role>>(streamReader.ReadToEnd());
            }

            CreateRoles(roles).Wait();

            var users = new List<UserPassword>();
            using (StreamReader streamReader = new StreamReader(usersInit))
            {
                users = JsonConvert.DeserializeObject<List<UserPassword>>(streamReader.ReadToEnd());
            }

            CreateUsers(users).Wait();
        }

        private static async Task CreateRoles(List<Role> roles)
        {
            foreach (var role in roles)
            {
                try
                {
                    await roleRepository.Create(role).ConfigureAwait(true);
                    logger.LogDiagnosticInformation("Created {@role}", role);
                }
                catch (CreateRoleException ex)
                {
                    logger.LogDiagnosticError("Error creating {@role} {@ex}", role, ex);
                }
                catch (Exception ex)
                {
                    logger.LogDiagnosticError("Error creating {@role} {@ex}", role, ex);
                    throw;
                }
            }
        }

        private static async Task CreateUsers(List<UserPassword> users)
        {
            foreach (var user in users)
            {
                try
                {
                    await userRepository.Create(user.User, user.Password).ConfigureAwait(true);
                    logger.LogDiagnosticInformation("Created {@user}", user);
                }
                catch (CreateUserException ex)
                {
                    logger.LogDiagnosticError("Error creating {@user} {@ex}", user, ex);
                }
                catch (Exception ex)
                {
                    logger.LogDiagnosticError("Error creating {@user} {@ex}", user, ex);
                    throw;
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

        #pragma warning disable CA1812 // Used in a list
        private class UserPassword
        {
            public string Password { get; set; }

            public User User { get; set; }
        }
        #pragma warning restore CA1812
    }
}
