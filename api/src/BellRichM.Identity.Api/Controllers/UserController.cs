using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Controllers
{
    /// <summary>
    /// The user controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepository;
        private readonly IJwtManager _jwtManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{UserController}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="userRepository">The <see cref="IUserRepository"/>.</param>
        /// <param name="jwtManager">The <see cref="IJwtManager"/>.</param>
        public UserController(ILogger<UserController> logger, IMapper mapper, IUserRepository userRepository, IJwtManager jwtManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
            _jwtManager = jwtManager;
        }

        /// <summary>
        /// Gets the user by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="UserModel"/>.</returns>
        [Authorize(Policy = "CanViewUsers")]
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

        /// <summary>
        /// Generates the JWT for the <paramref name="userLogin"/>
        /// </summary>
        /// <param name="userLogin">The<see cref="UserLoginModel"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/> containing the JWT.</returns>
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