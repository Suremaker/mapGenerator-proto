using System.Collections.Generic;
using LandTileGenerator;
using Mogre;
using Mesh = LandTileGenerator.Mesh;

namespace MOWorldEditor
{
    public class WorldObject
    {
        public class Vertex3D
        {
            public Vertex3D(Vector3 position)
            {
                Position = position;
            }

            public Vector3 Position { get; private set; }
            public Vector2 TextureCoord { get; set; }
            public Vector3 Normals { get; set; }
        }
        public class Face
        {
            public Face(Vertex3D vertex1, Vertex3D vertex2, Vertex3D vertex3)
            {
                Vertex3 = vertex3;
                Vertex2 = vertex2;
                Vertex1 = vertex1;
                CalculateNormals();
            }

            public Vertex3D Vertex1 { get; private set; }
            public Vertex3D Vertex2 { get; private set; }
            public Vertex3D Vertex3 { get; private set; }

            private void CalculateNormals()
            {
                var dir1 = Vertex3.Position - Vertex1.Position;
                var dir2 = Vertex1.Position - Vertex2.Position;
                var normals = dir1.CrossProduct(dir2).NormalisedCopy;
                Vertex1.Normals += normals;
                Vertex2.Normals += normals;
                Vertex3.Normals += normals;
            }
        }

        private readonly WorldModel _model;
        public ManualObject Entity { get; private set; }
        private float TILE_WIDTH = 30f;
        private float TILE_HEIGHT = 30f;
        private float TILE_DEPTH = 30f;

        public WorldObject(SceneManager mgr, WorldModel model)
        {
            _model = model;
            Entity = mgr.CreateManualObject("world");

            List<Face> faces = CreateFaces(model);

            Entity.Begin("surface", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            foreach (var face in faces)
                DrawFace(face);
            Entity.End();
            new TextureMaker().CreateFor("surface", model.Map);
        }

        private List<Face> CreateFaces(WorldModel model)
        {
            IDictionary<Vector3, Vertex3D> vertexDict = new Dictionary<Vector3, Vertex3D>();
            var faces = new List<Face>();
            for (int x = 0; x < model.Map.Width; ++x)
                for (int y = 0; y < model.Map.Height; ++y)
                    faces.AddRange(CreateFaces(x, y, model, vertexDict));
            return faces;
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
            var position = new Vector3((gx + vertex.X) * TILE_WIDTH, (gy + vertex.Y) * TILE_HEIGHT, (-level + vertex.Z) * TILE_DEPTH);
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
            Entity.Position(v.Position);
            Entity.Normal(v.Normals.NormalisedCopy);
            Entity.TextureCoord(v.TextureCoord);
        }
    }
}