using AutoMapper;
using BellRichM.Administration.Api.Models;
using BellRichM.Api.Controllers;
using BellRichM.Logging;
using BellRichM.Logging.Switches;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BellRichM.Administration.Api.Controllers
{
    /// <summary>
    /// The logging filter controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class LoggingFilterController : ApiController
    {
        private readonly ILoggerAdapter<LoggingFilterController> _logger;

        private readonly ILogManager _logManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingFilterController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="logManager">The <see cref="ILogManager"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        public LoggingFilterController(ILoggerAdapter<LoggingFilterController> logger, ILogManager logManager, IMapper mapper)
        {
            _logger = logger;
            _logManager = logManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves the logging filter.
        /// </summary>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        [Authorize(Policy = "CanViewLoggingFilters")]
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogEvent(EventId.LoggingFilterController_Get, string.Empty);

            var loggingFilterSwitchesModel = _mapper.Map<LoggingFilterSwitchesModel>(_logManager.LoggingFilterSwitches);
            return Ok(loggingFilterSwitchesModel);
        }

        /// <summary>
        /// Updates the logging filter.
        /// </summary>
        /// <param name="loggingFilterSwitchesModel">The <see cref="LoggingFilterSwitchesModel"/>.</param>
        /// <returns>A <see cref="IActionResult"/>.</returns>
        [Authorize(Policy = "CanUpdateLoggingFilters")]
        [HttpPatch]
        public IActionResult Update([FromBody] LoggingFilterSwitchesModel loggingFilterSwitchesModel)
        {
            _logger.LogEvent(EventId.LoggingFilterController_Update, "{@loggingFilterSwitchesModel}", loggingFilterSwitchesModel);

            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            _mapper.Map(loggingFilterSwitchesModel, _logManager.LoggingFilterSwitches);

            var updatedLoggingFilterSwitchesModel = _mapper.Map<LoggingFilterSwitchesModel>(_logManager.LoggingFilterSwitches);
            return Ok(updatedLoggingFilterSwitchesModel);
        }
    }
}