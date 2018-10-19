using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;
using BellRichM.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILoggerAdapter<UserController> _logger;
        private readonly IMapper _mapper;

        private readonly IUserRepository _userRepository;
        private readonly IJwtManager _jwtManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="userRepository">The <see cref="IUserRepository"/>.</param>
        /// <param name="jwtManager">The <see cref="IJwtManager"/>.</param>
        public UserController(ILoggerAdapter<UserController> logger, IMapper mapper, IUserRepository userRepository, IJwtManager jwtManager)
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
        /// Creates the specified user create.
        /// </summary>
        /// <param name="userCreate">The user to create.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="UserModel"/>.</returns>
        [Authorize(Policy = "CanCreateUsers")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateModel userCreate)
        {
            User newUser;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userCreate);
            try
            {
                newUser = await _userRepository.Create(user, userCreate.Password);
                var userModel = _mapper.Map<UserModel>(newUser);
                return Ok(userModel);
            }
            catch (CreateUserException ex)
            {
                ModelState.AddModelError(ex.Code, ex.Message);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes the user by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [Authorize(Policy = "CanDeleteUsers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _userRepository.Delete(id);
                return NoContent();
            }
            catch (DeleteUserException ex)
            {
                ModelState.AddModelError(ex.Code, ex.Message);
                return BadRequest(ModelState);
            }
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