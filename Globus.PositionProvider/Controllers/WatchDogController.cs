using System;
using System.Threading.Tasks;
using Globus.PositionProvider.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Globus.PositionProvider.Controllers
{
    [ApiController]
    [Route("watch-dog")]
    public class WatchDogController : ControllerBase
    {
        private readonly ILogger<WatchDogController> _logger;

        private static Aircraft aircraft;

        public WatchDogController(ILogger<WatchDogController> logger)
        {
            _logger = logger;

            _logger.LogDebug("WatchDog Controller Created");
        }

        [HttpGet]
        public async Task<ActionResult<bool>> Get()
        {
            _logger.LogDebug("GET Request for Is alive");
            return Ok(true);
        }
    }
}
