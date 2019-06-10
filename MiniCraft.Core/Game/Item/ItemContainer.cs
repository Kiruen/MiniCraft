using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class ItemContainer
    {
        public List<Item> Items = new List<Item>(16);

        private int selectedIndex;
        //TODO:注意这个设计可能并不好
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
               selectedIndex = (value + Items.Count) % Items.Count;
            }
        }
        public Item SelectedItem { get => Items[SelectedIndex]; }

        public ItemContainer()
        {
            foreach (var id in AssetManager.IDToType.Keys)
            {
                if (id == 0) continue;
                Items.Add(new Item() { BindingObjId = id });
            }
        }

        static Pen pen = new Pen(Color.Red, 4);
        public void Render(Graphics e, Rectangle clip)
        {
            int sideLength = clip.Height;
            //int baseX = SelectedIndex < 10 ? 0 : - (SelectedIndex - 10) * sideLength;
            int index_right_most = SelectedIndex > 10 ? SelectedIndex : 10;
            //int x_right_most = clip.Width - sideLength;
            for (int i = index_right_most; i >= 0; i--)
            {
                //int x = baseX + i * sideLength;
                int x = clip.Width - sideLength * (index_right_most - i + 1);
                e.DrawImage(AssetManager.GetImage(AssetManager.GetObjName(Items[i].BindingObjId)), x, 0, sideLength, sideLength);
                e.DrawString(Items[i].Count.ToString(), SystemFonts.DefaultFont, Brushes.Black, x, 0);
                //Texture t; t.GetImage()
            }
            int x_frame = clip.Width - sideLength * (SelectedIndex >= 10 ? 1 : 11 - SelectedIndex);
            e.DrawRectangle(pen, x_frame, 0, sideLength, sideLength);
            //e.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.
        }
    }
}
