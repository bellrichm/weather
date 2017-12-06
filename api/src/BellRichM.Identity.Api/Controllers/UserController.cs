using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using AutoMapper;
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
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepository;
        private readonly IJwtManager _jwtManager;

        public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository userRepository, IJwtManager jwtManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtManager = jwtManager;
        }

        [HttpGet("{id}")]   
        public async Task<IActionResult> GetById(string id)
        {
            var newUser = await _userRepository.GetById(id);
            if (newUser == null)
            {
                return NotFound();
            }
            
            var userModel = _mapper.Map<UserModel>(newUser);
            return Ok(userModel);
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