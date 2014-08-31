using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LandTileGenerator;
using LandTileGenerator.Generators;

namespace MOWorldEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var worldMap = new WorldMap(16, 16);
            worldMap[1,1] = new WorldTile(TileSlope.HighSE, 0);
            worldMap[2,1] = new WorldTile(TileSlope.HighSE|TileSlope.HighSW, 0);
            worldMap[3,1] = new WorldTile(TileSlope.HighSW, 0);
            worldMap[1, 2] = new WorldTile(TileSlope.HighNE|TileSlope.HighSE, 0);
            worldMap[2, 2] = new WorldTile(TileSlope.LowFlat, 1);
            worldMap[3, 2] = new WorldTile(TileSlope.HighSW|TileSlope.HighNW, 0);
            worldMap[1, 3] = new WorldTile(TileSlope.HighNE, 0);
            worldMap[2, 3] = new WorldTile(TileSlope.HighNE | TileSlope.HighNW, 0);
            worldMap[3, 3] = new WorldTile(TileSlope.HighNW, 0);

            worldMap[6, 6] = new WorldTile(TileSlope.HighSE, 0);
            worldMap[7, 6] = new WorldTile(TileSlope.HighSW, 0);
            worldMap[6, 7] = new WorldTile(TileSlope.HighNE, 0);
            worldMap[7, 7] = new WorldTile(TileSlope.HighNW, 0);

            worldMap[1, 5] = new WorldTile(TileSlope.HighSE, 0);
            worldMap[2, 5] = new WorldTile(TileSlope.HighSE | TileSlope.HighSW, 0);
            worldMap[3, 5] = new WorldTile(TileSlope.HighSE | TileSlope.HighSW, 0);
            worldMap[4, 5] = new WorldTile(TileSlope.HighSW, 0);

            worldMap[1, 6] = new WorldTile(TileSlope.HighNE | TileSlope.HighSE, 0);
            worldMap[2, 6] = new WorldTile(TileSlope.HighSE, 1);
            worldMap[3, 6] = new WorldTile(TileSlope.HighSW, 1);
            worldMap[4, 6] = new WorldTile(TileSlope.HighSW | TileSlope.HighNW, 0);

            worldMap[1, 7] = new WorldTile(TileSlope.HighNE | TileSlope.HighSE, 0);
            worldMap[2, 7] = new WorldTile(TileSlope.HighNE, 1);
            worldMap[3, 7] = new WorldTile(TileSlope.HighNW, 1);
            worldMap[4, 7] = new WorldTile(TileSlope.HighSW | TileSlope.HighNW, 0);

            worldMap[1, 8] = new WorldTile(TileSlope.HighNE, 0);
            worldMap[2, 8] = new WorldTile(TileSlope.HighNE | TileSlope.HighNW, 0);
            worldMap[3, 8] = new WorldTile(TileSlope.HighNE | TileSlope.HighNW, 0);
            worldMap[4, 8] = new WorldTile(TileSlope.HighNW, 0);
            var editor = new WorldEditor(new WorldModel(GenerateTiles(), worldMap));
            editor.Run();
        }

        private static IEnumerable<NamedTileGenerator> GenerateTiles()
        {
            var rand = new Random();
            var linear = new LinearLandGenerator(rand);
            var clifs = new ClifsGenerator(rand);

            return GenerateTiles("flat", TileSlope.LowFlat, linear)
                .Concat(GenerateTiles("clifNW", TileSlope.HighNW, clifs))
                .Concat(GenerateTiles("clifNE", TileSlope.HighNE, clifs))
                .Concat(GenerateTiles("clifSW", TileSlope.HighSW, clifs))
                .Concat(GenerateTiles("clifSE", TileSlope.HighSE, clifs))

                .Concat(GenerateTiles("clifS", TileSlope.HighSE | TileSlope.HighSW, clifs))
                .Concat(GenerateTiles("clifE", TileSlope.HighSE | TileSlope.HighNE, clifs))
                .Concat(GenerateTiles("clifN", TileSlope.HighNE | TileSlope.HighNW, clifs))
                .Concat(GenerateTiles("clifW", TileSlope.HighNW | TileSlope.HighSW, clifs))

                .Concat(GenerateTiles("clifNES", TileSlope.HighNW | TileSlope.HighNE | TileSlope.HighSE, clifs))
                .Concat(GenerateTiles("clifESW", TileSlope.HighNE | TileSlope.HighSE | TileSlope.HighSW, clifs))
                .Concat(GenerateTiles("clifSWN", TileSlope.HighSE | TileSlope.HighSW | TileSlope.HighNW, clifs))
                .Concat(GenerateTiles("clifWNE", TileSlope.HighSW | TileSlope.HighNW | TileSlope.HighNE, clifs))

                .Concat(GenerateTiles("clifNWSE", TileSlope.HighNW | TileSlope.HighSE, clifs))
                .Concat(GenerateTiles("clifNESW", TileSlope.HighNE | TileSlope.HighSW, clifs));
        }

        private static IEnumerable<NamedTileGenerator> GenerateTiles(string name, TileSlope slope, TileGenerator generator)
        {
            yield return new NamedTileGenerator(name, slope, (x, y, level) => generator.Generate(x, y, name, slope));
        }
    }
}
