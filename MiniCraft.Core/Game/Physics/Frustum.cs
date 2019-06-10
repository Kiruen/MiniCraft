using CSharpGL;
using MiniCraft.Core.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    public class Frustum
    {
        public Pane[] panes;
        public Pane this[int i]
        {
            get { return panes[i]; }
            set { panes[i] = value; }
        }

        /// <summary>
        /// 检测一个标准的立方体是否在椎体中
        /// </summary>
        /// <param name="pos">立方体体心</param>
        /// <param name="size">立方体的半边长</param>
        /// <returns></returns>
        public bool IsCubeInFrustum(vec4 pos, vec3 size)
        {
            float x = pos.x, 
                y = pos.y,
                z = pos.z;
            for (int p = 0; p < 6; p++)
            {
                if (panes[p].A * (x - size.x) + panes[p].B * (y - size.y) + panes[p].C * (z - size.z) + panes[p].D > 0 ||
                    panes[p].A * (x + size.x) + panes[p].B * (y - size.y) + panes[p].C * (z - size.z) + panes[p].D > 0 ||
                    panes[p].A * (x - size.x) + panes[p].B * (y + size.y) + panes[p].C * (z - size.z) + panes[p].D > 0 ||
                    panes[p].A * (x + size.x) + panes[p].B * (y + size.y) + panes[p].C * (z - size.z) + panes[p].D > 0 ||
                    panes[p].A * (x - size.x) + panes[p].B * (y - size.y) + panes[p].C * (z + size.z) + panes[p].D > 0 ||
                    panes[p].A * (x + size.x) + panes[p].B * (y - size.y) + panes[p].C * (z + size.z) + panes[p].D > 0 ||
                    panes[p].A * (x - size.x) + panes[p].B * (y + size.y) + panes[p].C * (z + size.z) + panes[p].D > 0 ||
                    panes[p].A * (x + size.x) + panes[p].B * (y + size.y) + panes[p].C * (z + size.z) + panes[p].D > 0)
                    continue;
                else
                    return false;
            }
            return true;
        }

        //panes[p].A* (x - size) + panes[p].B* (y - size) + panes[p].C* (z - size) + panes[p].D > 0 ||
        //            panes[p].A* (x + size) + panes[p].B* (y - size) + panes[p].C* (z - size) + panes[p].D > 0 ||
        //            panes[p].A* (x - size) + panes[p].B* (y + size) + panes[p].C* (z - size) + panes[p].D > 0 ||
        //            panes[p].A* (x + size) + panes[p].B* (y + size) + panes[p].C* (z - size) + panes[p].D > 0 ||
        //            panes[p].A* (x - size) + panes[p].B* (y - size) + panes[p].C* (z + size) + panes[p].D > 0 ||
        //            panes[p].A* (x + size) + panes[p].B* (y - size) + panes[p].C* (z + size) + panes[p].D > 0 ||
        //            panes[p].A* (x - size) + panes[p].B* (y + size) + panes[p].C* (z + size) + panes[p].D > 0 ||
        //            panes[p].A* (x + size) + panes[p].B* (y + size) + panes[p].C* (z + size) + panes[p].D > 0)

        public bool IsAABBInFrustum(AABB box)
        {
            //var chunk = box.Parent as GameObject;
            //var m = chunk.GetModelMatrixFromParent();
            //var pos = m * new vec4(Chunk.CHUNK_X_LEN_MAX / 2, Chunk.CHUNK_Y_LEN_MAX / 2, Chunk.CHUNK_Z_LEN_MAX / 2, 1);
            //return frustum.IsCubeInFrustum(pos.ToVec3(), chunk.ModelSize.x / 2);
            var gpos = box.GetModelMatrixFromParent() * vec4.zero;//box.Parent.GetModelMatrixFromParent() * box.Center.ToVec4();
            return IsCubeInFrustum(gpos, box.ModelSize / 2);
        }
    }
}
