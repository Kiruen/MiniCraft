using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core
{
    public partial class CompoundForm : Form
    {
        public CompoundForm()
        {
            InitializeComponent();
            TopLevel = false;
        }

        public void Switch()
        {
            Visible = !Visible;
            if (Visible)
            {
                this.BringToFront();
                Cursor.Show();
            }
            else
                Cursor.Hide();
        }

        private void CompoundForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Switch();
        }
    }
}
