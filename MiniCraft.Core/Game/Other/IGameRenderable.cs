using CSharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum RenderingFlags : byte
    {
        None = 0,
        BeforeChildren = 1,
        Children = 2,
        Self = 4,
        AfterChildren = 8,
    }

    [Editor(typeof(PropertyGridEditor), typeof(UITypeEditor))]
    public interface IGameRenderable
    {
        /// <summary>
        /// 
        /// </summary>
        RenderingFlags EnableRendering { get; set; }

        /// <summary>
        /// Render something.
        /// </summary>
        /// <param name="arg"></param>
        void RenderBefore(MRenderEventArgs arg);

        /// <summary>
        /// Render something.
        /// </summary>
        /// <param name="arg"></param>
        void RenderAfter(MRenderEventArgs arg);

        /// <summary>
        /// Render something by the way one node defines.
        /// </summary>
        /// <param name="arg"></param>
        RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true);
    }
}
