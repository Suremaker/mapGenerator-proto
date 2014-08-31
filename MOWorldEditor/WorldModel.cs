using System.Collections.Generic;
using System.Linq;
using LandTileGenerator;
using Mogre;

namespace MOWorldEditor
{
    public class WorldModel
    {
        private readonly WorldMap _worldMap;
        private readonly NamedTileGenerator[] _tileset;

        public WorldModel(IEnumerable<NamedTileGenerator> namedTileGenerators, WorldMap worldMap)
        {
            _worldMap = worldMap;
            _tileset = namedTileGenerators.ToArray();
        }

        public void UpdateMap(int x, int y, string name, int level)
        {
            var landTile = GenerateTile(x, y, name, level);

            _worldMap[x, y] = new WorldTile(landTile.Slope, level);

        }

        public WorldMap Map { get { return _worldMap; } }

        public LandTile GenerateTile(int x, int y, string name, int level)
        {
            return _tileset.First(t => t.Name == name).Generate(x, y, level);
        }

        public LandTile GenerateTile(int x, int y, TileSlope slope, int level)
        {
            return _tileset.First(t => t.Slope == slope).Generate(x, y, level);
        }

        public void Initialize(SceneManager mgr)
        {
            WorldObject = new WorldObject(mgr, this);
        }

        public WorldObject WorldObject { get; private set; }
    }
}