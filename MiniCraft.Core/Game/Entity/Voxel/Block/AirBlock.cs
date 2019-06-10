using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class AirBlock : Block
    {
        public AirBlock()
        {
            Physics.State = PhysicsStates.Gas;
            Visible = false;
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();
        }
    }
}
