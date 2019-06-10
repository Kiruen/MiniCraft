using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpGL.glm;
using static System.Math;

namespace MiniCraft.Core
{
    public static partial class MathExt
    {
        public static vec3 ToVec3(this vec4 vec)
        {
            return new vec3(vec.x, vec.y, vec.z);
        }

        public static vec4 ToVec4(this vec3 vec)
        {
            return new vec4(vec.x, vec.y, vec.z, 1);
        }


        public static mat4 ClearTranslation(this mat4 mat)
        {
            mat4 res = mat;
            res[3] = new vec4(0, 0, 0, 1);
            return res;
        }

        /// <summary>
        /// 由camera创建Frustum对象
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Frustum ExtractFrustum(this Camera camera)
        {
            Frustum frustum = new Frustum()
            {
                panes = new Pane[6]
            };

            var mat = camera.GetProjectionMatrix() * camera.GetViewMatrix();
            var clip = mat.ToArray();
            double t;

            var offset = new int[] { -0, +0, +1, 1, 2, +2 };
            var sign = new int[] { -1, +1, +1, -1, -1, +1 };
            for (int i = 0; i < 6; i++)
            {
                var pane = new Pane();
                /* Extract the numbers for the RIGHT plane */
                pane.A = clip[3] + sign[i] * clip[0 + offset[i]];
                pane.B = clip[7] + sign[i] * clip[4 + offset[i]];
                pane.C = clip[11] + sign[i] * clip[8 + offset[i]];
                pane.D = clip[15] + sign[i] * clip[12 + offset[i]];
                /* Normalize the result */
                t = Sqrt(pane.A * pane.A + pane.B * pane.B + pane.C * pane.C);
                pane.A /= t; pane.B /= t; pane.C /= t; pane.D /= t;
                frustum[i] = pane;
            }
            return frustum;
            ///* Extract the numbers for the RIGHT plane */
            //pane.A = clip[3] - clip[0]; pane.B = clip[7] - clip[4];
            //pane.C = clip[11] - clip[8]; pane.D = clip[15] - clip[12];
            ///* Normalize the result */
            //t = Sqrt(pane.A * pane.A + pane.B * pane.B + pane.C * pane.C);
            //pane.A /= t; pane.B /= t; pane.C /= t; pane.D /= t;
            //frustum[0] = pane;

            //pane = new Pane();
            ///* Extract the numbers for the LEFT plane */
            //pane.A = clip[3] + clip[0]; pane.B = clip[7] + clip[4];
            //pane.C = clip[11] + clip[8]; pane.D = clip[15] + clip[12];
            ///* Normalize the result */
            //t = Sqrt(pane.A * pane.A + pane.B * pane.B + pane.C * pane.C);
            //pane.A /= t; pane.B /= t; pane.C /= t; pane.D /= t;
            //frustum[1] = pane;


            //pane = new Pane();
            ///* Extract the numbers for the LEFT plane */
            //pane.A = clip[3] + clip[1]; pane.B = clip[7] + clip[5];
            //pane.C = clip[11] + clip[9]; pane.D = clip[15] + clip[13];
            ///* Normalize the result */
            //t = Sqrt(pane.A * pane.A + pane.B * pane.B + pane.C * pane.C);
            //pane.A /= t; pane.B /= t; pane.C /= t; pane.D /= t;
            //frustum[2] = pane;


            //pane = new Pane();
            ///* Extract the numbers for the LEFT plane */
            //pane.A = clip[3] - clip[1]; pane.B = clip[7] - clip[5];
            //pane.C = clip[11] - clip[9]; pane.D = clip[15] - clip[13];
            ///* Normalize the result */
            //t = Sqrt(pane.A * pane.A + pane.B * pane.B + pane.C * pane.C);
            //pane.A /= t; pane.B /= t; pane.C /= t; pane.D /= t;
            //frustum[3] = pane;

            //pane = new Pane();
            ///* Extract the numbers for the LEFT plane */
            //pane.A = clip[3] - clip[2]; pane.B = clip[7] - clip[6];
            //pane.C = clip[11] - clip[10]; pane.D = clip[15] - clip[14];
            ///* Normalize the result */
            //t = Sqrt(pane.A * pane.A + pane.B * pane.B + pane.C * pane.C);
            //pane.A /= t; pane.B /= t; pane.C /= t; pane.D /= t;
            //frustum[4] = pane;

            //pane = new Pane();
            ///* Extract the numbers for the LEFT plane */
            //pane.A = clip[3] + clip[2]; pane.B = clip[7] + clip[6];
            //pane.C = clip[11] + clip[10]; pane.D = clip[15] + clip[14];
            ///* Normalize the result */
            //t = Sqrt(pane.A * pane.A + pane.B * pane.B + pane.C * pane.C);
            //pane.A /= t; pane.B /= t; pane.C /= t; pane.D /= t;
            //frustum[5] = pane;
        }
    }
}
