using CSharpGL;
using fNbt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{ 
    /// <summary>
    /// 区块加载器，可以绑定不同的提供器，以便从不同的数据源获取chunk数据
    /// </summary>
    public static class ChunkLoader
    {
        public static IChunkProvider Provider { get; set; }

        public static Chunk Load(WorldNode world, ivec3 index)
        {
            var chunk = Provider.Fetch(world, index);
            //chunk.Parent = world;
            return chunk;
        }   
    }
}
