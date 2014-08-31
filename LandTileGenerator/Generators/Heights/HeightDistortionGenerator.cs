using System;

namespace LandTileGenerator.Generators.Heights
{
    public class HeightDistortionGenerator : BorderHeightGenerator
    {
        private readonly float _maxDistortion;
        private readonly Random _rand;

        public HeightDistortionGenerator(Random rand, float maxDistortion)
            : base(0, 0)
        {
            _rand = rand;
            _maxDistortion = maxDistortion;
        }

        protected override float OnGenerate(float x, float y, float precomputedHeight)
        {
            return (float)Math.Abs(precomputedHeight + (2 * _rand.NextDouble() - 1) * _maxDistortion);
        }
    }

    public class VariableHeightDistortionGenerator : BorderHeightGenerator
    {
        private readonly float _maxDistortion;
        private readonly float _maxBegin;
        private readonly float _maxEnd;
        private readonly Random _rand;

        public VariableHeightDistortionGenerator(Random rand, float maxDistortion, float maxBegin, float maxEnd)
            : base(0, 0)
        {
            _rand = rand;
            _maxDistortion = maxDistortion;
            _maxBegin = maxBegin;
            _maxEnd = maxEnd;
        }

        protected override float OnGenerate(float x, float y, float precomputedHeight)
        {
            return (float)Math.Max(0, precomputedHeight + (2 * _rand.NextDouble() - 1) * CalcDistortion(x, y, precomputedHeight));
        }

        private double CalcDistortion(float x, float y, float precomputedHeight)
        {
            if (precomputedHeight <= _maxBegin)
                precomputedHeight = precomputedHeight / _maxBegin;
            else if (precomputedHeight > _maxEnd)
                precomputedHeight = 1 - ((precomputedHeight - _maxEnd) / (1 - _maxEnd));
            else
                precomputedHeight = 1;
            return precomputedHeight * _maxDistortion;
        }
    }
}