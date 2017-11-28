using System;
using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BellRichM.Identity.Api.Services {
    public class JwtManager : IJwtManager {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;

        public JwtManager (ILogger<JwtManager> logger, IUserRepository userRepository, SignInManager<User> signInManager) {
            _logger = logger;
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateToken (string userId, string passWord) {
            throw new NotImplementedException();
        }
    }    
}