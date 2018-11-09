using BellRichM.Exceptions;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Repositories
{
    /// <inheritdoc/>
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly ILoggerAdapter<UserRepository> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityDbContext _context;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="roleRepository">The <see cref="IRoleRepository"/>.</param>
        /// <param name="userManager">The <see cref="UserManager{User}"/>.</param>
        /// <param name="context">The <see cref="IIdentityDbContext"/>.</param>
        public UserRepository(ILoggerAdapter<UserRepository> logger, IRoleRepository roleRepository, UserManager<User> userManager, IIdentityDbContext context)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<User> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Roles = await GetRoles(user);
            }

            return user;
        }

        /// <inheritdoc/>=
        public async Task<User> GetByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                user.Roles = await GetRoles(user);
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
            using (var identitydbContextTransaction = _context.BeginTransaction())
            {
                IdentityResult userResult = await _userManager.CreateAsync(user, password);
                if (!userResult.Succeeded)
                {
                    identitydbContextTransaction.Rollback();

                    var exceptionDetails = GetErrors(userResult);

                    // TODO: logging
                    throw new CreateUserException(CreateUserExceptionCode.CreateUserFailed, exceptionDetails);
                }

                if (user.Roles != null)
                {
                    foreach (var userRole in user.Roles)
                    {
                        var role = await _roleRepository.GetByName(userRole.Name);
                        if (role == null)
                        {
                            identitydbContextTransaction.Rollback();

                            // TODO: logging
                            throw new CreateUserException(CreateUserExceptionCode.RoleNotFound);
                        }

                        var roleResult = await _userManager.AddToRoleAsync(user, userRole.Name);
                        if (!roleResult.Succeeded)
                        {
                            identitydbContextTransaction.Rollback();

                            var exceptionDetails = GetErrors(roleResult);

                            // TODO: logging
                            throw new CreateUserException(CreateUserExceptionCode.AddRoleFailed, exceptionDetails);
                        }
                    }
                }

                identitydbContextTransaction.Commit();
                return await GetById(user.Id); // TODO: Move to private method
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
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            { // TODO: logging
                throw new DeleteUserException(DeleteUserExceptionCode.UserNotFound);
            }

            IdentityResult userResult = _userManager.DeleteAsync(user).Result;
            if (!userResult.Succeeded)
            {
                var exceptionDetails = GetErrors(userResult);

                // TODO: logging
                throw new DeleteUserException(DeleteUserExceptionCode.DeleteUserFailed, exceptionDetails);
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
                    _userManager.Dispose();
                }

                disposed = true;
            }
        }

        private async Task<IEnumerable<Role>> GetRoles(User user)
        {
            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = new List<Role>();
            foreach (var roleName in roleNames)
            {
                var role = await _roleRepository.GetByName(roleName);
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