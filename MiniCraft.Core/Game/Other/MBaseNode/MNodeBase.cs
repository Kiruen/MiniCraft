using CSharpGL;
using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace MiniCraft.Core
{
    /// <summary>
    /// 游戏系统在SceneNodeBase上加入的一层抽象，用于屏蔽掉一些属性的具体实现，以及添加其他的特性
    /// </summary>
    public abstract partial class MNodeBase : SceneNodeBase, IGameRenderable
    {
        public vec3 Center
        {
            get { return ModelSize / 2; }
        }

        public override SceneNodeBase Parent
        {
            get { return this.parent; }
            set
            {
                this.parent = value;
                //SceneNodeBase old = this.parent;
                //if (old != value)
                //{
                //    this.parent = value;
                //}
            }
        }

        private RenderingFlags enableRendering = RenderingFlags.BeforeChildren | RenderingFlags.Children | RenderingFlags.AfterChildren;
        /// <summary>
        /// 渲染开关，决定调用哪个(些)渲染方法
        /// </summary>
        public RenderingFlags EnableRendering
        {
            get { return enableRendering; }
            set { enableRendering = value; }
        }

        //sealed override public SceneNodeBaseChildren Children { get; }

        //render action会自动调用子结点的render方法！
        public abstract void RenderBefore(MRenderEventArgs arg);
        public virtual RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            if(!IsInitialized)
            {
                this.Initialize();
            }
            //var method = DoRenderSelf(arg);
            //method?.Render();
            return DoRenderSelf(arg, doRender);
        }

        protected abstract RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender=true);
        public abstract void RenderAfter(MRenderEventArgs arg);
    }
}