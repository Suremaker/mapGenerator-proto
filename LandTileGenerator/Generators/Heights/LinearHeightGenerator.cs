namespace LandTileGenerator.Generators.Heights
{
    public class LinearHeightGenerator : HeightGenerator
    {
        public override float Generate(float x, float y, float height)
        {
            return height;
        }
    }
}