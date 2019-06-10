using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public abstract class MeshBase : IBufferSource
    {
        public virtual vec3[] Vertices { get; set; }
        public virtual vec3[] Normals { get; set; }
        public virtual uint[] Triangles { get; set; }
        public virtual vec2[] UVs { get; set; }

        protected IDrawCommand commonDrawCommand;
        protected IDrawCommand instancingDrawCommand;

        #region 各种Buffer以及网格顶点数据支持的顶点属性
        public const string strPosition = "position";
        public const string strTexCoord = "texCoord";

        public VertexBuffer PositionBuffer { get; protected set; } // array in GPU side.
        public VertexBuffer UVBuffer { get; protected set; }
        public IndexBuffer IndexesBuffer { get; protected set; }
        #endregion

        public MeshBase()
        {

        }

        #region IBufferSource 成员
        public virtual IEnumerable<VertexBuffer> GetVertexAttribute(string bufferName)
        {
            if (bufferName == strPosition)
            {
                if (this.PositionBuffer == null)
                {
                    this.PositionBuffer = Vertices.GenVertexBuffer(VBOConfig.Vec3, BufferUsage.StaticDraw);
                }

                yield return this.PositionBuffer;
            }
            else if (bufferName == strTexCoord)
            {
                if (this.UVBuffer == null)
                {
                    this.UVBuffer = UVs.GenVertexBuffer(VBOConfig.Vec2, BufferUsage.StaticDraw);
                }

                yield return this.UVBuffer;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public bool InstancingMode { get; set; }
        int instanceCount = InstancingBatchNode.MAX_INSTANCE_COUNT_ONCE;
        public virtual IEnumerable<IDrawCommand> GetDrawCommand()
        {
            // indexes in GPU side.
            if (InstancingMode)
            {
                if (this.instancingDrawCommand == null)
                {
                    IndexesBuffer = Triangles.GenIndexBuffer(BufferUsage.StaticDraw);
                    this.instancingDrawCommand = new DrawElementsInstancedCmd(IndexesBuffer, DrawMode.Triangles, instanceCount);
                    //this.drawCommand = new DrawArraysCmd(DrawMode.Quads, positions.Length);
                }
                yield return this.instancingDrawCommand;
            }
            else
            {
                if (this.commonDrawCommand == null)
                {
                    IndexesBuffer = Triangles.GenIndexBuffer(BufferUsage.StaticDraw);
                    this.commonDrawCommand = new DrawElementsCmd(IndexesBuffer, DrawMode.Triangles);
                }
                yield return this.commonDrawCommand;
            }
        }
        #endregion

        /// <summary>
        /// 根据切分信息来生成UV坐标，按行主序切分，id的顺序通常是：前右上后左下
        /// </summary>
        /// <param name="max"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static vec2[] GenSplitUV(vec2 max, params int[] ids)
        {
            float _baseX = 1.0f / max.x, _baseY = 1.0f / max.y;
            vec2[] uvs = new vec2[ids.Length * 4];
            for (int i = 0; i < ids.Length; i++)
            {
                int j = (int)(ids[i] / max.x);
                uvs[4 * i + 0] = new vec2(_baseX * ids[i], 1 - _baseY * j);
                uvs[4 * i + 1] = new vec2(_baseX * (ids[i] + 1), 1 - _baseY * j);
                uvs[4 * i + 2] = new vec2(_baseX * (ids[i] + 1), 1 - _baseY * (1 + j));
                uvs[4 * i + 3] = new vec2(_baseX * ids[i], 1 - _baseY * (1 + j));
            }
            return uvs;
        }

        //public static vec2[] GenSplitUV(int max, uint id, int count)
        //{
        //    return GenSplitUV(max, Enumerable.Range(0, count).Select(x => id).ToArray());
        //}

        //int instanceCount = 1;

        //IDrawCommand lastCommand;
        //public void BeginInstancedMode(int instanceCount)
        //{
        //    this.instanceCount = instanceCount;
        //    instancingMode = true;
        //    lastCommand = this.drawCommand;
        //    this.drawCommand = new DrawArraysInstancedCmd(DrawMode.Triangles, 24, instanceCount);
        //}

        //public void EndInstancedMode()
        //{
        //    instancingMode = false;
        //    this.drawCommand = lastCommand;
        //}
    }
}
