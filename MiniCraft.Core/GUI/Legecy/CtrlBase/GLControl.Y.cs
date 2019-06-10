using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    public partial class MControl
    {
        /// <summary>
        /// 相对于Parent左下角的位置(Left Down location)
        /// </summary>
        [Category(strGLControl)]
        [Description("相对于Parent左下角的位置(Left Down location)")]
        public int Y
        {
            get { return this.bottom; }
            set
            {
                if (this.bottom != value)
                {
                    this.bottom = value;
                    MControl parent = this.parent;
                    if (parent != null)
                    {
                        //GLControl.LayoutAfterYChanged(parent, this);
                        this.top = parent.height - value - this.height;
                        MControl.UpdateAbsBottom(parent, this);
                    }
                }
            }
        }

        private static void LayoutAfterYChanged(MControl parent, MControl control)
        {
            GUIAnchorStyles anchor = control.Anchor;
            if ((anchor & topAnchor) == topAnchor)
            {
                control.Height = parent.height - control.bottom - control.top;
            }
            else
            {
                control.top = parent.height - control.bottom - control.height;
            }
        }

    }
}
