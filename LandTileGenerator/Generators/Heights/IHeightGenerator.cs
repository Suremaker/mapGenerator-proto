namespace LandTileGenerator.Generators.Heights
{
    public interface IHeightGenerator
    {
        float Generate(float x, float y, float precomputedHeight);
        void PrepareForNewObject(int globalX, int globalY);
    }
}