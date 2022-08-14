using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globus.PositionProvider.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Globus.PositionProvider.Controllers
{
    [ApiController]
    [Route("multi-position")]
    public class MultiPositionController : ControllerBase
    {
        private readonly ILogger<MultiPositionController> _logger;

        private static List<Aircraft> aircrafts;

        public MultiPositionController(ILogger<MultiPositionController> logger)
        {
            _logger = logger;
            if (aircrafts == null)
            {
                aircrafts = new List<Aircraft>();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aircraft>>>
        GetAsync()
        {
            return Ok(aircrafts);
        }

        [HttpGet("{count}")]
        public async Task<ActionResult<IEnumerable<Aircraft>>>
        GetAsync(int count)
        {
            while (aircrafts.Count <= count) {
                var aircraft = new Aircraft { CallSign = $"AIRCRAFT #{aircrafts.Count}", Position = new Position { Latitude = Randomizer.RandomDouble(30,33), Longitude = Randomizer.RandomDouble(34.4,35.6) }, TrueTrack = 0, Altitude = 0 };
                aircraft.Simulate();
                _logger.LogDebug($"Simulating {aircraft.CallSign}");
                aircrafts.Add (aircraft);
            }

            return (count != 0) ? Ok(aircrafts.Take(count)) : BadRequest();
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>>
        GetCountAsync()
        {
            return Ok(aircrafts.Count);
        }

        [HttpPost]
        public async Task<ActionResult<TimeSpan>> PostAsync(Aircraft aircraft)
        {
            var start = DateTime.Now;
            aircraft.Simulate();
            System.Console.WriteLine($"Simulating {aircraft.CallSign}");
            _logger.LogDebug($"Simulating {aircraft.CallSign}");
            aircrafts.Add (aircraft);
            var result = DateTime.Now - start;
            return Ok(new WdTime {ticks = result.Ticks, milliseconds = result.Milliseconds});
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Aircraft aircraft)
        {
            var result = aircrafts.Remove(aircraft);
            return result ? Ok(aircraft.CallSign) : NotFound();
        }

        
    }
}
