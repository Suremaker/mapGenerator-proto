using System.Collections.Generic;
using System.Linq;
using LandTileGenerator;
using Mogre;
using Mesh = LandTileGenerator.Mesh;

namespace MOWorldEditor
{
    public class WorldObject
    {
        public class Vertex3D
        {
            private readonly List<Face> _faces = new List<Face>();

            public Vertex3D(Vector3 position)
            {
                Position = position;
            }

            public Vector3 Position { get; private set; }
            public Vector2 TextureCoord { get; set; }
            public Vector3 Normals { get; private set; }
            public IEnumerable<Face> Faces { get { return _faces; } }
            public void AddFace(Face face)
            {
                Normals += face.Normals;
                _faces.Add(face);
            }
        }

        public class Face
        {
            public Face(Vertex3D vertex1, Vertex3D vertex2, Vertex3D vertex3)
            {
                Vertex3 = vertex3;
                Vertex2 = vertex2;
                Vertex1 = vertex1;
                Normals = CalculateNormals();
                SteepLevel = CalculateSteepLevel();
                Vertex1.AddFace(this);
                Vertex2.AddFace(this);
                Vertex3.AddFace(this);
            }

            private double CalculateSteepLevel()
            {
                var minZ = System.Math.Min(Vertex1.Position.z, System.Math.Min(Vertex2.Position.z, Vertex3.Position.z));
                var maxZ = System.Math.Max(Vertex1.Position.z, System.Math.Max(Vertex2.Position.z, Vertex3.Position.z));
                return maxZ - minZ;
            }

            public Vector3 Normals { get; private set; }
            public Vertex3D Vertex1 { get; private set; }
            public Vertex3D Vertex2 { get; private set; }
            public Vertex3D Vertex3 { get; private set; }
            public double SteepLevel { get; private set; }

            private Vector3 CalculateNormals()
            {
                var dir1 = Vertex3.Position - Vertex1.Position;
                var dir2 = Vertex1.Position - Vertex2.Position;
                return dir1.CrossProduct(dir2).NormalisedCopy;
            }
        }

        public class TileFaces
        {
            public TileFaces(int tileX, int tileY, List<Face> faces)
            {
                TileX = tileX;
                TileY = tileY;
                Faces = faces;
            }

            public int TileX { get; private set; }
            public int TileY { get; private set; }
            public List<Face> Faces { get; private set; }
        }

        private readonly WorldModel _model;
        public ManualObject Entity { get; private set; }
        private static float TILE_WIDTH = 30f;
        private static float TILE_HEIGHT = 30f;
        private static float TILE_DEPTH = 30f;

        public WorldObject(SceneManager mgr, WorldModel model)
        {
            _model = model;
            Entity = mgr.CreateManualObject("world");

            List<TileFaces> tileFaces = CreateFaces(model);

            Entity.Begin("surface", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            foreach (var tile in tileFaces.SelectMany(t => t.Faces))
                DrawFace(tile);
            Entity.End();
            new TextureMaker().CreateFor("surface", model.Map, tileFaces);
        }

        private List<TileFaces> CreateFaces(WorldModel model)
        {
            IDictionary<Vector3, Vertex3D> vertexDict = new Dictionary<Vector3, Vertex3D>();
            var faces = new List<TileFaces>();
            for (int x = 0; x < model.Map.Width; ++x)
                for (int y = 0; y < model.Map.Height; ++y)
                    faces.Add(CreateTileFaces(x, y, model, vertexDict));
            return faces;
        }

        private TileFaces CreateTileFaces(int x, int y, WorldModel model, IDictionary<Vector3, Vertex3D> vertexDict)
        {
            return new TileFaces(x, y, CreateFaces(x, y, model, vertexDict).ToList());
        }

        private IEnumerable<Face> CreateFaces(int x, int y, WorldModel model, IDictionary<Vector3, Vertex3D> vertexDict)
        {
            var mapTile = model.Map[x, y];
            var mesh = model.GenerateTile(x, y, mapTile.Slope, mapTile.Level).Mesh;
            for (int c = 1; c < mesh.Cols; ++c)
                for (int r = 1; r < mesh.Rows; ++r)
                {
                    if (UseTopLeftBottomRightCut(mesh, c - 1, r - 1))
                    {
                        yield return CreateFace(mapTile.Level, x, y, mesh[c - 1, r - 1], mesh[c - 1, r], mesh[c, r], vertexDict);
                        yield return CreateFace(mapTile.Level, x, y, mesh[c, r], mesh[c, r - 1], mesh[c - 1, r - 1], vertexDict);
                    }
                    else
                    {
                        yield return CreateFace(mapTile.Level, x, y, mesh[c - 1, r - 1], mesh[c - 1, r], mesh[c, r - 1], vertexDict);
                        yield return CreateFace(mapTile.Level, x, y, mesh[c, r - 1], mesh[c - 1, r], mesh[c, r], vertexDict);
                    }
                }
        }

        private Face CreateFace(int level, int gx, int gy, Vertex v1, Vertex v2, Vertex v3, IDictionary<Vector3, Vertex3D> vertexDict)
        {
            return new Face(
                GetOrCreateVertex(vertexDict, v1, level, gx, gy),
                GetOrCreateVertex(vertexDict, v2, level, gx, gy),
                GetOrCreateVertex(vertexDict, v3, level, gx, gy));
        }

        private Vertex3D GetOrCreateVertex(IDictionary<Vector3, Vertex3D> vertexDict, Vertex vertex, int level, int gx, int gy)
        {
            var position = new Vector3(gx + vertex.X, gy + vertex.Y, -level + vertex.Z);
            Vertex3D v3d;
            if (!vertexDict.TryGetValue(position, out v3d))
            {
                var uv = new Vector2((gx + vertex.X) / _model.Map.Width, (gy + vertex.Y) / _model.Map.Height);
                v3d = new Vertex3D(position) { TextureCoord = uv };
                vertexDict.Add(position, v3d);
            }
            return v3d;
        }

        private bool UseTopLeftBottomRightCut(Mesh mesh, int col, int row)
        {
            return Math.Abs(mesh[col, row].Z - mesh[col + 1, row + 1].Z) <
                   Math.Abs(mesh[col + 1, row].Z - mesh[col, row + 1].Z);
        }

        private void DrawFace(Face face)
        {
            DrawVertex(face.Vertex1);
            DrawVertex(face.Vertex2);
            DrawVertex(face.Vertex3);
        }

        private void DrawVertex(Vertex3D v)
        {
            Entity.Position(v.Position.x * TILE_WIDTH, v.Position.y * TILE_HEIGHT, v.Position.z * TILE_DEPTH);
            Entity.Normal(v.Normals.NormalisedCopy);
            Entity.TextureCoord(v.TextureCoord);
        }
    }
}