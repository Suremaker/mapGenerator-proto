namespace LandTileGenerator.Generators.Heights
{
    public abstract class BorderHeightGenerator : HeightGenerator
    {
        private readonly float _ymargin;
        private readonly float _xmargin;

        protected BorderHeightGenerator(float xmargin, float ymargin)
        {
            _xmargin = xmargin;
            _ymargin = ymargin;
        }


        public sealed override float Generate(float x, float y, float precomputedHeight)
        {
            if (x > _xmargin && x + _xmargin < 1 && y > _ymargin && y + _ymargin < 1)
                return OnGenerate(x, y, precomputedHeight);
            return precomputedHeight;
        }

        protected abstract float OnGenerate(float x, float y, float precomputedHeight);

    }
}