using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MiniCraft.Core.Game.GameObject;
namespace MiniCraft.Core.Game
{
    public class PointMesh : MeshBase
    {
        public PointMesh()
        {
            Triangles = new uint[] { 1 };
            Vertices = new[] { new vec3(0, 0, 0) };
        }

        public override IEnumerable<VertexBuffer> GetVertexAttribute(string bufferName)
        {
            if (bufferName == strPosition)
            {
                if (this.PositionBuffer == null)
                {
                    this.PositionBuffer = Vertices.GenVertexBuffer(VBOConfig.Vec3, BufferUsage.StaticDraw);
                }
                yield return this.PositionBuffer;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public override IEnumerable<IDrawCommand> GetDrawCommand()
        {
            // indexes in GPU side.
            IndexBuffer indexBuffer = Triangles.GenIndexBuffer(BufferUsage.StaticDraw);
            if (this.commonDrawCommand == null)
            {
                this.commonDrawCommand = new DrawElementsCmd(indexBuffer, DrawMode.Points);
            }
            yield return this.commonDrawCommand;
        }
    }

    /// <summary>
    /// 透明渲染批处理结点
    /// </summary>
    public class PointSprite : GameObject
    {
        public override vec3 GlobalPosition { get; set; }

        public PointSprite(vec3 pos)
        {
            GlobalPosition = pos;
            TransparentMode = TransparentMode.Opaque;
        }

        
        #region IRenderable 成员 
        //render action会自动调用子结点的render方法！


        protected override void InitEventHandlers()
        {

        }

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = new MRenderUnit();
            var vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.SIMPLE, vertexCode);
            var fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.SIMPLE, fragmentCode);
            var provider = new ShaderArray(vs, fs);

            var map2 = new AttributeMap
            {
                { u_inPosition, BlockMesh.strPosition },
            };
            unit.AddBuilder(RenderStrategy.SIMPLE, new RenderMethodBuilder(provider, map2));
            unit.Mesh = new PointMesh();
            return unit;
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
