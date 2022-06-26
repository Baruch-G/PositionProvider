using System;
using System.Threading;

using System.Collections;

namespace Globus.PositionProvider.Utils
{
    public static class AircraftSimulator
    {
        public static void Simulate(Aircraft aircraft) 
        {
            Simulate(aircraft, (int)Randomizer.RandomDouble(100,300));
        }
        public static void Simulate(Aircraft aircraft, int knot)
        {
            Console.WriteLine("Starting Simulation");
            var geoCoordinate = new GeoCoordinate(Randomizer.RandomDouble(31, 35), Randomizer.RandomDouble(31, 35));
            var trueTrack = 0;
            var trueHeading = 0;
            var altitude = 0;
            var sendInterval = 0.1;
            var arrowsMode = true;
            var knotToKmConstant = 0.0005144;


            for (int i = 0; i < 100000; i++)
            {
                System.Console.WriteLine("Cycle " + i);
                Thread.Sleep((int)(sendInterval * 1000));

                var KmPassed = sendInterval * knot * knotToKmConstant;

                geoCoordinate.MoveToPostion(geoCoordinate.Latitude, geoCoordinate.Longitude, KmPassed, trueHeading);

                if (!arrowsMode)
                {
                    trueTrack = trueHeading = trueHeading + (int)Randomizer.RandomDouble(-1, 0);
                    altitude += (int)(sendInterval * 10);
                }
            }
        }

        // public static void SimulateMany(IEnumerable<Aircraft> aircrafts)
        // {
        //     Parallel.ForEach(aircrafts, aircraft => StartSimulation(aircraft, Randomizer.RandomDouble(100,300)));
        // }
    }
}