using System;

namespace LandTileGenerator.Generators.Heights
{
    public class SinusoidalHeightShiftGenerator : HeightGenerator
    {
        private static readonly float SIN_MUL = (float)(Math.PI * 2);
        private float _sinXShift;
        private float _sinYShift;
        private static readonly float HEIGHT_MUL = 0.005f;

        public override void PrepareForNewObject(int globalX, int globalY)
        {
            _sinXShift = globalX * SIN_MUL;
            _sinYShift = globalY * SIN_MUL;
        }

        public override float Generate(float x, float y, float precomputedHeight)
        {
            double sin = (Math.Sin(_sinXShift + SIN_MUL * x) + Math.Sin(_sinYShift + SIN_MUL * y));
            sin -= 1;
            return Math.Abs((float)(sin * HEIGHT_MUL)) + precomputedHeight;
        }
    }
}