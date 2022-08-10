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

        [HttpGet("{count}")]
        public async Task<ActionResult<IEnumerable<Aircraft>>>
        GetAsync(int count)
        {
            while (aircrafts.Count <= count) {
                var aircraft = new Aircraft { CallSign = $"AIRCRAFT #{aircrafts.Count}", Position = new Position { Latitude = Randomizer.RandomDouble(34.4,35.6), Longitude = Randomizer.RandomDouble(30,33) }, TrueTrack = 0, Altitude = 0 };
                aircraft.Simulate();
                _logger.LogDebug($"Simulating {aircraft.CallSign}");
                aircrafts.Add (aircraft);
            }

            return (count != 0) ? Ok(aircrafts.Take(count)) : BadRequest();
        }

        [HttpGet("create-delete")]
        public async Task<ActionResult<TimeSpan>> GetAsync()
        {
            var start = DateTime.Now;
            var aircraft =
                new Aircraft {
                    CallSign = $"AIRCRAFT #{aircrafts.Count}",
                    Position =
                        new Position {
                            Longitude = Randomizer.RandomDouble(34.4, 35.6),
                            Latitude = Randomizer.RandomDouble(30, 33)
                        },
                    TrueTrack = 0,
                    Altitude = 0
                };
            aircraft.Simulate();
            _logger.LogDebug($"Simulating {aircraft.CallSign}");
            aircrafts.Add (aircraft);
            aircrafts.Remove (aircraft);
            return Ok(DateTime.Now - start);
        }
    }
}
