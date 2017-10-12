using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BellRichM.Weather.Api.Controllers
{

    [Route("api/[controller]")]
    public class ConditionsController
    {
        private readonly ILogger _logger;

        public ConditionsController(ILogger<ConditionsController> logger)
        {
            _logger = logger;
        }   
        
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInformation("Get conditions route called");
            throw new NotImplementedException();
        }
    }
}