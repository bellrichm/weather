using BellRichM.Exceptions;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Repositories
{
    /// <inheritdoc/>
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly ILoggerAdapter<UserRepository> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<User> _userManager;
        private readonly IdentityDbContext _context;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="roleRepository">The <see cref="IRoleRepository"/>.</param>
        /// <param name="userManager">The <see cref="UserManager{User}"/>.</param>
        /// <param name="context">The <see cref="IdentityDbContext"/>.</param>
        public UserRepository(ILoggerAdapter<UserRepository> logger, IRoleRepository roleRepository, UserManager<User> userManager, IdentityDbContext context)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<User> GetById(string id)
        {
            _logger.LogDiagnosticDebug("GetById: {@id}", id);
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(true);
            if (user != null)
            {
                user.Roles = await GetRoles(user).ConfigureAwait(true);
            }

            return user;
        }

        /// <inheritdoc/>=
        public async Task<User> GetByName(string name)
        {
            _logger.LogDiagnosticDebug("GetByName: {@name}", name);
            var user = await _userManager.FindByNameAsync(name).ConfigureAwait(true);
            if (user != null)
            {
                user.Roles = await GetRoles(user).ConfigureAwait(true);
            }

            return user;
        }

        /// <summary>
        /// Creates the specified <paramref name="user"/> with a <paramref name="password"/>.
        /// </summary>
        /// <param name="user">The <see cref="User"/>.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        /// <exception cref="CreateUserException">
        /// Thrown with <see cref="CreateUserExceptionCode.RoleNotFound"/> when the user has a role that does not exist.
        /// Thrown with <see cref="CreateUserExceptionCode.AddRoleFailed"/> when unable to add a role to the user.
        /// Thrown with <see cref="CreateUserExceptionCode.CreateUserFailed"/> when unable to create the user.
        /// </exception>
        public async Task<User> Create(User user, string password)
        {
            _logger.LogDiagnosticDebug("Create: {@user}", user);
            if (user == null)
            {
                return null;
            }

            using (var identitydbContextTransaction = _context.BeginTransaction())
            {
                IdentityResult userResult = await _userManager.CreateAsync(user, password).ConfigureAwait(true);
                if (!userResult.Succeeded)
                {
                    identitydbContextTransaction.Rollback();

                    var exceptionDetails = GetErrors(userResult);

                    throw new CreateUserException(CreateUserExceptionCode.CreateUserFailed, exceptionDetails);
                }

                if (user.Roles != null)
                {
                    foreach (var userRole in user.Roles)
                    {
                        var role = await _roleRepository.GetByName(userRole.Name).ConfigureAwait(true);
                        if (role == null)
                        {
                            identitydbContextTransaction.Rollback();

                            throw new CreateUserException(CreateUserExceptionCode.RoleNotFound);
                        }

                        var roleResult = await _userManager.AddToRoleAsync(user, userRole.Name).ConfigureAwait(true);
                        if (!roleResult.Succeeded)
                        {
                            identitydbContextTransaction.Rollback();

                            var exceptionDetails = GetErrors(roleResult);

                            throw new CreateUserException(CreateUserExceptionCode.AddRoleFailed, exceptionDetails);
                        }
                    }
                }

                identitydbContextTransaction.Commit();
                return await GetById(user.Id).ConfigureAwait(true); // TODO: Move to private method
            }
        }

        /// <summary>
        /// Deletes the user with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        /// <exception cref="DeleteUserException">
        /// Thrown with <see cref="DeleteUserExceptionCode.UserNotFound"/> when the user does not exist.
        /// Thrown with <see cref="DeleteUserExceptionCode.DeleteUserFailed"/> when unable to delete the user.
        /// </exception>
        public async Task Delete(string id)
        {
            _logger.LogDiagnosticDebug("Delete: {@id}", id);
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(true);
            if (user == null)
            {
                throw new DeleteUserException(DeleteUserExceptionCode.UserNotFound);
            }

            IdentityResult userResult = _userManager.DeleteAsync(user).Result;
            if (!userResult.Succeeded)
            {
                var exceptionDetails = GetErrors(userResult);

                throw new DeleteUserException(DeleteUserExceptionCode.DeleteUserFailed, exceptionDetails);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> GetUsers()
        {
            _logger.LogDiagnosticDebug("GetUsers: ");

            var users = await _userManager.Users.ToListAsync().ConfigureAwait(true);
            foreach (User user in users)
            {
                user.Roles = await GetRoles(user).ConfigureAwait(true);
            }

            return users;
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
                    _userManager.Dispose();
                    _context.Dispose();
                }

                disposed = true;
            }
        }

        private async Task<IEnumerable<Role>> GetRoles(User user)
        {
            var roleNames = await _userManager.GetRolesAsync(user).ConfigureAwait(true);
            var roles = new List<Role>();
            foreach (var roleName in roleNames)
            {
                var role = await _roleRepository.GetByName(roleName).ConfigureAwait(true);
                roles.Add(role);
            }

            return roles;
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
