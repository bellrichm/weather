using System;
using System.Collections.Generic;
using BellRichM.Logging;
using Microsoft.AspNetCore.Mvc;

namespace BellRichM.Weather.Api.Controllers
{
    /// <summary>
    /// The conditions controller
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public class ConditionsController : Controller
    {
        private readonly ILoggerAdapter<ConditionsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionsController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        public ConditionsController(ILoggerAdapter<ConditionsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets weather condistions.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{String}"/></returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        /// <remarks>Not yet implemented.</remarks>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDiagnosticInformation("Get conditions route called");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new weather condition.
        /// </summary>
        /// <exception cref="NotImplementedException">Not Implemented</exception>
        /// <remarks>Not yet implemented.</remarks>
        public void Post()
        {
            throw new NotImplementedException();
        }
    }
}