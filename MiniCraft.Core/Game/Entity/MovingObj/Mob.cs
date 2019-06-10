using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public abstract partial class Mob : GameObject
    {
        public BasicManipulater manipulater;

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        protected override void InitEventHandlers()
        {
            
        }
    }
}
