using System.Drawing;
using LandTileGenerator;
using Mogre;
using Mesh = LandTileGenerator.Mesh;

namespace MOWorldEditor
{
    public class WorldObject
    {
        private readonly WorldModel _model;
        public ManualObject Entity { get; private set; }
        private float TILE_WIDTH = 30f;
        private float TILE_HEIGHT = 30f;
        private float TILE_DEPTH = 30f;

        public WorldObject(SceneManager mgr, WorldModel model)
        {
            _model = model;
            Entity = mgr.CreateManualObject("world");

            Entity.Begin("surface", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            for (int x = 0; x < model.Map.Width; ++x)
                for (int y = 0; y < model.Map.Height; ++y)
                    InitializeTile(x, y, model);
            Entity.End();
            new TextureMaker().CreateFor("surface", model.Map);
        }

        private void InitializeTile(int x, int y, WorldModel model)
        {
            var mapTile = model.Map[x, y];
            var mesh = model.GenerateTile(x, y, mapTile.Slope, mapTile.Level).Mesh;
            for (int c = 1; c < mesh.Cols; ++c)
                for (int r = 1; r < mesh.Rows; ++r)
                {
                    if (UseTopLeftBottomRightCut(mesh, c - 1, r - 1))
                    {
                        AddTriangle(mapTile.Level, x, y, mesh, new Point(c - 1, r - 1), new Point(c - 1, r), new Point(c, r));
                        AddTriangle(mapTile.Level, x, y, mesh, new Point(c, r), new Point(c, r - 1), new Point(c - 1, r - 1));
                    }
                    else
                    {
                        AddTriangle(mapTile.Level, x, y, mesh, new Point(c - 1, r - 1), new Point(c - 1, r), new Point(c, r - 1));
                        AddTriangle(mapTile.Level, x, y, mesh, new Point(c, r - 1), new Point(c - 1, r), new Point(c, r));
                    }
                }
        }

        private bool UseTopLeftBottomRightCut(Mesh mesh, int col, int row)
        {
            return Math.Abs(mesh[col, row].Z - mesh[col + 1, row + 1].Z) <
                   Math.Abs(mesh[col + 1, row].Z - mesh[col, row + 1].Z);
        }

        private void AddTriangle(int level, int gx, int gy, Mesh mesh, Point vertex1, Point vertex2, Point vertex3)
        {
            var normals = CalculateNormals(mesh[vertex1.X, vertex1.Y], mesh[vertex2.X, vertex2.Y], mesh[vertex3.X, vertex3.Y]);
            AddVertex(level, gx, gy, mesh[vertex1.X, vertex1.Y], normals);
            AddVertex(level, gx, gy, mesh[vertex2.X, vertex2.Y], normals);
            AddVertex(level, gx, gy, mesh[vertex3.X, vertex3.Y], normals);
        }

        private Vector3 CalculateNormals(Vertex v1, Vertex v2, Vertex v3)
        {
            var dir1 = v3.ToVector() - v1.ToVector();
            var dir2 = v1.ToVector() - v2.ToVector();
            return dir1.CrossProduct(dir2).NormalisedCopy;
        }

        private void AddVertex(int level, int gx, int gy, Vertex vertex, Vector3 normals)
        {
            Entity.Position((gx + vertex.X) * TILE_WIDTH, (gy + vertex.Y) * TILE_HEIGHT, (-level + vertex.Z) * TILE_DEPTH);
            Entity.Normal(normals);
            Entity.TextureCoord((gx + vertex.X) / _model.Map.Width, (gy + vertex.Y) / _model.Map.Height);
        }
    }
}