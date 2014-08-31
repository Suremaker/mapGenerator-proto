using LandTileGenerator;

namespace MOWorldEditor
{
    public class WorldTile
    {
        public WorldTile(TileSlope slope, int level)
        {
            Slope = slope;
            Level = level;
        }

        public TileSlope Slope { get; private set; }
        public int Level { get; private set; }
    }
}