using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;

namespace BellRichM.Identity.Api.Controllers
{

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly IJwtManager _jwtManager;

        public UserController(ILogger<UserController> logger, IUserRepository userRepository, IJwtManager jwtManager)
        {
            _logger = logger;
            _userRepository = userRepository;
            _jwtManager = jwtManager;
        }

        [HttpPost("/api/[controller]/[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLogin) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var jwt = await _jwtManager.GenerateToken(userLogin.UserName, userLogin.Password);
            
            if (jwt == null) 
            {
                ModelState.AddModelError("loginError", "Invalid user password combination.");            
                return BadRequest(ModelState);
            }

            var accessToken = new AccessTokenModel
            {
                JsonWebToken = jwt
            };
            return Ok(accessToken);
        }                    
    }
}