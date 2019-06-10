﻿using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    /// <summary>
    /// Root Control in CSharpGL's scene's GUI system.
    /// </summary>
    public class MWinCtrlRoot : MCtrlRoot
    {
        /// <summary>
        /// Gets binding canva
        /// s.
        /// </summary>
        public WinGLCanvas BindingCanvas { get; private set; }

        private readonly System.Windows.Forms.MouseEventHandler mouseMove;
        private readonly System.Windows.Forms.MouseEventHandler mouseDown;
        private readonly System.Windows.Forms.MouseEventHandler mouseUp;
        private readonly System.Windows.Forms.KeyEventHandler keyDown;
        private readonly System.Windows.Forms.KeyEventHandler keyUp;
        private readonly System.EventHandler resize;

        /// <summary>
        /// Root Control in CSharpGL's scene's GUI system.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MWinCtrlRoot(int width, int height)
            : base(width, height)
        {
            this.mouseMove = new System.Windows.Forms.MouseEventHandler(winCanvas_MouseMove);
            this.mouseDown = new System.Windows.Forms.MouseEventHandler(winCanvas_MouseDown);
            this.mouseUp = new System.Windows.Forms.MouseEventHandler(winCanvas_MouseUp);
            this.keyDown = new System.Windows.Forms.KeyEventHandler(winCanvas_KeyDown);
            this.keyUp = new System.Windows.Forms.KeyEventHandler(winCanvas_KeyUp);
            this.resize = new EventHandler(winCanvas_Resize);

            this.EnableGUIRendering = ThreeFlags.Children;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        public override void Bind(IGLCanvas canvas)
        {
            var winCanvas = canvas as WinGLCanvas;
            if (winCanvas == null) { throw new ArgumentException(); }

            winCanvas.MouseMove += mouseMove;
            winCanvas.MouseDown += mouseDown;
            winCanvas.MouseUp += mouseUp;
            winCanvas.KeyDown += keyDown;
            winCanvas.KeyUp += keyUp;
            winCanvas.Resize += resize;

            this.BindingCanvas = winCanvas;
        }

        void winCanvas_Resize(object sender, EventArgs e)
        {
            var control = sender as System.Windows.Forms.Control;
            this.Width = control.Width;
            this.Height = control.Height;
        }

        void winCanvas_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (this.currentControl != null)
            {
                GLKeyEventArgs args = e.Translate();
                this.currentControl.InvokeEvent(MEventType.KeyUp, args);
            }
        }

        void winCanvas_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (this.currentControl != null)
            {
                GLKeyEventArgs args = e.Translate();
                this.currentControl.InvokeEvent(MEventType.KeyDown, args);
            }
        }

        private MControl currentControl;

        void winCanvas_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.currentControl != null)
            {
                GLMouseEventArgs args = e.Translate();
                this.currentControl.InvokeEvent(MEventType.MouseUp, args);
            }
        }

        void winCanvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int x = e.X, y = this.BindingCanvas.Height - e.Y - 1;
            MControl control = GetControlAt(x, y, this);
            this.currentControl = control;
            if (control != null)
            {
                GLMouseEventArgs args = e.Translate();
                control.InvokeEvent(MEventType.MouseDown, args);
            }
        }

        /// <summary>
        /// Get the control at specified position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        private MControl GetControlAt(int x, int y, MControl control)
        {
            MControl result = null;
            if (control.ContainsPoint(x, y))
            {
                if (control.AcceptPicking)
                {
                    result = control;
                }

                foreach (var item in control.Children)
                {
                    MControl child = GetControlAt(x, y, item);
                    if (child != null)
                    {
                        result = child;
                        break;
                    }
                }
            }

            return result;
        }

        void winCanvas_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.currentControl != null)
            {
                GLMouseEventArgs args = e.Translate();
                this.currentControl.InvokeEvent(MEventType.MouseMove, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Unbind()
        {
            WinGLCanvas winCanvas = this.BindingCanvas;
            if (winCanvas != null)
            {
                winCanvas.MouseMove -= mouseMove;
                winCanvas.MouseDown -= mouseDown;
                winCanvas.MouseUp -= mouseUp;
                winCanvas.KeyDown -= keyDown;
                winCanvas.KeyUp -= keyUp;

                this.BindingCanvas = null;
            }
        }

        public override void RenderGUIBeforeChildren(GUIRenderEventArgs arg)
        {
        }

        public override void RenderGUIAfterChildren(GUIRenderEventArgs arg)
        {
        }
    }
}
