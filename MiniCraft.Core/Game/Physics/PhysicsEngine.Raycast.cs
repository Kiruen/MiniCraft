using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MiniCraft.Core.Game
{
    public class RaycastHit
    {
        public Direction3D Face;
        public uint[] Triangle;
        public vec3 HitPoint;
        public GameObject Obj;
    }

    public struct Ray
    {
        public vec3 Start;
        public vec3 Direction;
        public vec3 End;
        public float MaxDistance;

        /// <summary>
        /// 射线
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="dir">方向。不必是单位向量，因为将会自动单位化</param>
        /// <param name="maxDist">射线延伸的最大值，可为负数</param>
        public Ray(vec3 start, vec3 dir, float maxDist)
        {
            Start = start;
            Direction = dir.normalize();
            MaxDistance = maxDist;
            End = Start + Direction * MaxDistance;
        }

        public Ray(vec3 start, vec3 end)
        {
            Start = start;
            var rawDir = end - start;
            Direction = rawDir.normalize();
            MaxDistance = rawDir.length();
            End = end;
        }

        public vec3 Extend(float dist)
        {
            return Start + dist * Direction;
        }
    }

    public static partial class PhysicsEngine
    {
        public static RaycastHit Raycast(WorldNode world, Ray ray, float testSpan= 0.25f)
        {
            var res = new RaycastHit() { Obj = VoxelBase.None };
            
            int maxCount = Math.Max((int)ray.MaxDistance, 16);
            vec3 start = ray.Start, end = ray.End;
            vec3 checkPoint = start;
            GameObject last = null;
            //vec3 dist = world[end][end].GlobalPosition - world[start][start].GlobalPosition;
            for (int i = 0; i < maxCount; i++)
            {
                checkPoint = ray.Extend(testSpan * i); // new vec3(0.5f);
                var chunk = world[checkPoint];
                //如果是空区块，则跳过
                if (chunk.Temporary || !chunk.Visible)
                    continue;
                var obj = chunk[checkPoint];
                //TODO:解决多线程环境下空区块单例被修改的问题
                //TODO:应付紧急情况用的，之后要删掉！
                //if (obj == null) return res;
                //TODO:对任意包围盒都可以进行检测
                //TODO:支持对非体素进行检测
                if (last != obj)
                {
                    if (obj.Visible && 
                        IntersectAABB(ray, obj.Physics.Wrapper as AABB, 
                        out res.Triangle, out res.HitPoint))
                    {
                        res.Obj = obj;
                        //TODO:
                        res.Face = (Direction3D)(res.Triangle[0] / 4 + 1);
                        return res;
                    }
                }
                last = obj;
            }
            return res;
        }
    }
}
