using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    public static class Perlin
    {
        //持续度 
        static float persistence;
        //倍率
        static int octaves;
        static Perlin()
        {
            persistence = 0.9f;
            octaves = 4;
        }

        static double NoiseSeed(int x, int z)    // 根据(x,y)获取一个初步噪声值，种子？
        {
            int n = x + z * 57;
            n = (n << 13) ^ n;
            return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
        }

        static double SmoothedNoise(int x, int z)   //光滑噪声
        {
            double corners = (NoiseSeed(x - 1, z - 1) + NoiseSeed(x + 1, z - 1) + NoiseSeed(x - 1, z + 1) + NoiseSeed(x + 1, z + 1)) / 16;
            double sides = (NoiseSeed(x - 1, z) + NoiseSeed(x + 1, z) + NoiseSeed(x, z - 1) + NoiseSeed(x, z + 1)) / 8;
            double center = NoiseSeed(x, z) / 4;
            return corners + sides + center;
        }

        static double InterpolatedNoise(double x, double z)   // 获取插值噪声
        {
            int integer_X = (int)Math.Floor(x);
            double fractional_X = x - integer_X;
            int integer_Y = (int)Math.Floor(z);
            double fractional_Y = z - integer_Y;
            double v1 = SmoothedNoise(integer_X, integer_Y);
            double v2 = SmoothedNoise(integer_X + 1, integer_Y);
            double v3 = SmoothedNoise(integer_X, integer_Y + 1);
            double v4 = SmoothedNoise(integer_X + 1, integer_Y + 1);
            double i1 = MathExt.CosLerp(v1, v2, fractional_X); //CosLerp
            double i2 = MathExt.CosLerp(v3, v4, fractional_X);
            return MathExt.CosLerp(i1, i2, fractional_Y);
        }

        public static double PerlinNoise(float x, float z, double persistence, int octaves)    // 最终调用：根据(x,y)获得其对应的PerlinNoise值
        {
            double noise = 0;
            double p = persistence;
            int n = octaves;
            for (int i = 0; i < n; i++)
            {
                double frequency = Math.Pow(2, i);
                double amplitude = Math.Pow(p, i);
                noise += InterpolatedNoise(x * frequency, z * frequency) * amplitude;
            }

            return noise;
        }
    }

}
