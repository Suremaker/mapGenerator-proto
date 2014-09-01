using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using LandTileGenerator;
using Mogre;
using Image = System.Drawing.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace MOWorldEditor
{
    public class TextureMaker
    {
        private TextureBrush _grassBrush;
        private TextureBrush _rockBrush;
        private int _MUL = 256;

        public TextureMaker()
        {
            _grassBrush = new TextureBrush(new Bitmap("Resources\\grass.png"), WrapMode.Tile);
            _rockBrush = new TextureBrush(new Bitmap("Resources\\rock.png"), WrapMode.Tile);
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
            using (var bmp = new Bitmap(map.Width * _MUL, map.Height * _MUL, PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(bmp))
            {
                for (int x = 0; x < map.Width; x++)
                    for (int y = 0; y < map.Height; ++y)
                        RenderTile(g, x, y, tileFaces.First(t => t.TileX == x && t.TileY == y).Faces);
                bmp.Save(textureName);
            }
        }

        private void RenderTile(Graphics g, int x, int y, IEnumerable<WorldObject.Face> faces)
        {
            g.FillRectangle(_grassBrush, x * _MUL, y * _MUL, _MUL, _MUL);
            foreach (var face in faces.Where(IsSteep))
                g.FillPolygon(_rockBrush, ToTexturePoints(face));
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