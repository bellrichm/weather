using System;
using System.Collections.Generic;
using System.IO;
using BellRichM.Configuration;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Extensions;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;
using BellRichM.Logging;
using Machine.Specifications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BellRichM.Identity.Api.Integration.Controllers
{
    public class UserControllerSetupAndCleanup : IAssemblyContext
    {
        private IServiceProvider _serviceProvider;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private User _testUser;
        private Role _adminRole;
        private User _adminUser;

        public UserControllerSetupAndCleanup()
        {
            Configure();
            _roleRepository = _serviceProvider.GetService<IRoleRepository>();
            _userRepository = _serviceProvider.GetService<IUserRepository>();
        }

        public void OnAssemblyStart()
        {
            var adminUserPw = "P@ssw0rd";
            var adminUserName = "userAdmin";
            var userAdminRoleName = "userAdminRole";

            var testUserName = "userTest";
            var testUserPw = "P@ssw0rd";

            DeleteUser(adminUserName);
            DeleteRole(userAdminRoleName);
            DeleteUser(testUserName);

            _adminRole = CreateUserAdminRole(userAdminRoleName);
            List<Role> adminRoles = new List<Role>();
            adminRoles.Add(_adminRole);
            _adminUser = CreateUser(adminUserName, adminUserPw, adminRoles);
            UserControllerTests.UserAdminJwt = GenerateJwt(_adminUser.UserName, adminUserPw);

            _testUser = CreateUser(testUserName, testUserPw, null);
            UserControllerTests.TestUser = _testUser;
            UserControllerTests.UserTestJwt = GenerateJwt(_testUser.UserName, testUserPw);

            var configurationManager = new ConfigurationManager("Development", System.AppDomain.CurrentDomain.BaseDirectory);
            var configuration = configurationManager.Create();

            var logManager = new LogManager(configuration);
            var logger = logManager.Create();
            var saveSeriloLogger = Log.Logger;
            Log.Logger = logger;

            var server = new TestServer(
                new WebHostBuilder()
                .UseStartup<StartupIntegration>()
                .ConfigureServices(s => s.AddSingleton(Log.Logger))
                .ConfigureServices(s => s.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>)))
                .UseConfiguration(configuration)
                .UseSerilog());
            UserControllerTests.Client = server.CreateClient();

            Log.Logger = saveSeriloLogger;
        }

        public void OnAssemblyComplete()
        {
            DeleteUser(_adminUser.UserName);
            DeleteRole(_adminRole.Name);
            DeleteUser(_testUser.UserName);
        }

        private void Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory + "../../..")
                .AddJsonFile("appsettings.Setup.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RollingFile("../../../logsTest/testLog-{Date}.txt", fileSizeLimitBytes: 10485760, retainedFileCountLimit: 7) // 10 MB file size
                .CreateLogger();
            Log.Logger = logger;

            var services = new ServiceCollection();
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddIdentityServices(configuration);

            _serviceProvider = services.BuildServiceProvider();
        }

        private Role CreateUserAdminRole(string roleName)
        {
            var claimValues = new List<ClaimValue>()
            {
                new ClaimValue
                {
                    Type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    Value = "CanUpdateUsers"
                },
                new ClaimValue
                {
                    Type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    Value = "CanViewUsers"
                },
                new ClaimValue
                {
                    Type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    Value = "CanCreateUsers"
                },
                new ClaimValue
                {
                    Type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    Value = "CanDeleteUsers"
                }
            };

            var role = new Role
            {
                Name = roleName,
                Description = "Administer the users",
                ClaimValues = claimValues
            };

            var newAdminRole = _roleRepository.Create(role).Result;
            return newAdminRole;
        }

        private void DeleteRole(string roleName)
        {
            var role = _roleRepository.GetByName(roleName).Result;
            if (role != null)
            {
                _roleRepository.Delete(role.Id);
            }
        }

        private User CreateUser(string userName, string pW, IEnumerable<Role> roles)
        {
            var user = new User
            {
                UserName = userName,
                Roles = roles
            };

            var newUser = _userRepository.Create(user, pW).Result;
            return newUser;
        }

        private void DeleteUser(string userName)
        {
            var user = _userRepository.GetByName(userName).Result;
            if (user != null)
            {
                _userRepository.Delete(user.Id);
            }
        }

        private string GenerateJwt(string userName, string userPw)
        {
            var jwtManager = _serviceProvider.GetService<IJwtManager>();
            return jwtManager.GenerateToken(userName, userPw).Result;
        }
    }
}
