using System;
using LandTileGenerator.Generators.Heights;

namespace LandTileGenerator.Generators
{
    public class TileGenerator
    {
        private readonly IHeightGenerator _heightGenerator;

        protected TileGenerator(IHeightGenerator heightGenerator)
        {
            _heightGenerator = heightGenerator;
        }

        public LandTile Generate(int globalX, int globalY, string name, TileSlope slope)
        {
            int lvl = getDetailLevel();
            float delta = 1.0f / (lvl - 1);
            _heightGenerator.PrepareForNewObject(globalX, globalY);

            var vertexes = new Vertex[lvl, lvl];

            for (int i = 0; i < lvl; ++i)
                for (int j = 0; j < lvl; ++j)
                    vertexes[i, j] = new Vertex(i * delta, j * delta, GetHeight(i * delta, j * delta, slope));

            return new LandTile(name, new Mesh(vertexes), slope);
        }

        private float GetHeight(float x1, float y1, TileSlope slope)
        {
            float dist = 1;
            if (slope.Has(TileSlope.HighNW))
            {
                if (slope.Has(TileSlope.HighNE))
                    dist = Math.Min(dist, y1);
                else
                    dist = Math.Min(dist, (float)Distance(x1, y1, 0, 0));
                if (slope.Has(TileSlope.HighSW))
                    dist = Math.Min(dist, x1);
            }
            else if (slope.Has(TileSlope.HighNE))
                dist = Math.Min(dist, (float)Distance(x1, y1, 1, 0));
            if (slope.Has(TileSlope.HighSE))
            {
                if (slope.Has(TileSlope.HighSW))
                    dist = Math.Min(dist, Math.Abs(1 - y1));
                else
                    dist = Math.Min(dist, (float)Distance(x1, y1, 1, 1));
                if (slope.Has(TileSlope.HighNE))
                    dist = Math.Min(dist, Math.Abs(1 - x1));
            }
            else if (slope.Has(TileSlope.HighSW))
                dist = Math.Min(dist, (float)Distance(x1, y1, 0, 1));

            dist = 1 - dist;
            return CalcHeight(x1, y1, dist);
        }

        private float CalcHeight(float x1, float y1, float dist)
        {
            return -_heightGenerator.Generate(x1, y1, dist);
        }

        private static float Distance(float x1, float y1, float x2, float y2)
        {
            float x = x1 - x2;
            float y = y1 - y2;
            return (float)Math.Sqrt(x * x + y * y);
        }

        private int getDetailLevel()
        {
            return 8;
        }
    }
}