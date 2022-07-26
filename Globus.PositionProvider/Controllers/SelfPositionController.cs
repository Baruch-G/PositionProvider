using System.Threading.Tasks;
using Globus.PositionProvider.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            if (aircraft == null)
            {
                aircraft =
                    new Aircraft {
                        CallSign = "SelfData",
                        Position =
                            new Position {
                                Longitude = Randomizer.RandomDouble(34.4, 35.6),
                                Latitude = Randomizer.RandomDouble(30, 33)
                            },
                        TrueTrack = 0,
                        Altitude = 0
                    };
                aircraft.Simulate();
            }
            _logger.LogDebug("SelfPosition Created");
        }

        [HttpGet]
        public async Task<ActionResult<Aircraft>> Get()
        {
            _logger.LogDebug("GET Request for SelfPosition");
            return (aircraft.Position != null) ? Ok(aircraft) : BadRequest();
        }
    }
}
