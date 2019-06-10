using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class GlassBlock : Block
    {
        public GlassBlock(/*Chunk chunk*/) : base(/*chunk*/)
        {
            TransparentMode = TransparentMode.Transparent;
        }
    }
}
