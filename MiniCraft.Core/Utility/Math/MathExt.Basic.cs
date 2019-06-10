using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    public static partial class MathExt
    {
        public const float PI = 3.1415926535897931f;
        public const float TwoPI = PI * 2;
        public const float HalfPI = PI / 2;
        public const float E = 2.7182818284590451f;

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) { return min; }
            if (max < value) { return max; }

            return value;
        }

        public static int Floor(float value)
        {
            return (int)Math.Floor(value);
            //if (value < 0)
            //    return (int)(value - 0.5f);
            //else
            //    return (int)value;
        }

        public static vec3 Floor(vec3 vec)
        {
            return new vec3(Floor(vec.x), Floor(vec.y), Floor(vec.z));
        }


        //TODO:尝试实现泛型版本？
        public static float Lerp(float s, float e, float t)
        {
            return (1 - t) * s + t * e;
        }

        public static double Lerp(double s, double e, double t)
        {
            return (1 - t) * s + t * e;
        }

        public static vec3 Lerp(vec3 s, vec3 e, float t)
        {
            return (1 - t) * s + t * e;
        }

        public static double CosLerp(double a, double b, double t)
        {
            double ft = t * PI;
            double f = (1 - Math.Cos(ft)) * 0.5f;
            return a * (1 - f) + b * f;
        }

        public static float CosLerp(float a, float b, float t)
        {
            float ft = t * PI;
            float f = (float)(1 - Math.Cos(ft)) * 0.5f;
            return a * (1 - f) + b * f;

            //return (float)(Math.Cos(t * HalfPI) * s + Math.Cos((1 - t) * HalfPI) * e);
        }

        public static vec3 CosLerp(vec3 a, vec3 b, float t)
        {
            float ft = t * PI;
            float f = (float)(1 - Math.Cos(ft)) * 0.5f;
            return a * (1 - f) + b * f;
            //return (float)Math.Cos(t * HalfPI) * s + (float)Math.Cos((1 - t) * HalfPI) * e;
        }

        public static float RadianToAngle(float radian)
        {
            return radian / PI * 180;
        }

        public static float AngleToRadian(float angle)
        {
            return angle / 180 * PI;
        }

        public static mat4 YPRToRotation(float roll, float pitch, float yaw)
        {
            roll = AngleToRadian(roll);
            pitch = AngleToRadian(pitch);
            yaw = AngleToRadian(yaw);
            //x
            mat4 rRoll = default(mat4);
            vec4 col0 = new vec4(1, 0, 0, 0);
            vec4 col1 = new vec4(0, (float)Math.Cos(roll), (float)Math.Sin(roll), 0);
            vec4 col2 = new vec4(0, -(float)Math.Sin(roll), (float)Math.Cos(roll), 0);
            vec4 col3 = new vec4(0, 0, 0, 1);
            rRoll[0] = col0; rRoll[1] = col1; rRoll[2] = col2; rRoll[3] = col3;

            //y
            mat4 rPitch = default(mat4);
            col0 = new vec4((float)Math.Cos(pitch), 0, -(float)Math.Sin(pitch), 0);
            col1 = new vec4(0, 1, 0, 0);
            col2 = new vec4((float)Math.Sin(pitch), 0, (float)Math.Cos(pitch), 0);
            col3 = new vec4(0, 0, 0, 1);
            rPitch[0] = col0; rPitch[1] = col1; rPitch[2] = col2; rPitch[3] = col3;

            //z
            mat4 rYaw = default(mat4);
            col0 = new vec4((float)Math.Cos(yaw), (float)Math.Sin(yaw), 0, 0);
            col1 = new vec4(-(float)Math.Sin(yaw), (float)Math.Cos(yaw), 0, 0);
            col2 = new vec4(0, 0, 1, 0);
            col3 = new vec4(0, 0, 0, 1);
            rYaw[0] = col0; rYaw[1] = col1; rYaw[2] = col2; rYaw[3] = col3;

            return rRoll* rPitch * rYaw;

        }
    }
}
