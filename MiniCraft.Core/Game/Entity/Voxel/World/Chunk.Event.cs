using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static MiniCraft.Core.Constant;

namespace MiniCraft.Core.Game
{
    public partial class Chunk
    {
        protected override void InitEventHandlers()
        {
            OnObjDestroyEvent += Chunk_OnObjDestroyEvent;
        }

        private void Chunk_OnObjDestroyEvent(GameObject sender, MEventArgs args)
        {
            if(sender is VoxelBase voxel)
                this[voxel.Index] = VoxelBase.None;
            RenderBatch.Pop(sender);
        }
    }
}
