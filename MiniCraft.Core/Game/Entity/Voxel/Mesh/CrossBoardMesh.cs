using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class CrossBoardMesh : MeshBase
    {
        private const float xLength = 0.5f;
        private const float yLength = 0.5f;
        private const float zLength = 0.5f;

        public CrossBoardMesh()
        {
            Triangles = new uint[]
            {
                0,1,2, 0,2,3, // +X faces.
                4,5,6, 4,6,7, // +Y faces.
            };

            Vertices = new vec3[]
            {
            //TODO:遵循标准：front面的顶点统统从左下角开始逆时针，UV统统从左上角开始顺时针
            new vec3(-xLength, -yLength, -zLength),
            new vec3(+xLength, -yLength, +zLength),
            new vec3(+xLength, +yLength, +zLength),
            new vec3(-xLength, +yLength, -zLength),

            new vec3(-xLength, -yLength, +zLength),
            new vec3(+xLength, -yLength, -zLength),
            new vec3(+xLength, +yLength, -zLength),
            new vec3(-xLength, +yLength, +zLength),
};

            UVs = GenSplitUV(new vec2(1, 1), 0, 0); //Enumerable.Range(0, 2).Select(x => 0).ToArray()
        }
    }
}

