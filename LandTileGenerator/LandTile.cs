namespace LandTileGenerator
{
    public class LandTile
    {
        public LandTile(string name, Mesh mesh, TileSlope slope)
        {
            Name = name;
            Mesh = mesh;
            Slope = slope;
        }

        public string Name { get; private set; }
        public Mesh Mesh { get; private set; }
        public TileSlope Slope { get; private set; }
    }
}