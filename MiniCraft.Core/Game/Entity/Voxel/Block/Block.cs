using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class Block : VoxelBase
    {
        public Block()
        {
            this.ModelSize = new vec3(1, 1, 1);
        }

        protected override void InitEventHandlers()
        {
            base.InitEventHandlers();
            OnDestroyEvent += Block_OnDestroyEvent;
        }

        private void Block_OnDestroyEvent(GameObject sender, MEventArgs args)
        {
            var origpos = GlobalPosition;
            for (int i = 0; i <= 4; i++)
            {
                var gpos = origpos + faceCheckSeq[i];
                var chunk = World[gpos];
                //若访问到的方块所在的区块未加载，则忽略此方块
                if (chunk.Temporary == true) continue;
                var voxel = chunk[gpos];
                if (voxel is Water water)
                {
                    water.OnFlow(null, null);
                }
            }
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        //protected override void DoInitialize()
        //{
        //    if (Texture == null)
        //    {
        //        Texture = AssetManager.GetTexture(@"stone");
        //    }
        //    base.DoInitialize();
        //}

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            unit.Mesh = new BlockMesh();
            return unit;
        }
    }
}
