using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static MiniCraft.Core.Constant;

namespace MiniCraft.Core.Game
{
    /// <summary>
    /// 游戏对象所处单元格的索引，通常用于对方块等无法活动的实体的定位
    /// </summary>
    public struct GridIndex
    {
        //public static GridIndex None = new GridIndex(-1, -1, -1);
        public vec3 vec;
        public uint X { get; set; }
        public uint Y { get; set; }
        public uint Z { get; set; }

        public GridIndex(uint x, uint y, uint z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            vec = new vec3(x, y, z);
        }

        public GridIndex(int x, int y, int z) 
            : this((uint)x, (uint)y, (uint)z)
        {

        }

        public override string ToString()
        {
            return $"{X}, {Y}, {Z}";
        }
    }

    public partial class Chunk : GameObject
    {
        //注意，最好不要在初始化器中构造用到GL功能的对象，尤其是在多线程中，因为context初始化可能会发生混乱。
        private static Chunk emptyChunk;
        public static Chunk None
        {
            get
            {
                if (emptyChunk == null)
                    emptyChunk = new Chunk(0, 0, 0, null, true);
                return emptyChunk;
            }
        }

        public WorldNode World { get => Parent as WorldNode; }

        /// <summary>
        /// 表示该区块是否是临时的
        /// </summary>
        public bool Temporary { get; set; }

        public ivec3 Index { get; set; }
        public override vec3 WorldPosition
        {
            get
            {
                //此处不可能对WorldPosition赋新值，所以自然不用管那个判断是否更新的布尔量
                return worldSpacePosition;
            }
        }

        protected VoxelBase[,,] voxels = new VoxelBase[CHUNK_X_LEN, CHUNK_Y_LEN, CHUNK_Z_LEN];
        public IEnumerable<VoxelBase> Voxels
        {
            get
            {
                foreach (var item in voxels)
                {
                    yield return item;
                }
            }
        }

        //protected List<GameObject> movingObjs = ;
        //public IEnumerable<VoxelBase> MovingObjs
        //{
        //    get
        //    {
        //        foreach (var item in voxels)
        //        {
        //            yield return item;
        //        }
        //    }
        //}

        /// <summary>
        /// 按标准三维栅格排列顺序获取体素流
        /// </summary>
        public IEnumerable<VoxelBase> VoxelsWithStandardOrder
        {
            get
            {
               return from y in Enumerable.Range(-CHUNK_Y_HALF_LEN, CHUNK_Y_LEN)
                    from z in Enumerable.Range(-CHUNK_Z_HALF_LEN, CHUNK_Z_LEN)
                    from x in Enumerable.Range(-CHUNK_X_HALF_LEN, CHUNK_X_LEN)
                    select this[x, y, z];
            }
        }

        //TODO:考虑动态加载的情况
        public virtual VoxelBase this[float gx, float gy, float gz]
        {
            get
            {
                int ix = (int)Floor(gx - GlobalPosition.x),
                    iy = (int)Floor(gy - GlobalPosition.y),
                    iz = (int)Floor(gz - GlobalPosition.z);
                return this[ix, iy, iz];
            }
            set
            {
                int ix = (int)Floor(gx - GlobalPosition.x),
                    iy = (int)Floor(gy - GlobalPosition.y),
                    iz = (int)Floor(gz - GlobalPosition.z);
                this[ix, iy, iz] = value;
            }
        }

        public virtual VoxelBase this[vec3 gpos]
        {
            get
            {
                return this[gpos.x, gpos.y, gpos.z];
            }
            set
            {
                this[gpos.x, gpos.y, gpos.z] = value;
            }
        }

        /// <summary>
        /// 区块内坐标，仍然以体心（chunk的(0, 0)位置）为原点。将会自动转化为数组下标。
        /// 注意，直接调用索引器只会设置一些基本的数据，而不会发生任何事件。
        /// </summary>
        /// <returns></returns>
        public virtual VoxelBase this[int ix, int iy, int iz]
        {
            get
            {
                ix += CHUNK_X_HALF_LEN; iy += CHUNK_Y_HALF_LEN; iz += CHUNK_Z_HALF_LEN;
                if(ix.InRange(0, CHUNK_X_LEN) && iy.InRange(0, CHUNK_Y_LEN) && iz.InRange(0, CHUNK_Z_LEN))
                    return voxels[ix, iy, iz] ?? VoxelBase.None;
                else
                    return VoxelBase.None;
            }
            set
            {
                //将相对坐标转换成数组的索引
                var vec = new ivec3(ix, iy, iz);
                ix += CHUNK_X_HALF_LEN; iy += CHUNK_Y_HALF_LEN; iz += CHUNK_Z_HALF_LEN;
                if (!(ix.InRange(0, CHUNK_X_LEN) && iy.InRange(0, CHUNK_Y_LEN) &&
                    iz.InRange(0, CHUNK_Z_LEN)))
                    return;
                //若之前有方块则把方块清理掉，但不触发销毁事件。通常事件只发生在游戏逻辑中
                if (!Temporary)
                {
                    var occ = voxels[ix, iy, iz];
                    if (occ != null && occ != VoxelBase.None)
                    {
                        occ.Chunk?.RenderBatch.Pop(occ);
                    }
                }
                //塞入栅格中
                voxels[ix, iy, iz] = value;
                //空气块放入后不会进行任何操作
                if (value == VoxelBase.None || value == null)
                {
                    return;
                }
                value.Index = vec;
                value.Parent = this;
                //如果不是临时区块，则执行修改操作
                if (!Temporary)
                {
                    //TODO:只有当方块可见的时候chunk才可见
                    Visible = true;
                    Modified = true;
                    //加入到渲染batch中，相当于做一个分类管理，无需渲染时分类。
                    this.RenderBatch.Push(value);
                }
            }
        }

        public virtual VoxelBase this[ivec3 index]
        {
            get
            {
                return this[index.x, index.y, index.z];
            }
            set
            {
                this[index.x, index.y, index.z] = value;
            }
        }

        /// <summary>
        /// 表示该区块是否可以跳过某些效果或者测试
        /// </summary>
        public bool Skipable
        {
            get => Temporary || !Visible;
        }

        internal IEnumerable<VoxelBase> toRender;
        //internal IEnumerable<VoxelBase> toBlend;
        //internal IEnumerable<VoxelBase> theOpaque;

        
        public Chunk(ivec3 index, WorldNode world=null, bool temporary=false)
        {
            this.Temporary = temporary;
            //this.Index = index;
            //this.Parent = parent;
            //this.WorldPosition = Utility.ChunkIndexToPos(index);
            this.Visible = false;
            this.ModelSize = new vec3(CHUNK_X_LEN, CHUNK_Y_LEN, CHUNK_Z_LEN);
            this.PutOn(world, index);
            if (!temporary)
            {
                RenderBatch = new ChunkBatchNode(this);

                toRender = from y in Enumerable.Range(-CHUNK_Y_HALF_LEN, CHUNK_Y_LEN)
                           from z in Enumerable.Range(-CHUNK_Z_HALF_LEN, CHUNK_Z_LEN)
                           from x in Enumerable.Range(-CHUNK_X_HALF_LEN, CHUNK_X_LEN)
                           select this[x, y, z];
                toRender = toRender.Where(b => b.Visible);
                //toBlend = toRender.Where(b => b.TransparentMode != TransparentMode.Opaque);
                //theOpaque = toRender.Where(b => b.TransparentMode == TransparentMode.Opaque);
            }
        }


        public Chunk(int x, int y, int z, WorldNode parent = null, bool temporary=false)
            : this(new ivec3(x, y, z), parent, temporary)
        {
            
        }

        /// <summary>
        /// 将区块放置在指定世界结点的指定索引位置
        /// </summary>
        /// <param name="world"></param>
        /// <param name="index"></param>
        public void PutOn(WorldNode world, ivec3 index)
        {
            this.Index = index;
            this.Parent = world;
            this.WorldPosition = Utility.ChunkIndexToPos(index);
        }
    }
}
