using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MiniCraft.Core.Game.GameObject;
namespace MiniCraft.Core.Game
{
    /// <summary>
    /// 透明渲染批处理结点
    /// </summary>
    public abstract class BatchNode : MNodeBase
    {
        protected IEnumerable<GameObject> objects;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public BatchNode(IEnumerable<GameObject> objects)
        {
            this.objects = objects;
        }

        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            throw new NotImplementedException();
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

    }
}
