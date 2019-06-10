using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class LogBlock : Block
    {
        public LogBlock()
        {

        }

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            unit.Mesh.UVs = MeshBase.GenSplitUV(new vec2(2, 1), 0, 0, 1, 0, 0, 1);
            return unit;
        }
    }
}
