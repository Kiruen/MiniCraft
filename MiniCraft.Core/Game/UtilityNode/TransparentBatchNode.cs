using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.Game
{
    /// <summary>
    /// 透明渲染批处理结点
    /// </summary>
    class TransparentBatchNode : BatchNode
    {
        public TransparentBatchNode(IEnumerable<GameObject> objects)
            : base(objects)
        {
            //, BlendSrcFactor source, BlendDestFactor dest
            EnableRendering = RenderingFlags.BeforeChildren | RenderingFlags.Children | RenderingFlags.AfterChildren;
        }

        public override RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            GLState.DepthMask.On();
            var blending = GLState.Blending;
            //var b = new BlendEquationSwitch();
            
            blending.SourceFactor = BlendSrcFactor.SrcAlpha;
            blending.DestFactor = BlendDestFactor.OneMinusSrcAlpha;
            GLState.Blending.On();

            var needCullface = objects.Where(x => x.TransparentMode == TransparentMode.Transparent);
            var needNotCullface = objects.Where(x => x.TransparentMode == TransparentMode.SemiTransparent); //objects.Except(needCullface)
            GLState.CullFace.On();
            InstancingBatchNode instancingBatch = new InstancingBatchNode(needCullface);
            instancingBatch.RenderSelf(arg);
            //foreach (var item in needCullface)
            //{
            //    item.RenderSelf(arg);
            //}
            GLState.CullFace.Off();

            instancingBatch = new InstancingBatchNode(needNotCullface);
            instancingBatch.RenderSelf(arg);
            //foreach (var item in needNotCullface)
            //{
            //    item.RenderSelf(arg);
            //}
            GLState.Blending.Off();
            GLState.DepthMask.Off();
            return null;
        }
    }
}
