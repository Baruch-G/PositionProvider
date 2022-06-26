using System;

namespace Globus.PositionProvider.Utils
{
    public static class Randomizer
    {
        public static double RandomDouble(double minimum, double maximum)
        {
            return new Random().NextDouble() * (maximum - minimum) + minimum;
        }
    }
}