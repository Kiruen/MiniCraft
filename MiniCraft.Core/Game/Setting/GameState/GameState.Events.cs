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
        public static void OnGameExit()
        {
            AssetManager.ChunkFileManager.Save(currWorld.ChunkCache);
        }
    }
}

