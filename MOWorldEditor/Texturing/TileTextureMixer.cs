using System;
using ImageOps;
using ImageOps.Sources;
using SimplexNoise;

namespace MOWorldEditor
{
    public class TileTextureMixer
    {
        private int _noiseMapSize = int.MaxValue;
        private PixelColor GetNoisePixel(int x, int y)
        {
            var amount = Noise.Generate(x*0.003f, y*0.003f)*256 + 128;
            return new PixelColor((byte)Math.Min(255,Math.Max(0,amount)),255, 255, 255);
        }

        public IPixelSource MixTextures(int width, int height, params IPixelSource[] sources)
        {
            if (sources == null || sources.Length == 0 || sources.Length > 4)
                throw new ArgumentException("Accepted source amount is 1-5");

            var source = sources[0].RepeatSource(width, height);
            source.ToBitmap().Save("source0.png");
            for (int i = 1; i < sources.Length; ++i)
            {
                var pixelSource = sources[i].RepeatSource(width + i*128, height + i*64).Crop(new PixelRectangle(i*128, i*64, width, height));
                pixelSource.ToBitmap().Save(string.Format("source{0}.png",i));
                source = source.Mix(pixelSource
                    .AddAlphaMask(GetMap(i).RepeatSource(width, height)));
            source.ToBitmap().Save(string.Format("source{0}_m.png",i));}
            return source;
        }

        private IPixelSource GetMap(int i)
        {
            var computedSource = new ComputedSource(_noiseMapSize, _noiseMapSize, (x, y) => GetNoisePixel(x + i*111, y + i*735));
            computedSource.Crop(new PixelRectangle(0,0,4096,4096)).ToBitmap().Save(string.Format("map{0}.png",i));
            return computedSource;
        }
    }
}