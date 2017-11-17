using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;

namespace BellRichM.Identity.Api.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ILogger _logger;
        private readonly RoleManager<Role> _roleManager;
        private readonly IIdentityDbContext _context;

        public RoleRepository(ILogger<RoleRepository> logger, RoleManager<Role> roleManager, IIdentityDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }           

        public async Task<Role> GetById(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                var claimValues = new List<ClaimValue>();
                if (roleClaims.Count > 0)
                {
                    foreach(Claim roleClaim in roleClaims)
                    {
                        claimValues.Add(new ClaimValue
                            {
                                Type = roleClaim.Type,
                                Value = roleClaim.Value,
                                ValueType = roleClaim.ValueType
                            }
                        );                
                    }            
                }
                role.ClaimValues = claimValues;
            }

            return role;
        }

        public async Task<Role> Create(Role role)
        { 
            using (var identitydbContextTransaction = _context.Database.BeginTransaction())
            {
                IdentityResult roleResult = await _roleManager.CreateAsync(role);

                if (!roleResult.Succeeded) 
                {
                    identitydbContextTransaction.Rollback();
                    throw new CreateRoleException(CreateRoleExceptionCode.CreateRoleFailed);
                }
                    
                if (role.ClaimValues != null)
                {
                    foreach (var claimValue in role.ClaimValues)
                    {
                        var claim = new Claim(claimValue.Type, claimValue.Value);
                        var claimResult = await _roleManager.AddClaimAsync(role, claim);
                        if (!claimResult.Succeeded)        
                        {
                            identitydbContextTransaction.Rollback();
                            throw new CreateRoleException(CreateRoleExceptionCode.AddClaimFailed); 
                        }
                    }
                } 
                    
                identitydbContextTransaction.Commit();              
                return await GetById(role.Id); 
            }            
        }
    }
}