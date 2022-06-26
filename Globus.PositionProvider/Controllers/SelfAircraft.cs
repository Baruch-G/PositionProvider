using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Globus.PositionProvider;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Globus.PositionProvider.SelfData
{
    public static class SelfAircraft
    {
        public static Aircraft Aircraft { get; set; }
    }
}
