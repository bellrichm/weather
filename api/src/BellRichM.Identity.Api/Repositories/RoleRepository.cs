using BellRichM.Exceptions;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Repositories
{
    /// <inheritdoc/>
    public class RoleRepository : IRoleRepository, IDisposable
    {
        private readonly ILoggerAdapter<RoleRepository> _logger;
        private readonly RoleManager<Role> _roleManager;
        private readonly IdentityDbContext _context;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="roleManager">The <see cref="RoleManager{Role}"/>.</param>
        /// <param name="context">The <see cref="IdentityDbContext"/>.</param>
        public RoleRepository(ILoggerAdapter<RoleRepository> logger, RoleManager<Role> roleManager, IdentityDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Role> GetById(string id)
        {
            _logger.LogDiagnosticDebug("GetById: {@id}", id);
            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(true);

            if (role != null)
            {
                role.ClaimValues = await GetClaimValues(role).ConfigureAwait(true);
            }

            return role;
        }

        /// <inheritdoc/>
        public async Task<Role> GetByName(string name)
        {
            _logger.LogDiagnosticDebug("GetByName: {@name}", name);
            var role = await _roleManager.FindByNameAsync(name).ConfigureAwait(true);

            if (role != null)
            {
                role.ClaimValues = await GetClaimValues(role).ConfigureAwait(true);
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
            _logger.LogDiagnosticDebug("Create: {@role}", role);
            if (role == null)
            {
                return null;
            }

            using (var identitydbContextTransaction = _context.BeginTransaction())
            {
                IdentityResult roleResult = await _roleManager.CreateAsync(role).ConfigureAwait(true);

                if (!roleResult.Succeeded)
                {
                    identitydbContextTransaction.Rollback();

                    var exceptionDetails = GetErrors(roleResult);

                    throw new CreateRoleException(CreateRoleExceptionCode.CreateRoleFailed, exceptionDetails);
                }

                if (role.ClaimValues != null)
                {
                    foreach (var claimValue in role.ClaimValues)
                    {
                        var claim = new Claim(claimValue.Type, claimValue.Value);
                        var claimResult = await _roleManager.AddClaimAsync(role, claim).ConfigureAwait(true);
                        if (!claimResult.Succeeded)
                        {
                            identitydbContextTransaction.Rollback();

                            var exceptionDetails = GetErrors(claimResult);

                            throw new CreateRoleException(CreateRoleExceptionCode.AddClaimFailed, exceptionDetails);
                        }
                    }
                }

                identitydbContextTransaction.Commit();
                return await GetById(role.Id).ConfigureAwait(true); // TODO: Move to private method
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
            _logger.LogDiagnosticDebug("Delete: {@id}", id);
            var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(true);
            if (role == null)
            {
                throw new DeleteRoleException(DeleteRoleExceptionCode.RoleNotFound);
            }

            IdentityResult roleResult = _roleManager.DeleteAsync(role).Result;
            if (!roleResult.Succeeded)
            {
                var exceptionDetails = GetErrors(roleResult);

                throw new DeleteRoleException(DeleteRoleExceptionCode.DeleteRoleFailed, exceptionDetails);
            }
        }

            /// <inheritdoc/>
        public async Task<IEnumerable<Role>> GetRoles()
        {
            _logger.LogDiagnosticDebug("GetRoles: ");

            var roles = await _roleManager.Roles.ToListAsync().ConfigureAwait(true);
            foreach (Role role in roles)
            {
                role.ClaimValues = await GetClaimValues(role).ConfigureAwait(true);
            }

            return roles;
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
                    _context.Dispose();
                }

                disposed = true;
            }
        }

        private async Task<List<ClaimValue>> GetClaimValues(Role role)
        {
            var roleClaims = await _roleManager.GetClaimsAsync(role).ConfigureAwait(true);

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

        private List<ExceptionDetail> GetErrors(IdentityResult identityResult)
        {
            var exceptionDetails = new List<ExceptionDetail>();
            foreach (var error in identityResult.Errors)
            {
                exceptionDetails.Add(
                    new ExceptionDetail
                    {
                         Code = error.Code,
                         Text = error.Description
                    });
             }

            return exceptionDetails;
        }
    }
}
