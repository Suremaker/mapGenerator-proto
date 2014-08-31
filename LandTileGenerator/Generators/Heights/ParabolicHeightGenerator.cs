using System;

namespace LandTileGenerator.Generators.Heights
{
    public class ParabolicHeightGenerator : HeightGenerator
    {
        public override float Generate(float x, float y, float height)
        {
            height = 1 - height;
            return (float)(1 - Math.Pow(height, 2));
        }
    }
}