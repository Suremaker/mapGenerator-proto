using System;
using LandTileGenerator;

namespace MOWorldEditor
{
    public class NamedTileGenerator
    {
        private readonly Func<int, int, int, LandTile> _generate;

        public NamedTileGenerator(string name, TileSlope slope, Func<int, int, int, LandTile> generate)
        {
            Name = name;
            Slope = slope;
            _generate = generate;
        }

        public string Name { get; private set; }
        public TileSlope Slope { get; private set; }

        public LandTile Generate(int x, int y, int level)
        {
            return _generate(x, y, level);
        }
    }
}