using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace MiniCraft.Core.Game
{

    public partial class Chunk
    {
        //区块批处理对象
        ChunkBatchNode RenderBatch;
        public override RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            return RenderBatch.RenderSelf(arg, doRender);
        }

        /*失败的优化
        public RenderMethod RenderSelf1(MRenderEventArgs arg, bool doRender = true)
        {
            var camera = arg.Camera as Camera;
            var choosing = VoxelBase.None;
            var worlds = parent.Parent as GameContainer;

            GLState.CullFace.On();
            var instancingBatch = new InstancingBatchNode(theOpaque);
            instancingBatch.RenderSelf(arg);
            GLState.CullFace.Off();

            //Random ran = new Random();
            var blendingBatch = new TransparentBatchNode(toBlend
                .OrderByDescending(b =>
                {
                    return (b.GlobalPosition - camera.position).length();
                })); //toBlend.Where(b => QueryManager.SampleRendered((uint)b.Id))
            blendingBatch.RenderSelf(arg);

            return null;
        }
        */
        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{base.ToString()} {Index} Visible:{Visible}";
        }

        protected override void DisposeUnmanagedResources()
        {
            //throw new NotImplementedException();
        }

        protected override void DoInitRenderParams()
        {
            EnableRendering = RenderingFlags.Self;
        }

        //foreach (var item in Voxels.Where(v => v.Visible))
            //{
            //    //var wrap = item.Wrapper;
            //    //QueryManager.BeginQuery(QueryTarget.SamplesPassed, (uint)item.Id);
            //    //wrap.Render(arg);
            //    //QueryManager.EndQuery(QueryTarget.SamplesPassed);
            //    if(!item.Wrapper.IsInitialized)
            //        item.Wrapper.Initialize();
            //    //if (choosing == VoxelBase.EmptyVoxel && 
            //    //    worlds.chosingChunk == this && 
            //    //    PhysicsEngine.IntersectAABB(camera.Position, camera.GetFront(), item.Wrapper as AABBBox))
            //    //{
            //    //    //GLStateManager.logicOp.OpCode = LogicOperationCode.Or;
            //    //    //GLStateManager.logicOp.On();
            //    //    item.Wrapper.Render(arg);
            //    //    //GLStateManager.logicOp.Off();
            //    //    choosing = item;
            //    //}
            //}
            //WorldNode.depthFrame.Unbind();

            //foreach (var item in theOpaque.Where(b => QueryManager.SampleRendered((uint)b.Id)))
            //{
            //    item.Render(arg);
            //}
    }
}
