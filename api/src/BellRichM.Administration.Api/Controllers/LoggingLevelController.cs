using AutoMapper;
using BellRichM.Administration.Api.Models;
using BellRichM.Api.Controllers;
using BellRichM.Logging;
using BellRichM.Logging.Switches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Events;

namespace BellRichM.Administration.Api.Controllers
{
    /// <summary>
    /// The logging level controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class LoggingLevelController : ApiController
    {
        private readonly ILoggerAdapter<LoggingLevelController> _logger;

        private readonly ILogManager _logManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingLevelController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="logManager">The <see cref="ILogManager"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        public LoggingLevelController(ILoggerAdapter<LoggingLevelController> logger, ILogManager logManager, IMapper mapper)
        {
            _logger = logger;
            _logManager = logManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the logging level.
        /// </summary>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        [Authorize(Policy = "CanViewLoggingLevels")]
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogEvent(EventId.LoggingLevelController_Get, string.Empty);

            var loggingLevelSwitchesModel = _mapper.Map<LoggingLevelSwitchesModel>(_logManager.LoggingLevelSwitches);
            return Ok(loggingLevelSwitchesModel);
        }

        /// <summary>
        /// Updates the logging level.
        /// </summary>
        /// <param name="loggingLevelSwitchesModel">The <see cref="LoggingLevelSwitches"/>.</param>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        [Authorize(Policy = "CanUpdateLoggingLevels")]
        [HttpPatch]
        public IActionResult Update([FromBody] LoggingLevelSwitchesModel loggingLevelSwitchesModel)
        {
            _logger.LogEvent(EventId.LoggingLevelController_Update, "{@loggingLevelSwitchesModel}", loggingLevelSwitchesModel);

            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            _mapper.Map(loggingLevelSwitchesModel, _logManager.LoggingLevelSwitches);

            var updatedLoggingLevelSwitchesModel = _mapper.Map<LoggingLevelSwitchesModel>(_logManager.LoggingLevelSwitches);
            return Ok(updatedLoggingLevelSwitchesModel);
        }
    }
}