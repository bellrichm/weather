using AutoMapper;
using BellRichM.Api.Controllers;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Controllers
{
    /// <summary>
    /// The users controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly ILoggerAdapter<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="userRepository">The <see cref="IUserRepository"/>.</param>
        public UsersController(ILoggerAdapter<UsersController> logger, IMapper mapper, IUserRepository userRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets users.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="List{UserModel}"/>.</returns>
        [Authorize(Policy = "CanViewUsers")]
        public async Task<IActionResult> Get()
        {
            _logger.LogEvent(EventId.UsersController_Get, string.Empty);

            var users = await _userRepository.GetUsers().ConfigureAwait(true);
            var usersModel = _mapper.Map<List<UserModel>>(users);
            return Ok(usersModel);
        }
    }
}