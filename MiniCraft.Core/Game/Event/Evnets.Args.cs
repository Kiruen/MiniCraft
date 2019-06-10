using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public delegate void MBasicEventHandler(GameObject sender, MEventArgs args);
    public delegate void MVoxelEventHandler(GameObject sender, MVoxelEventArgs args);
    public delegate void MMouseEventHandler(MMouseEventArgs args);

    public class MVoxelEventArgs : MEventArgs
    {
        /// <summary>
        /// 发生事件的Voxel。若为null或者空对象，则说明是直接放置于空气中
        /// </summary>
        public VoxelBase Voxel { get; set; } = VoxelBase.None;
        /// <summary>
        /// 发生事件的Voxel的具体面
        /// </summary>
        public Direction3D Face { get; set; }
    }

    public class MEventArgs : EventArgs
    {
        /// <summary>
        /// 发生事件的chunk
        /// </summary>
        public Chunk Chunk { get; set; }
        /// <summary>
        /// 发生事件的单元格
        /// </summary>
        public ivec3 Index { get; set; }
    }

    public class MMouseEventArgs : MEventArgs
    {
        public GLMouseButtons Button { get; set; }
    }
}
