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
    [Route("obstacles")]
    public class ObstaclesController : ControllerBase
    {
        private readonly ILogger<ObstaclesController> _logger;

        private static List<Obstacle> obstacles;

        public ObstaclesController(ILogger<ObstaclesController> logger)
        {
            _logger = logger;
            if (obstacles == null)
            {
                obstacles = new List<Obstacle>();
            }
        }

        [HttpGet("{count}")]
        public async Task<ActionResult<IEnumerable<Obstacle>>>
        GetAsync(int count)
        {
            while (obstacles.Count < count)
            {
                var obstacle =
                    new Obstacle {
                        Name = $"Obstacle #{obstacles.Count}",
                        Position =
                            new Position {
                                Longitude = Randomizer.RandomDouble(34.4, 35.6),
                                Latitude = Randomizer.RandomDouble(30, 33)
                            },
                        Description = "",
                        HeightMeters = Randomizer.RandomDouble(0, 200)
                    };

                _logger.LogDebug($"Obstacle {obstacle.Name} created");
                obstacles.Add (obstacle);
            }

            return (count != 0) ? Ok(obstacles.Take(count)) : BadRequest();
        }

    }
}
