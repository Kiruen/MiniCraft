using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class CrossPlant : VoxelBase
    {
        public CrossPlant()
        {
            TransparentMode = TransparentMode.SemiTransparent;
            Physics.State = PhysicsStates.Fragile;
        }

        protected override void DoInitRenderParams()
        {
            base.DoInitRenderParams();
            EnableRendering = RenderingFlags.Self;
        }

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            unit.Mesh = new CrossBoardMesh();
            return unit;
        }

        //protected override void InitEventHandlers()
        //{
        //    base.InitEventHandlers();
        //    OnMouseClickedEvent += (args) =>
        //    {
        //        Chunk[GlobalPosition] = VoxelBase.None;
        //        var newpos = GlobalPosition + new vec3(0, 17, 0);
        //        var chunk = GameState.currWorld[newpos];
        //        chunk.Visible = true;
        //        chunk[newpos] = this;
        //    };
        //}

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }
}
