using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Machine.Specifications;
using Newtonsoft.Json;
using Serilog;
using Serilog.Filters;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Extensions;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;
using BellRichM.Weather.Api;

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

            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            UserControllerTests.Client = server.CreateClient();            
        }

        public void OnAssemblyComplete()
        {
            DeleteUser(_adminUser.UserName);   
            DeleteRole(_adminRole.Name); 
            DeleteUser(_testUser.UserName);          
        }

        private void Configure() 
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Filter.ByExcluding(Matching.FromSource‌​("Microsoft"))
                .WriteTo.Console()
                .WriteTo.RollingFile("logs/testLog-{Date}.txt", fileSizeLimitBytes: 10485760, retainedFileCountLimit: 7) // 10 MB file size
                .CreateLogger();      
            var loggerFactory = new LoggerFactory().AddSerilog();
            UserControllerTests.Logger = loggerFactory.CreateLogger<UserControllerTests>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();            
            var configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddSingleton(loggerFactory);
            services.AddLogging();
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
                    Value =  "CanUpdateUsers"
                },
                new ClaimValue
                {
                    Type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    Value =  "CanViewUsers"
                },                
                new ClaimValue
                {
                    Type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    Value =  "CanCreateUsers"
                },                
                new ClaimValue
                {
                    Type = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    Value =  "CanDeleteUsers"
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
            if (role !=null){
                _roleRepository.Delete(role.Id);
            }
        }        
        private User CreateUser(string userName, string pW, IEnumerable<Role> roles)
        {
            var user = new  User 
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