﻿using CSharpGL;
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
        /// Width of this control.
        /// </summary>
        [Category(strGLControl)]
        [Description("Width of this control.")]
        public int Width
        {
            get { return this.width; }
            set
            {
                if (this.width != value)
                {
                    this.width = value;
                    MControl parent = this.parent;
                    if (parent != null)
                    {
                        this.right = parent.width - this.left - value;
                    }

                    MControl.LayoutAfterWidthChanged(this, this.Children);
                }
            }
        }

        private static void LayoutAfterWidthChanged(MControl parent, MCtrlChildren glControlChildren)
        {
            foreach (var control in glControlChildren)
            {
                if (control.parent != parent) { throw new Exception("Parent info not matching!"); }

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
                    control.left = parent.width - control.right - control.width;
                    control.absLeft = parent.absLeft + control.left;
                    foreach (var item in control.Children)
                    {
                        UpdateAbsLeft(control, item);
                    }
                }
                else // if ((anchor & noneAnchor) == noneAnchor)
                {
                    int diff = parent.width - control.left - control.width - control.right;
                    int halfDiff = diff / 2;
                    int OtherHalfDiff = diff - halfDiff;
                    control.left += halfDiff;
                    control.absLeft = parent.absLeft + control.left;
                    control.right += OtherHalfDiff;
                    foreach (var item in control.Children)
                    {
                        UpdateAbsLeft(control, item);
                    }
                }
            }
        }

    }
}
