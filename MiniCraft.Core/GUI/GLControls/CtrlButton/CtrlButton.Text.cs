using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    public partial class MButton
    {
        private MLabel label;

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get { return this.label.Text; }
            set { this.label.Text = value; }
        }
    }
}
