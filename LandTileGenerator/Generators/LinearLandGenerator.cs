using System;
using LandTileGenerator.Generators.Heights;

namespace LandTileGenerator.Generators
{
    public class LinearLandGenerator:TileGenerator
    {
        public LinearLandGenerator(Random rand) 
            : base(new HeightGeneratorSequence(
                       new LinearHeightGenerator(),
                       new SinusoidalHeightShiftGenerator(),
                       new HeightDistortionGenerator(rand, 0.006f)))
        {
        }
    }
}