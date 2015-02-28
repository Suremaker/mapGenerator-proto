using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LibNoise;
using LibNoise.Builder;
using LibNoise.Filter;
using LibNoise.Primitive;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var perlin = new SimplexPerlin(0, NoiseQuality.Fast);
            var filter = new HeterogeneousMultiFractal {Primitive3D = perlin,Offset = 1,OctaveCount = 4,SpectralExponent = 0.3f,Lacunarity = 2,Frequency = 4};
            var proj = new NoiseMapBuilderPlane(2, 4, 2, 4, true) {SourceModule = filter,NoiseMap = new NoiseMap()};
            proj.SetSize(1024, 1024);
            proj.CallBack += row => { };
            var bmp = new Bitmap(1024, 1024);
            float max = float.MinValue;
            float min = float.MaxValue;
            proj.Build();
            for (int i = 0; i < 1024; ++i)
                for (int j = 0; j < 1024; ++j)
            {
                //int value = (int) ((filter.GetValue(i, j))*255);
                //bmp.SetPixel(i,j,Color.FromArgb(value,value,value));
                var f = proj.NoiseMap.GetValue(i, j);
                if (f < min) min = f;
                if (f > max) max = f;
                var value = (int)((f-3)*128);
                value = Math.Min(Math.Max(0, value), 255);
                
                bmp.SetPixel(i, j, Color.FromArgb(value, value, value));
            }
            Console.WriteLine("{0}, {1}",min,max);
            bmp.Save("out.png");
        }
    }
}
