using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Globus.PositionProvider.Utils;

namespace Globus.PositionProvider.WebSockets
{
    public static class WsServer 
    {
        public static WebSocketServer otherPlanesServer = new WebSocketServer("ws://0.0.0.0:2000");
        public static List<Aircraft> aircrafts;

        public static void InitServer()
        {
             if (otherPlanesServer.WebSocketServices.Count == 0) {
                otherPlanesServer.AddWebSocketService<OtherPlanesData>("/other-planes");
                otherPlanesServer.Start();
            }
        }
    }

    public class OtherPlanesData : WebSocketBehavior
    {
        public static int count {get; set;}
        protected override void OnOpen() {
            Task.Run(()=>{
                while (true) 
                {
                    Sessions.Broadcast(JsonConvert.SerializeObject((count != 0) ? AircraftList.aircrafts.Take(count) : new List<Aircraft>()));
                    Thread.Sleep(1000/30);
                }
            });
        }

        protected override void OnMessage(MessageEventArgs eventArgs)
        {
            count = JsonConvert.DeserializeObject<int>(eventArgs.Data);
            while (AircraftList.aircrafts.Count <= count)
            {
                var aircraft = new Aircraft { CallSign = $"AIRCRAFT #{AircraftList.aircrafts.Count}", Position = new Position { Latitude = Randomizer.RandomDouble(30, 33), Longitude = Randomizer.RandomDouble(34.4, 35.6) }, TrueTrack = Randomizer.RandomInt(0, 360), Altitude = 0 };
                aircraft.Simulate();
                AircraftList.aircrafts.Add(aircraft);
            }
        }
    }
}
