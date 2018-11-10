using System.Threading.Tasks;
using AutoMapper;
using BellRichM.Api.Controllers;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BellRichM.Identity.Api.Controllers
{
    /// <summary>
    /// The role controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class RoleController : ApiController
    {
        private readonly ILoggerAdapter<RoleController> _logger;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="roleRepository">The <see cref="IRoleRepository"/>.</param>
        public RoleController(ILoggerAdapter<RoleController> logger, IMapper mapper, IRoleRepository roleRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="RoleModel"/>.</returns>
        [Authorize(Policy = "CanViewRoles")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogEvent(EventIds.RoleController_GetById, "{@id}", id);
            var newRole = await _roleRepository.GetById(id);
            if (newRole == null)
            {
                return NotFound();
            }

            var roleModel = _mapper.Map<RoleModel>(newRole);
            return Ok(roleModel);
        }

        /// <summary>
        /// Creates the specified role.
        /// </summary>
        /// <param name="roleCreate">The role to create.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="RoleModel"/>.</returns>
        [Authorize(Policy = "CanCreateRoles")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleModel roleCreate)
        {
            _logger.LogEvent(EventIds.RoleController_Create, "{@roleCreate}", roleCreate);
            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            var role = _mapper.Map<Role>(roleCreate);
            try
            {
                var newRole = await _roleRepository.Create(role);
                var roleModel = _mapper.Map<RoleModel>(newRole);
                return Ok(roleModel);
            }
            catch (CreateRoleException ex)
            {
                _logger.LogDiagnosticInformation("{@ex}", ex);
                var errorResponseModel = CreateModel(ex);
                return BadRequest(errorResponseModel);
            }
        }

        /// <summary>
        /// Deletes the role by id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [Authorize(Policy = "CanDeleteRoles")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogEvent(EventIds.RoleController_Delete, "{@id}", id);
            try
            {
                await _roleRepository.Delete(id);
                return NoContent();
            }
            catch (DeleteRoleException ex)
            {
                _logger.LogDiagnosticInformation("{@ex}", ex);
                var errorResponseModel = CreateModel(ex);
                return BadRequest(errorResponseModel);
            }
        }
    }
}