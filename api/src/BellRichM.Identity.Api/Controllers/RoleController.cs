using System.Threading.Tasks;
using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BellRichM.Identity.Api.Controllers
{
    /// <summary>
    /// The role controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{RoleController}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="roleRepository">The <see cref="IRoleRepository"/>.</param>
        public RoleController(ILogger<RoleController> logger, IMapper mapper, IRoleRepository roleRepository)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                ModelState.AddModelError(ex.Code, ex.Message);
                return BadRequest(ModelState);
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
            try
            {
                await _roleRepository.Delete(id);
                return NoContent();
            }
            catch (DeleteRoleException ex)
            {
                ModelState.AddModelError(ex.Code, ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}