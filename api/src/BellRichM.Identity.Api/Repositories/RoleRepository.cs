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
    /// <inheritdoc/>
    public class RoleRepository : IRoleRepository, IDisposable
    {
        private readonly ILogger _logger;
        private readonly RoleManager<Role> _roleManager;
        private readonly IIdentityDbContext _context;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{RoleRepository}"/>.</param>
        /// <param name="roleManager">The <see cref="RoleManager{Role}"/>.</param>
        /// <param name="context">The <see cref="IIdentityDbContext"/>.</param>
        public RoleRepository(ILogger<RoleRepository> logger, RoleManager<Role> roleManager, IIdentityDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Role> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                role.ClaimValues = await GetClaimValues(role);
            }

            return role;
        }

        /// <inheritdoc/>
        public async Task<Role> GetByName(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);

            if (role != null)
            {
                role.ClaimValues = await GetClaimValues(role);
            }

            return role;
        }

        /// <summary>
        /// Creates the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The <see cref="Role"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        /// <exception cref="CreateRoleException">
        /// Thrown with <see cref="CreateRoleExceptionCode.AddClaimFailed"/> when unable to add a claim to the role.
        /// Thrown with <see cref="CreateRoleExceptionCode.CreateRoleFailed"/> when unable to create the role.
        /// </exception>
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

        /// <summary>
        /// Deletes the role with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="DeleteRoleException">
        /// Thrown with <see cref="DeleteRoleExceptionCode.RoleNotFound"/> when the role does not exist.
        /// Thrown with <see cref="DeleteRoleExceptionCode.DeleteRoleFailed"/> when unable to delete the role.
        /// </exception>
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

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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