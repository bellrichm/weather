using AutoMapper;
using BellRichM.Api.Controllers;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Controllers
{
    /// <summary>
    /// The observation controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public class ObservationsController : ApiController
    {
        private readonly ILoggerAdapter<ObservationsController> _logger;
        private readonly IMapper _mapper;
        private readonly IObservationService _observationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationsController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="observationService">The <see cref="IObservationService"/>.</param>
        public ObservationsController(ILoggerAdapter<ObservationsController> logger, IMapper mapper, IObservationService observationService)
        {
            _logger = logger;
            _mapper = mapper;
            _observationService = observationService;
        }

        /// <summary>
        /// Gets the observarion by identifier.
        /// </summary>
        /// <param name="dateTime">The identifier.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="ObservationModel"/>.</returns>
        [Authorize(Policy = "CanViewObservations")]
        [HttpGet("{dateTime}")]
        public async Task<IActionResult> GetObservation(int dateTime)
        {
            _logger.LogEvent(EventId.ObservationsController_Get, "{@dateTime}", dateTime);
            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            var observation = await _observationService.GetObservation(dateTime).ConfigureAwait(true);
            if (observation == null)
            {
                return NotFound();
            }

            var observationModel = _mapper.Map<ObservationModel>(observation);
            return Ok(observationModel);
        }

        /// <summary>
        /// Creates the specified observation.
        /// </summary>
        /// <param name="observationCreateModel">The observation to create.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="ObservationModel"/>.</returns>
        [Authorize(Policy = "CanCreateObservations")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ObservationModel observationCreateModel)
        {
            _logger.LogEvent(EventId.ObservationsController_Create, "{@observationCreate}", observationCreateModel);

            var observationCreate = _mapper.Map<Observation>(observationCreateModel);
            var observation = await _observationService.CreateObservation(observationCreate).ConfigureAwait(true);
            if (observation == null)
            {
                return NotFound();
            }

            var observationModel = _mapper.Map<ObservationModel>(observation);
            return Ok(observationModel);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified observation.
        /// </summary>
        /// <param name="observationUpdate">The observation to update.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="ObservationModel"/>.</returns>
        [Authorize(Policy = "CanUpdateObservations")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ObservationModel observationUpdate)
        {
            _logger.LogEvent(EventId.ObservationsController_Update, "{@observationUpdate}", observationUpdate);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the observation.
        /// </summary>
        /// <param name="dateTime">The identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [Authorize(Policy = "CanDeleteObservations")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int dateTime)
        {
            _logger.LogEvent(EventId.ObservationsController_Delete, "{@dateTime}", dateTime);

            throw new NotImplementedException();
        }
    }
}