using System;

namespace LandTileGenerator.Generators.Heights
{
    public class SinusoidalHeightGenerator : HeightGenerator
    {
        public override float Generate(float x, float y, float height)
        {
            return (float)((Math.Sin(height * Math.PI - Math.PI * 0.5) + 1) * 0.5);
        }
    }
}