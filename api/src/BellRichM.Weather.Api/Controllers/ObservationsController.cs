using BellRichM.Logging;
using BellRichM.Weather.Api.Models;
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
    public class ObservationsController
    {
        private readonly ILoggerAdapter<ObservationsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationsController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        public ObservationsController(ILoggerAdapter<ObservationsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the observarion by identifier.
        /// </summary>
        /// <param name="dateTime">The identifier.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="ObservationModel"/>.</returns>
        [Authorize(Policy = "CanViewObservations")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetObservation(int dateTime)
        {
            _logger.LogEvent(EventId.ObservationsController_Get, "{@dateTime}", dateTime);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the specified observation.
        /// </summary>
        /// <param name="observationCreate">The observation to create.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>containing the <see cref="ObservationModel"/>.</returns>
        [Authorize(Policy = "CanCreateObservations")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ObservationModel observationCreate)
        {
            _logger.LogEvent(EventId.ObservationsController_Create, "{@observationCreate}", observationCreate);

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