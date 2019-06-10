using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    public static class TextBoard
    {
        public static bool Visible { get; set; } = true;

        static Font font = new Font("黑体", 10, FontStyle.Bold);
        static Brush fontBrush = Brushes.Red;
        public static void Draw(Graphics g, params string[] lines)
        {
            if (!Visible) return;

            var sb = new StringBuilder();
            int lineNo = 0, baseY = 20;
            foreach (var line in lines)
            {
                //sb.Append(line);
                //sb.Append("\n");
                g.DrawString(line, font, fontBrush, 10, baseY + 20 * lineNo);
                lineNo++;
            }
            //StringFormat sf = new StringFormat
            //{
            //    Alignment = StringAlignment.Center,
            //    LineAlignment = StringAlignment.Center
            //};
            //g.DrawString(sb.ToString(), font, fontBrush, 10, 10, sf);
        }
    }
}
