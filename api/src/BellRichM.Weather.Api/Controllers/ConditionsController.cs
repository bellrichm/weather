using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace BellRichM.Weather.Api.Controllers
{
    [Route("api/[controller]")]
    public class ConditionsController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException();
        }
    }
}