using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageOps;
using ImageOps.Sources;
using Mogre;

namespace MOWorldEditor
{
    public class TextureMaker
    {
        private int _MUL = 256;
        private TileTextureMixer _tileTextureMixer;

        public TextureMaker()
        {
            _tileTextureMixer = new TileTextureMixer();
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
            int txWidth = map.Width * _MUL;
            int txHeight = map.Height * _MUL;
            
            using (var rockSource = _tileTextureMixer.MixTextures(txWidth, txHeight, LoadSources("rock")))
            using (var grassSource = _tileTextureMixer.MixTextures(txWidth, txHeight, LoadSources("grass")))
            {
                var finalSource = grassSource;
                for (int x = 0; x < map.Width; x++)
                    for (int y = 0; y < map.Height; ++y)
                        finalSource = RenderTile(finalSource,rockSource, tileFaces.First(t => t.TileX == x && t.TileY == y).Faces);
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

        private IPixelSource RenderTile(IPixelSource finalSource, IPixelSource rockSource, IEnumerable<WorldObject.Face> faces)
        {
            foreach (var face in faces.Where(IsSteep))
            {
                var polygon = Regions.Polygon(ToTexturePoints(face));
                finalSource=finalSource.BlendRegion(polygon, rockSource.Crop(polygon.BoundingBox), BlendingMethods.Normal);
            }
            return finalSource;
        }

        private Point[] ToTexturePoints(WorldObject.Face face)
        {
            return new[] { ToTexturePoint(face.Vertex1.Position), ToTexturePoint(face.Vertex2.Position), ToTexturePoint(face.Vertex3.Position) };
        }

        private Point ToTexturePoint(Vector3 position)
        {
            return new Point((int)(position.x * _MUL), (int)(position.y * _MUL));
        }

        private bool IsSteep(WorldObject.Face face)
        {
            var minZ = System.Math.Min(face.Vertex1.Position.z, System.Math.Min(face.Vertex2.Position.z, face.Vertex3.Position.z));
            var maxZ = System.Math.Max(face.Vertex1.Position.z, System.Math.Max(face.Vertex2.Position.z, face.Vertex3.Position.z));
            return maxZ - minZ > 0.035f;
        }
    }
}