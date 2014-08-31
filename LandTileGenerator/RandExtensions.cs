using System;

namespace LandTileGenerator
{
    public static class RandExtensions
    {
        public static float NextFloat(this Random random)
        {
            return (float) random.NextDouble();
        }
    }
}