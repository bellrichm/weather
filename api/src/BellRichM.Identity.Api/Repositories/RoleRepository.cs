using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Repositories
{
    public class RoleRepository : IRoleRepository, IDisposable
    {
        private readonly ILogger _logger;
        private readonly RoleManager<Role> _roleManager;
        private readonly IIdentityDbContext _context;
        private bool disposed = false;

        public RoleRepository(ILogger<RoleRepository> logger, RoleManager<Role> roleManager, IIdentityDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<Role> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                role.ClaimValues = await GetClaimValues(role);
            }

            return role;
        }

        public async Task<Role> GetByName(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);

            if (role != null)
            {
                role.ClaimValues = await GetClaimValues(role);
            }

            return role;
        }

        public async Task<Role> Create(Role role)
        {
            using (var identitydbContextTransaction = _context.BeginTransaction())
            {
                IdentityResult roleResult = await _roleManager.CreateAsync(role);

                if (!roleResult.Succeeded)
                {
                    identitydbContextTransaction.Rollback();

                    // TODO: logging
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

                            // TODO: logging
                            throw new CreateRoleException(CreateRoleExceptionCode.AddClaimFailed);
                        }
                    }
                }

                identitydbContextTransaction.Commit();
                return await GetById(role.Id); // TODO: Move to private method
            }
        }

        public async Task Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            { // TODO: logging
                throw new DeleteRoleException(DeleteRoleExceptionCode.RoleNotFound);
            }

            IdentityResult roleResult = _roleManager.DeleteAsync(role).Result;
            if (!roleResult.Succeeded)
            { // TODO: logging
                throw new DeleteRoleException(DeleteRoleExceptionCode.DeleteRoleFailed);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
         {
            if (!disposed)
            {
                if (disposing)
                {
                    _roleManager.Dispose();
                }

                disposed = true;
            }
        }

        private async Task<List<ClaimValue>> GetClaimValues(Role role)
        {
            var roleClaims = await _roleManager.GetClaimsAsync(role);

            var claimValues = new List<ClaimValue>();
            if (roleClaims.Count > 0)
            {
               foreach (Claim roleClaim in roleClaims)
               {
                    claimValues.Add(new ClaimValue
                     {
                        Type = roleClaim.Type,
                        Value = roleClaim.Value,
                        ValueType = roleClaim.ValueType
                      });
                }
            }

            return claimValues;
       }
    }
}