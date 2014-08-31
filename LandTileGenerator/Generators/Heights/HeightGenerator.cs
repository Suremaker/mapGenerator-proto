namespace LandTileGenerator.Generators.Heights
{
    public abstract class HeightGenerator : IHeightGenerator
    {
        public abstract float Generate(float x, float y, float precomputedHeight);

        public virtual void PrepareForNewObject(int globalX,int globalY)
        {
        }
    }
}