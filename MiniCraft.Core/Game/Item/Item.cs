using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class Item
    {
        //public GameObject BindingObj { get; set; }
        public int BindingObjId { get; set; }
        public int Count { get; set; }

        public VoxelBase Spawn() //WorldNode world vec3 gpos
        {
            var obj = AssetManager.GenVoxel(BindingObjId);
            //world[gpos][gpos] = obj;
            //obj.OnSettled(new MVoxelEventArgs());
            return obj;
        }
    }
}
