using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<Aircraft>> GetAsync(int count)
        {
            while (aircrafts.Count < count) {
                var aircraft = new Aircraft { CallSign = $"AIRCRAFT #{aircrafts.Count}", Position = new Position { Latitude = Randomizer.RandomDouble(31,35), Longitude = Randomizer.RandomDouble(31,35) }, TrueTrack = 0, Altitude = 0 };
                aircraft.Simulate();
                _logger.LogDebug($"Simulating {aircraft.CallSign}");
                aircrafts.Add(aircraft);
            }

            return aircrafts.Take(count);
        }
    }
}
