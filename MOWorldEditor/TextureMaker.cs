using System.Drawing;
using System.Drawing.Drawing2D;
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

        public void CreateFor(string materialName, WorldMap map)
        {
            var textureName = materialName + ".png";
            WriteMapTexture(textureName, map);

            MaterialPtr material = MaterialManager.Singleton.Create(materialName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, true);
            
            material.GetTechnique(0).GetPass(0).CreateTextureUnitState(textureName);
        }

        private void WriteMapTexture(string textureName, WorldMap map)
        {
            using (var bmp = new Bitmap(map.Width*_MUL, map.Height*_MUL, PixelFormat.Format32bppArgb))
            using (var g=Graphics.FromImage(bmp))
            {
                for (int x = 0; x < map.Width; x++)
                    for (int y = 0; y < map.Height; ++y)
                        RenderTile(g, x, y, map[x, y]);
                bmp.Save(textureName);
            }
        }

        private void RenderTile(Graphics g, int x, int y, WorldTile tile)
        {
            g.FillRectangle(tile.Slope.IsFlat()?_grassBrush:_rockBrush,x*_MUL,y*_MUL,_MUL,_MUL);
        }
    }
}