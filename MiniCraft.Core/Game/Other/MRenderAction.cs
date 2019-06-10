using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    /// <summary>
    /// 渲染动作指令。对指定场景中所有对象进行渲染。
    /// </summary>
    public class MRenderAction : ActionBase
    {
        private Scene scene;
        /// <summary>
        /// Render <see cref="IRenderable"/> objects.
        /// </summary>
        /// <param name="scene"></param>
        public MRenderAction(Scene scene)
        {
            if (scene == null) { throw new ArgumentNullException("Where is the scene?"); }
            this.scene = scene;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public override void Act(ActionParams param)
        {
            Scene scene = this.scene;
            var arg = new MRenderEventArgs(param, scene.Camera);
            Render(scene.RootNode, arg);
            GameState.visibleVoxelCount = GameState.visibleVoxelAcc;
            GameState.visibleVoxelAcc = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneNodeBase"></param>
        /// <param name="arg"></param>
        public static void Render(SceneNodeBase sceneNodeBase, MRenderEventArgs arg)
        {
            if (sceneNodeBase != null)
            {
                var node = sceneNodeBase as IGameRenderable;
                var isIGameRenderable = node != null;
                RenderingFlags flags = isIGameRenderable ? node.EnableRendering : RenderingFlags.None;
                bool before = isIGameRenderable && ((flags & RenderingFlags.BeforeChildren) == RenderingFlags.BeforeChildren);
                bool children = /*!isIGameRenderable || */((flags & RenderingFlags.Children) == RenderingFlags.Children);
                bool self = isIGameRenderable && ((flags & RenderingFlags.Self) == RenderingFlags.Self);
                bool after = isIGameRenderable && ((flags & RenderingFlags.AfterChildren) == RenderingFlags.AfterChildren);

                if (before)
                {
                    node.RenderBefore(arg);
                }
                if (self)
                {
                    node.RenderSelf(arg);
                }
                if (children)
                {
                    foreach (var item in sceneNodeBase.Children)
                    {
                        Render(item, arg);
                    }
                }
                if (after)
                {
                    node.RenderAfter(arg);
                }
            }
        }

    }
}
