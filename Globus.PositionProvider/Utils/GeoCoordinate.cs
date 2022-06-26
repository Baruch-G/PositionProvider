using System;

namespace Globus.PositionProvider.Utils
{
    public class GeoCoordinate
    {
        public GeoCoordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public void MoveToPostion(double latitude, double longitude, double distanceKm, double bearing)
        {
            // taken from: https://stackoverflow.com/a/46410871/13549    
            // distance in KM, bearing in degrees

            var R = 6378.1; // Radius of the Earth
            var brng = bearing * Math.PI / 180; // Convert bearing to radian
            var lat = latitude * Math.PI / 180; // Current coords to radians
            var lon = longitude * Math.PI / 180;

            // Do the math magic
            lat = Math.Asin(Math.Sin(lat) * Math.Cos(distanceKm / R) + Math.Cos(lat) * Math.Sin(distanceKm / R) * Math.Cos(brng));
            lon += Math.Atan2(Math.Sin(brng) * Math.Sin(distanceKm / R) * Math.Cos(lat), Math.Cos(distanceKm / R) - Math.Sin(lat) * Math.Sin(lat));

            // Coords back to degrees and return
            Latitude = lat * 180.0 / Math.PI;
            Longitude = lon * 180.0 / Math.PI;

        }
    }
}