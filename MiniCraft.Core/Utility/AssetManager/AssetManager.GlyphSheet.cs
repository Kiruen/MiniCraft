using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core
{
    public static partial class AssetManager
    {
        public static Bitmap[] GetGlyphSheets()
        {
            return Directory.GetFiles(GetGlyphSheetPath())
                     .Where(f => f.Contains("glyph_"))
                     .OrderBy(f => int.Parse(f.Replace(".png", "").Split('_')[1]))
                     .Select(f => new Bitmap(f)).ToArray();
        }
    }
}
