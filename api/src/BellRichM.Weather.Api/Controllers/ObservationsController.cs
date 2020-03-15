using AutoMapper;
using BellRichM.Api.Controllers;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> GetObservation([Range(1, int.MaxValue)] int dateTime)
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
        /// Gets the observations within  time period.
        /// </summary>
        /// <param name="timePeriod">The time period.</param>
        /// <returns>The observations.</returns>
        [HttpGet]
        public async Task<IActionResult> GetObservations([FromBody]TimePeriodModel timePeriod)
        {
            _logger.LogEvent(EventId.ObservationsController_Get, "{@timePeriod}", timePeriod);
            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            var observations = await _observationService.GetObservations(timePeriod).ConfigureAwait(true);
            if (observations == null)
            {
                return NotFound();
            }

            List<ObservationModel> observationsModel = _mapper.Map<List<ObservationModel>>(observations);
            return Ok(observationsModel);
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
            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            var observationCreate = _mapper.Map<Observation>(observationCreateModel);
            var observation = await _observationService.CreateObservation(observationCreate).ConfigureAwait(true);
            if (observation == null)
            {
                return NotFound();
            }

            var observationModel = _mapper.Map<ObservationModel>(observation);
            return Ok(observationModel);
        }

        /// <summary>
        /// Updates the specified observation.
        /// </summary>
        /// <param name="observationUpdateModel">The observation to update.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="ObservationModel"/>.</returns>
        [Authorize(Policy = "CanUpdateObservations")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ObservationModel observationUpdateModel)
        {
            _logger.LogEvent(EventId.ObservationsController_Update, "{@observationUpdateModel}", observationUpdateModel);
            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            var observationUpdate = _mapper.Map<Observation>(observationUpdateModel);
            var observation = await _observationService.UpdateObservation(observationUpdate).ConfigureAwait(true);
            if (observation == null)
            {
                return NotFound();
            }

            var observationModel = _mapper.Map<ObservationModel>(observation);
            return Ok(observationModel);
        }

        /// <summary>
        /// Deletes the observation.
        /// </summary>
        /// <param name="dateTime">The identifier.</param>
        /// <returns>A <see cref="Task{IActionResult}"/>.</returns>
        [Authorize(Policy = "CanDeleteObservations")]
        [HttpDelete("{dateTime}")]
        public async Task<IActionResult> Delete([Range(1, int.MaxValue)] int dateTime)
        {
            _logger.LogEvent(EventId.ObservationsController_Delete, "{@dateTime}", dateTime);
            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            var count = await _observationService.DeleteObservation(dateTime).ConfigureAwait(true);
            if (count == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}