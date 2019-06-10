using CSharpGL;
using IrrKlang;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core.Game
{
    public static partial class GameState
    {
        public static void InitializEventHandlers()
        {
            if (canvas == null)
                throw new Exception("Canvas' initialization fails!");

            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.KeyDown += Canvas_KeyDown;
            canvas.KeyUp += Canvas_KeyUp;
            canvas.MouseWheel += Canvas_MouseWheel;
        }

        private static void Canvas_MouseWheel(object sender, GLMouseEventArgs e)
        {
            Input.MouseWheel(e);
        }

        private static void Canvas_KeyUp(object sender, GLKeyEventArgs e)
        {
            Input.KeyUp(e);
        }

        private static void Canvas_KeyDown(object sender, GLKeyEventArgs e)
        {
            Input.KeyDown(e);
        }

        private static void Canvas_MouseUp(object sender, GLMouseEventArgs e)
        {
            Input.MouseUp(e);
        }

        private static void Canvas_MouseDown(object sender, GLMouseEventArgs e)
        {
            Input.MouseDown(e);
        }
    }
}
