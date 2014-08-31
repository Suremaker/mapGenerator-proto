namespace LandTileGenerator
{
    public class Mesh
    {
        private readonly Vertex[,] _vertexes;

        public Mesh(Vertex[,] vertexes)
        {
            _vertexes = vertexes;
        }
        public int Rows { get { return _vertexes.GetLength(0); } }
        public int Cols { get { return _vertexes.GetLength(1); } }
        public Vertex this[int col, int row] { get { return _vertexes[col, row]; } }
    }
}