using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class BlockMesh : MeshBase
    {
        private const float halfLength = 0.5f;
        private const float xLength = 0.5f;
        private const float yLength = 0.5f;
        private const float zLength = 0.5f;

        public BlockMesh()
        {
            Triangles = new uint[]
{
            //0, 2, 1,  1, 2, 3, // +X faces.
            //0, 1, 5,  0, 5, 4, // +Y faces.
            //0, 4, 2,  2, 4, 6, // +Z faces.
            
            //7, 6, 4,  7, 4, 5, // -X faces.
            //7, 5, 3,  3, 5, 1, // -Z faces.
            //7, 3, 2,  7, 2, 6, // -Y faces.
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
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 1), new vec2(0, 1),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 1), new vec2(0, 1),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 1), new vec2(0, 1),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 1), new vec2(0, 1),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 1), new vec2(0, 1),
                new vec2(0, 0), new vec2(1, 0), new vec2(1, 1), new vec2(0, 1),
            };
        }
    }
}

