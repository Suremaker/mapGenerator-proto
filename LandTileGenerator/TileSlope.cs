using System;

namespace LandTileGenerator
{
    [Flags]
    public enum TileSlope : byte
    {
        LowFlat = 0,
        HighNW = 1,
        HighNE = 2,
        HighSW = 4,
        HighSE = 8,

        HighFlat = HighNE | HighNW | HighSE | HighSW
    }

    public static class TileSlopeExtensions
    {
        public static bool IsFlat(this TileSlope slope)
        {
            return slope == TileSlope.LowFlat || slope == TileSlope.HighFlat;
        }

        public static bool Has(this TileSlope slope, TileSlope value)
        {
            return (slope & value) == value;
        }
    }
}