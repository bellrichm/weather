using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ILogger _logger;
        private readonly RoleManager<Role> _roleManager;

        public RoleRepository(ILogger<RoleRepository> logger, RoleManager<Role> roleManager)
        {
            _logger = logger;
            _roleManager = roleManager;
        }           

        public async Task<Role> GetById(string Id)
        {
            throw new NotImplementedException();
        }        
    }
}