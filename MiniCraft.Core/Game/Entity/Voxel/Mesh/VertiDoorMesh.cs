using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpGL;

namespace MiniCraft.Core.Game
{
    public class VertiDoorMesh : MeshBase
    {
        private const float xLength = 0.5f, yLength = 0.5f, zLength = 0.05f;
        private const float halfWidth = 0.5f, halfHeight = 1, halfLength = 0.05f;


            public VertiDoorMesh()
        {
            Triangles = new uint[]
{
              0,1,2, 0,2,3, // +X faces.
              4,5,6, 4,6,7, // +Y faces.
              8,9,10, 8,10,11,
              //顺时针↓
              12,13,14, 12,14,15, // -X faces.
              16,17,18, 16,18,19, // -Z faces.
              20,21,22, 20,22,23 // -Y faces.
};

            Vertices = new vec3[]
            {
            //front
            new vec3(-xLength, -yLength, +zLength),//  0
            new vec3(+xLength, -yLength, +zLength),//  1
            new vec3(+xLength, +yLength, +zLength),//  2
            new vec3(-xLength, +yLength, +zLength),//  3
            //right
            new vec3(+xLength, -yLength, +zLength),//  4
            new vec3(+xLength, -yLength, -zLength),//  5
            new vec3(+xLength, +yLength, -zLength),//  6
            new vec3(+xLength, +yLength, +zLength),//  7
            //top
            new vec3(-xLength, +yLength, +zLength),//  8
            new vec3(+xLength, +yLength, +zLength),//  9
            new vec3(+xLength, +yLength, -zLength),// 10
            new vec3(-xLength, +yLength, -zLength),// 11
            //back
            new vec3(+xLength, -yLength, -zLength),// 12
            new vec3(-xLength, -yLength, -zLength),// 13
            new vec3(-xLength, +yLength, -zLength),// 14
            new vec3(+xLength, +yLength, -zLength),// 15
            //left
            new vec3(-xLength, -yLength, -zLength),// 16
            new vec3(-xLength, -yLength, +zLength),// 17
            new vec3(-xLength, +yLength, +zLength),// 18
            new vec3(-xLength, +yLength, -zLength),// 19
            //bottom
            new vec3(+xLength, -yLength, -zLength),// 20
            new vec3(+xLength, -yLength, +zLength),// 21
            new vec3(-xLength, -yLength, +zLength),// 22
            new vec3(-xLength, -yLength, -zLength),// 23
};

            UVs = new vec2[]
            {
                //改成2而不是0.5，可以实现3/4空白效果
                new vec2(0, 1), new vec2(1, 1), new vec2(1, 0), new vec2(0, 0),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.01f), new vec2(0, 0.01f),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.01f), new vec2(0, 0.01f),
                new vec2(0, 1), new vec2(1, 1), new vec2(1, 0), new vec2(0, 0),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.01f), new vec2(0, 0.01f),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.01f), new vec2(0, 0.01f),
            };
        }

        //            public VertiDoorMesh()
        //        {
        //            Triangles = new uint[]
        //        {
        //            0,1,2, 0,2,3, 
        //            4,5,6, 4,6,7,
        //            8,9,10, 8,10,11,
        //            //顺时针↓
        //            12,13,14, 12,14,15, 
        //            16,17,18, 16,18,19, 

        //            20,21,22, 20,22,23, 
        //            24,25,26, 24,26,27, 
        //            28,29,30, 28,30,31,
        //            //顺时针↓
        //            32,33,34, 32,34,35, 
        //            36,37,38, 36,38,39, 
        //        };

        //            Vertices = new vec3[]
        //            {
        //            //每个面都是从左下角开始转
        //            //上半块
        //            //front-up
        //            new vec3(-xLength, 0, +zLength),//  0
        //            new vec3(+xLength, 0, +zLength),//  1
        //            new vec3(+xLength, +yLength, +zLength),//  2
        //            new vec3(-xLength, +yLength, +zLength),//  3
        //            //right-up
        //            new vec3(+xLength, 0, +zLength),//  4
        //            new vec3(+xLength, 0, -zLength),//  5
        //            new vec3(+xLength, +yLength, -zLength),//  6
        //            new vec3(+xLength, +yLength, +zLength),//  7
        //            //top-up
        //            new vec3(-xLength, +yLength, +zLength),//  8
        //            new vec3(+xLength, +yLength, +zLength),//  9
        //            new vec3(+xLength, +yLength, -zLength),// 10
        //            new vec3(-xLength, +yLength, -zLength),// 11
        //            //back-up
        //            new vec3(+xLength, 0, -zLength),// 12
        //            new vec3(-xLength, 0, -zLength),// 13
        //            new vec3(-xLength, +yLength, -zLength),// 14
        //            new vec3(+xLength, +yLength, -zLength),// 15
        //            //left-up
        //            new vec3(-xLength, 0, -zLength),// 16
        //            new vec3(-xLength, 0, +zLength),// 17
        //            new vec3(-xLength, +yLength, +zLength),// 18
        //            new vec3(-xLength, +yLength, -zLength),// 19
        //            //下半块
        //            //front-down
        //            new vec3(-xLength, -yLength, +zLength),//  20
        //            new vec3(+xLength, -yLength, +zLength),//  21
        //            new vec3(+xLength, 0, +zLength),//  22
        //            new vec3(-xLength, 0, +zLength),//  23
        //            //right-down
        //            new vec3(+xLength, -yLength, +zLength),//  24
        //            new vec3(+xLength, -yLength, -zLength),//  25
        //            new vec3(+xLength, 0, -zLength),//  26
        //            new vec3(+xLength, +0, +zLength),//  27
        //            //back-down
        //            new vec3(+xLength, -yLength, -zLength),// 28
        //            new vec3(-xLength, -yLength, -zLength),// 29
        //            new vec3(-xLength, +0, -zLength),// 30
        //            new vec3(+xLength, +0, -zLength),// 31
        //            //left-down
        //            new vec3(-xLength, -yLength, -zLength),// 32
        //            new vec3(-xLength, -yLength, +zLength),// 33
        //            new vec3(-xLength, +0, +zLength),// 34
        //            new vec3(-xLength, +0, -zLength),// 35
        //            //bottom-down
        //            new vec3(-xLength, -yLength, +zLength),//  36
        //            new vec3(+xLength, -yLength, +zLength),//  37
        //            new vec3(+xLength, -yLength, -zLength),// 38
        //            new vec3(-xLength, -yLength, -zLength),// 39
        //};

        //            UVs = new vec2[]
        //            {
        //                new vec2(0, 0.5f), new vec2(1, 0.5f), new vec2(1, 1), new vec2(0, 1),
        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.1f), new vec2(0, 0.1f),
        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.1f), new vec2(0, 0.1f),
        //                new vec2(0, 0.5f), new vec2(1, 0.5f), new vec2(1, 1), new vec2(0, 1),
        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.1f), new vec2(0, 0.1f),

        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.5f), new vec2(0, 0.5f),
        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.1f), new vec2(0, 0.1f),
        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.1f), new vec2(0, 0.1f),
        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.5f), new vec2(0, 0.5f),
        //                new vec2(0, 0), new vec2(1, 0), new vec2(1, 0.1f), new vec2(0, 0.1f),
        //            };
        //        }
    }
}
