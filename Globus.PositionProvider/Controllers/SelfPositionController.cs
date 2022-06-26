using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Globus.PositionProvider.SelfData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Globus.PositionProvider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SelfPositionController
    {
        private readonly ILogger<SelfPositionController> _logger;

        public SelfPositionController(ILogger<SelfPositionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Aircraft Get()
        {
            return SelfAircraft.Aircraft;
        }
    }
}
