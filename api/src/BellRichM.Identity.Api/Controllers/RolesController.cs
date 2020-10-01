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
    /// The roles controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public class RolesController : ApiController
    {
        private readonly ILoggerAdapter<RolesController> _logger;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="roleRepository">The <see cref="IRoleRepository"/>.</param>
        public RolesController(ILoggerAdapter<RolesController> logger, IMapper mapper, IRoleRepository roleRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Gets roles.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="List{RoleModel}"/>.</returns>
        [Authorize(Policy = "CanViewUsers")]
        public async Task<IActionResult> Get()
        {
            _logger.LogEvent(EventId.RolesController_Get, string.Empty);

            var roles = await _roleRepository.GetRoles().ConfigureAwait(true);
            var rolesModel = _mapper.Map<List<RoleModel>>(roles);
            return Ok(rolesModel);
        }
    }
}