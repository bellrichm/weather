using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BellRichM.Weather.Api.Controllers
{
    /// <summary>
    /// The conditions controller
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public class ConditionsController : Controller
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionsController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{ConditionsController}"/>.</param>
        public ConditionsController(ILogger<ConditionsController> logger)
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
            _logger.LogInformation("Get conditions route called");
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