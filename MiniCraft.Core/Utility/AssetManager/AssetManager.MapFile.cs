using CSharpGL;
using MiniCraft.Core.Game;
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
        public static NBTChunkManager ChunkFileManager;
            
        public static void LoadMapFile(string mapName)
        {
            ChunkFileManager = new NBTChunkManager(GetNBTFilePath(mapName))
            {
                generator = GenVoxel
            };
            ChunkLoader.Provider = ChunkFileManager;
        }
    }
}
