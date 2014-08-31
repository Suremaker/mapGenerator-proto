using LandTileGenerator;
using Mogre;

namespace MOWorldEditor
{
    public static class VertextExtensions
    {
        public static Vector3 ToVector(this Vertex vertex)
        {
            return new Vector3(vertex.X, vertex.Y, vertex.Z);
        }
    }
}