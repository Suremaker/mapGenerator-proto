using System;
using LandTileGenerator.Generators.Heights;

namespace LandTileGenerator.Generators
{
    public class ClifsGenerator : TileGenerator
    {
        public ClifsGenerator(Random rand)
            : base(new HeightGeneratorSequence(
                new SinusoidalHeightGenerator(),
                new SinusoidalHeightShiftGenerator(),
                new HeightDistortionGenerator(rand, 0.006f),
                new VariableHeightDistortionGenerator(rand, 0.1f,0.5f,0.95f)))
        {
        }
    }
}
