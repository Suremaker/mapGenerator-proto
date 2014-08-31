namespace LandTileGenerator
{
    public struct Vertex
    {
        public Vertex(float x, float y, float z)
            : this()
        {
            Z = z;
            Y = y;
            X = x;
        }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
    }
}