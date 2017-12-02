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
    public class UserRepository : IUserRepository
    {
        private readonly ILogger _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityDbContext _context;

        public UserRepository(ILogger<UserRepository> logger, IRoleRepository roleRepository, UserManager<User> userManager, IIdentityDbContext context)
        {
            _logger = logger;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _context = context;
        }           

        public async Task<User> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {     
                user.Roles = await GetRoles(user);
            }

            return user;
        }

        public async Task<User> GetByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {      
                user.Roles = await GetRoles(user);  
            }

            return user;
        }

        public async Task<User> Create(User user, string password)
        { 
            using (var identitydbContextTransaction = _context.BeginTransaction())
            {
                IdentityResult userResult = await _userManager.CreateAsync(user, password);
                if (!userResult.Succeeded) 
                {
                    identitydbContextTransaction.Rollback();
                    // TODO: logging
                    throw new CreateUserException(CreateUserExceptionCode.CreateUserFailed);
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
                            // TODO: logging
                            throw new CreateUserException(CreateUserExceptionCode.AddRoleFailed); 
                        }
                    }
                } 
                    
                identitydbContextTransaction.Commit();              
                return await GetById(user.Id); // TODO: Move to private method
            }            
        }
        
        public async Task Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // TODO: logging
                throw new DeleteUserException(DeleteUserExceptionCode.UserNotFound);
            }
                        
            IdentityResult userResult = _userManager.DeleteAsync(user).Result;
            if (!userResult.Succeeded) 
            {
                // TODO: logging
               throw new DeleteUserException(DeleteUserExceptionCode.DeleteUserFailed);
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
    }
}