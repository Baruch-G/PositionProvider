using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private static bool isFetching;

        public MultiPositionController(ILogger<MultiPositionController> logger)
        {
            _logger = logger;
            if (aircrafts == null)
            {
                aircrafts = new List<Aircraft>();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aircraft>>> GetAsync()
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

        //Obsolete, but may be used for testing
        [HttpPost]
        public async Task<ActionResult<long>> PostVerifyAsync(Aircraft aircraft){
            aircrafts.Add(aircraft);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            isFetching = true;
            while (isFetching) 
            {
                Thread.Sleep(1);
            }
            stopWatch.Stop();
            return Ok(stopWatch.ElapsedMilliseconds);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Aircraft aircraft)
        {
            var count = aircrafts.RemoveAll(x => x.CallSign == aircraft.CallSign && x.Position.Latitude == aircraft.Position.Latitude && x.Position.Longitude == aircraft.Position.Longitude);
            if (count > 0) {
                isFetching = false;
                return Ok(aircraft.CallSign);
            }
            else {
                return NotFound();
            }
        }
    }
}
