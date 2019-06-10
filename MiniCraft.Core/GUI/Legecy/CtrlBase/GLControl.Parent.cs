using CSharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    public partial class MControl
    {
        internal MControl parent;
        /// <summary>
        /// Parent control.
        /// </summary>
        [Category(strGLControl)]
        [Description("Parent control. This node inherits parent's layout properties.")]
        public MControl Parent
        {
            get { return this.parent; }
            set
            {
                MControl old = this.parent;
                if (old != value)
                {
                    this.parent = value; // this records the parent.

                    if (old != null)
                    {
                        old.Children.Remove(this); // bye, old parent.
                    }
                    
                    if (value != null)
                    {
                        value.Children.children.Add(this); // parent records this.

                        MControl.LayoutAfterAddChild(this, value);
                        MControl.UpdateAbsLocation(this, value);
                    }
                }
            }
        }

        internal static void LayoutAfterAddChild(MControl control, MControl parent)
        {
            GUIAnchorStyles anchor = control.Anchor;
            if ((anchor & leftRightAnchor) == leftRightAnchor)
            {
                control.Width = parent.width - control.left - control.right;
            }
            else if ((anchor & leftAnchor) == leftAnchor)
            {
                control.right = parent.width - control.left - control.width;
            }
            else if ((anchor & rightAnchor) == rightAnchor)
            {
                control.left = parent.width - control.width - control.right;
            }
            else // if ((anchor & noneAnchor) == nonAnchor)
            {
                int diff = parent.width - control.left - control.width - control.right;
                int halfDiff = diff / 2;
                int otherHalfDiff = diff - halfDiff;
                control.left += halfDiff;
                control.right += otherHalfDiff;
            }
            
            if ((anchor & bottomTopAnchor) == bottomTopAnchor)
            {
                control.Height = parent.height - control.bottom - control.top;
            }
            else if ((anchor & bottomAnchor) == bottomAnchor)
            {
                control.top = parent.height - control.bottom - control.height;
            }
            else if ((anchor & topAnchor) == topAnchor)
            {
                control.bottom = parent.height - control.height - control.top;
            }
            else // if ((anchor & noneAnchor) == nonAnchor)
            {
                int diff = parent.height - control.bottom - control.height - control.top;
                int halfDiff = diff / 2;
                int otherHalfDiff = diff - halfDiff;
                control.bottom += halfDiff;
                control.top += otherHalfDiff;
            }
        }

    }
}
