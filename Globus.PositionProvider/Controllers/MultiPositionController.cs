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
    [Route("multi-position")]
    public class MultiPositionController : ControllerBase
    {
        private readonly ILogger<MultiPositionController> _logger;
        private readonly List<Aircraft> aircrafts;

        public MultiPositionController(ILogger<MultiPositionController> logger)
        {
            _logger = logger;
            aircrafts = new List<Aircraft>();

        }

        [HttpGet("{count}")]
        public async Task<IEnumerable<Aircraft>> GetAsync(int count)
        {
            while (aircrafts.Count < count) {
                var aircraft = new Aircraft();
                AircraftSimulator.Simulate(aircraft);
                aircrafts.Add(aircraft);
            }

            return aircrafts.Take(count);
        }
    }
}
