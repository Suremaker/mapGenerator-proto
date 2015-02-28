using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageOps;
using ImageOps.Sources;
using ImageOps.Sources.Regions;
using Mogre;

namespace MOWorldEditor
{
    public class TextureMaker
    {
        private const int _TEXTURE_TILE_SIZE = 256;
        private TileTextureMixer _tileTextureMixer;
        private Random _random;

        public TextureMaker()
        {
            _tileTextureMixer = new TileTextureMixer();
            _random = new Random(321);
        }

        public void CreateFor(string materialName, WorldMap map, List<WorldObject.TileFaces> tileFaces)
        {
            var textureName = materialName + ".png";
            WriteMapTexture(textureName, map, tileFaces);

            MaterialPtr material = MaterialManager.Singleton.Create(materialName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, true);

            material.GetTechnique(0).GetPass(0).CreateTextureUnitState(textureName);
        }

        private void WriteMapTexture(string textureName, WorldMap map, List<WorldObject.TileFaces> tileFaces)
        {
            int txWidth = map.Width * _TEXTURE_TILE_SIZE;
            int txHeight = map.Height * _TEXTURE_TILE_SIZE;

            using (var rockSource = _tileTextureMixer.MixTextures(txWidth, txHeight, LoadSources("rock")))
            using (var grassSource = _tileTextureMixer.MixTextures(txWidth, txHeight, LoadSources("grass")))
            {
                var finalSource = grassSource;
                for (int x = 0; x < map.Width; x++)
                    for (int y = 0; y < map.Height; ++y)
                        finalSource = RenderTile(finalSource, rockSource, tileFaces.First(t => t.TileX == x && t.TileY == y));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                finalSource.ToBitmap().Save(textureName);

                sw.Stop();
                Debug.WriteLine(sw.Elapsed);
            }
        }

        private IPixelSource[] LoadSources(string name)
        {
            return Directory
                .EnumerateFiles("Resources", string.Format("{0}*.png", name), SearchOption.TopDirectoryOnly)
                .Select(fn => new Bitmap(fn).AsPixelSource())
                .ToArray();
        }

        private IPixelSource RenderTile(IPixelSource finalSource, IPixelSource rockSource, WorldObject.TileFaces tileFaces)
        {
            foreach (var face in tileFaces.Faces.Where(ShouldBeSteep))
            {
                var polygon = Regions.Polygon(ToTexturePoints(face));
                finalSource = finalSource.BlendRegion(polygon, RenderFace(face, rockSource, polygon), BlendingMethods.Normal);
            }
            return finalSource;
        }

        private static IPixelSource RenderFace(WorldObject.Face face, IPixelSource rockSource, IRegion polygon)
        {
            int v1 = IsSteep(face.Vertex1) ? 1 : -1;
            int v2 = IsSteep(face.Vertex2) ? 1 : -1;
            int v3 = IsSteep(face.Vertex3) ? 1 : -1;

            if (v1 == v2 && v1 == v3)
                return rockSource.Crop(polygon.BoundingBox);

            PointF p1 = new PointF(face.Vertex1.Position.x * _TEXTURE_TILE_SIZE - polygon.BoundingBox.X, face.Vertex1.Position.y * _TEXTURE_TILE_SIZE - polygon.BoundingBox.Y);
            PointF p2 = new PointF(face.Vertex2.Position.x * _TEXTURE_TILE_SIZE - polygon.BoundingBox.X, face.Vertex2.Position.y * _TEXTURE_TILE_SIZE - polygon.BoundingBox.Y);
            PointF p3 = new PointF(face.Vertex3.Position.x * _TEXTURE_TILE_SIZE - polygon.BoundingBox.X, face.Vertex3.Position.y * _TEXTURE_TILE_SIZE - polygon.BoundingBox.Y);
            return rockSource.Crop(polygon.BoundingBox).Multiply(new ComputedSource(polygon.BoundingBox.Width, polygon.BoundingBox.Height,
                (x, y) =>
                {
                    return mix(x, y, p1, p2, p3, v1, v2, v3);
                }));
        }

        private static PixelColor mix(int x, int y, PointF p1, PointF p2, PointF p3, int v1, int v2, int v3)
        {
            double d1 = Distance(x, y, p1);
            double d2 = Distance(x, y, p2);
            double d3 = Distance(x, y, p3);
            var totalDistance = (d1 + d2 + d3);
            var pr1 = d1/totalDistance;
            var pr2 = d2/totalDistance;
            var pr3 = d3/totalDistance;
            var alpha = 1-System.Math.Max(0, pr1*v1 + pr2*v2 + pr3*v3);
            return PixelColor.FromFGrayscale(alpha, 1);
        }

        private static double Distance(int x, int y, PointF p1)
        {
            return System.Math.Sqrt((p1.X - x)*(p1.X - x) + (p1.Y - y)*(p1.Y - y));
        }

        private static bool IsSteep(WorldObject.Vertex3D v)
        {
            int total = 0;
            int steep = 0;
            foreach (var face in v.Faces)
            {
                total += 1;
                if (IsSteep(face))
                    steep += 1;
            }
            return steep * 2 > total;
        }

        private Point[] ToTexturePoints(WorldObject.Face face)
        {
            return new[] { ToTexturePoint(face.Vertex1.Position), ToTexturePoint(face.Vertex2.Position), ToTexturePoint(face.Vertex3.Position) };
        }

        private Point ToTexturePoint(Vector3 position)
        {
            return new Point((int)(position.x * _TEXTURE_TILE_SIZE), (int)(position.y * _TEXTURE_TILE_SIZE));
        }

        private static bool IsSteep(WorldObject.Face face)
        {
            return face.SteepLevel > 0.035f;
        }

        private bool ShouldBeSteep(WorldObject.Face face)
        {
            int total = 0;
            int steep = 0;
            foreach (var f in face.Vertex1.Faces.Union(face.Vertex2.Faces).Union(face.Vertex3.Faces))
            {
                total += 1;
                if (IsSteep(f))
                    steep += 1;
            }
            return _random.Next(total) < steep;
        }
    }
}