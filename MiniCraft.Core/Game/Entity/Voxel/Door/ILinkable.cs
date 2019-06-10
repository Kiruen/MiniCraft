using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public abstract class MControllerBase
    {
        public List<GameObject> Parts { get; set; } = new List<GameObject>(4);
    }

    public interface ILinkable
    {
        MControllerBase Controller { get; set; }
        void OnLinkage(VoxelBase obj, MVoxelEventArgs args);
    }
}
