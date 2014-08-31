namespace LandTileGenerator.Generators.Heights
{
    public class HeightGeneratorSequence : IHeightGenerator
    {
        private readonly IHeightGenerator[] _generators;

        public HeightGeneratorSequence(params IHeightGenerator[] generators)
        {
            _generators = generators;
        }

        public float Generate(float x, float y, float precomputedHeight)
        {
            float result = precomputedHeight;
            foreach (IHeightGenerator gen in _generators)
                result = gen.Generate(x, y, result);
            return result;
        }

        public void PrepareForNewObject(int globalX, int globalY)
        {
            foreach (IHeightGenerator gen in _generators)
                gen.PrepareForNewObject(globalX, globalY);
        }
    }
}