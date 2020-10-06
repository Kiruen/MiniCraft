﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    /// <summary>
    /// Root control in control tree.
    /// </summary>
    public abstract partial class MCtrlRoot : MControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MCtrlRoot(int width, int height)
            : base(GUIAnchorStyles.Left | GUIAnchorStyles.Bottom | GUIAnchorStyles.Right | GUIAnchorStyles.Top)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        public abstract void Bind(IGLCanvas canvas);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Unbind();

    }
}