using LandTileGenerator;

namespace MOWorldEditor
{
    public class WorldMap
    {
        private readonly WorldTile[,] _tiles;
        public WorldMap(int width, int height)
        {
            _tiles = new WorldTile[width, height];
            for (int i = 0; i < width; ++i)
                for (int j = 0; j < height; ++j)
                    _tiles[i, j] = new WorldTile(TileSlope.LowFlat, 0);
        }

        public int Width { get { return _tiles.GetLength(0); } }
        public int Height { get { return _tiles.GetLength(1); } }

        public WorldTile this[int x, int y]
        {
            get { return _tiles[x, y]; }
            set { _tiles[x, y] = value; }
        }
    }
}