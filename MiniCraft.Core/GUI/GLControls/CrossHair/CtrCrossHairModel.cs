using CSharpGL;
using MiniCraft.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    /// <summary>
    /// Renders a <see cref="GLControl"/>.
    /// </summary>
    class MCrossHairMesh : MeshBase
    {
        public MCrossHairMesh()
        {
            Vertices = new[] {
                new vec3(-1, 0, 0), new vec3(1, 0, 0),
                new vec3(0, -1, 0), new vec3(0, 1, 0),
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bufferName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IDrawCommand> GetDrawCommand()
        {
            if (this.commonDrawCommand == null)
            {
                this.commonDrawCommand = new DrawArraysCmd(DrawMode.Lines, Vertices.Length);
            }

            yield return this.commonDrawCommand;
        }
    }
}
