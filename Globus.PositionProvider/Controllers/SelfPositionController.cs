using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Globus.PositionProvider.Utils;

namespace Globus.PositionProvider.Controllers
{
    [ApiController]
    [Route("self-position")]
    public class SelfPositionController : ControllerBase
    {
        private readonly ILogger<SelfPositionController> _logger;

        private static Aircraft aircraft;

        public SelfPositionController(ILogger<SelfPositionController> logger)
        {
            _logger = logger;
            AircraftSimulator.Simulate(aircraft);
        }

        [HttpGet]
        public async Task<ActionResult<Aircraft>> Get()
        {
            return Ok(aircraft);
        }
    }
}
