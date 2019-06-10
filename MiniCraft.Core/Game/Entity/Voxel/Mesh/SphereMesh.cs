using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class SphereMesh : MeshBase
    {
        private static Random random = new Random();

        private static vec3 RandomVec3()
        {
            return new vec3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
        }

        private static readonly Func<int, int, vec3> defaultColorGenerator = new Func<int, int, vec3>(DefaultColorGenerator);

        private static vec3 DefaultColorGenerator(int latitude, int longitude)
        {
            return RandomVec3();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="latitudeParts">用纬线把地球切割为几块。</param>
        /// <param name="longitudeParts">用经线把地球切割为几块。</param>
        /// <returns></returns>
        internal SphereMesh(float radius = 1.0f, int latitudeParts = 10, int longitudeParts = 40)
        {
            if (radius <= 0.0f || latitudeParts < 2 || longitudeParts < 3) { throw new Exception(); }

            int vertexCount = (latitudeParts + 1) * (longitudeParts + 1);
            this.Vertices = new vec3[vertexCount];
            this.Normals = new vec3[vertexCount];
            this.UVs = new vec2[vertexCount];

            int indexCount = (latitudeParts) * (2 * (longitudeParts + 1) + 1);
            this.Triangles = new uint[indexCount];

            int index = 0;

            //// 把星球平铺在一个平面上
            //for (int i = 0; i < latitudeParts + 1; i++)
            //{
            //    double theta = (latitudeParts - i * 2) * Math.PI / 2 / latitudeParts;
            //    double y = radius * Math.Sin(theta);
            //    for (int j = 0; j < longitudeParts + 1; j++)
            //    {
            //        double x = radius * (i - latitudeParts / 2);
            //        double z = radius * (j - longitudeParts / 2);

            //        vec3 position = new vec3((float)x, (float)y, (float)z);
            //        this.positions[index] = position;

            //        this.normals[index] = position.normalize();

            //        this.colors[index] = colorGenerator(i, j);

            //        index++;
            //    }
            //}

            for (int i = 0; i < latitudeParts + 1; i++)
            {
                double theta = (latitudeParts - i * 2) * Math.PI / 2 / latitudeParts;
                double y = radius * Math.Sin(theta);
                for (int j = 0; j < longitudeParts + 1; j++)
                {
                    double x = radius * Math.Cos(theta) * Math.Sin(j * Math.PI * 2 / longitudeParts);
                    double z = radius * Math.Cos(theta) * Math.Cos(j * Math.PI * 2 / longitudeParts);

                    vec3 position = new vec3((float)x, (float)y, (float)z);
                    this.Vertices[index] = position;

                    this.Normals[index] = position.normalize();

                    this.UVs[index] = new vec2((float)j / (float)longitudeParts, (float)i / (float)latitudeParts);

                    index++;
                }
            }

            // 索引
            index = 0;
            for (int i = 0; i < latitudeParts; i++)
            {
                for (int j = 0; j < longitudeParts + 1; j++)
                {
                    this.Triangles[index++] = (uint)((longitudeParts + 1) * (i + 0) + j);
                    this.Triangles[index++] = (uint)((longitudeParts + 1) * (i + 1) + j);
                }
                // use
                // GL.Enable(GL.GL_PRIMITIVE_RESTART);
                // GL.PrimitiveRestartIndex(uint.MaxValue);
                // GL.Disable(GL.GL_PRIMITIVE_RESTART);
                this.Triangles[index++] = uint.MaxValue;
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
                int length = Triangles.Length;
                if (Vertices.Length < byte.MaxValue)
                {
                    IndexBuffer buffer = GetBufferInBytes(length);
                    this.commonDrawCommand = new DrawElementsCmd(buffer, DrawMode.TriangleStrip, byte.MaxValue);
                }
                else if (Vertices.Length < ushort.MaxValue)
                {
                    IndexBuffer buffer = GetBufferInUShort(length);
                    this.commonDrawCommand = new DrawElementsCmd(buffer, DrawMode.TriangleStrip, ushort.MaxValue);
                }
                else
                {
                    IndexBuffer buffer = GetBufferInUInt(length);
                    this.commonDrawCommand = new DrawElementsCmd(buffer, DrawMode.TriangleStrip, uint.MaxValue);
                }
            }

            yield return commonDrawCommand;
        }

        private IndexBuffer GetBufferInUInt(int length)
        {
            IndexBuffer buffer = GLBuffer.Create(IndexBufferElementType.UInt, length, BufferUsage.StaticDraw);
            unsafe
            {
                IntPtr pointer = buffer.MapBuffer(MapBufferAccess.WriteOnly);
                var array = (uint*)pointer;
                for (int i = 0; i < Triangles.Length; i++)
                {
                    array[i] = Triangles[i];
                }
                buffer.UnmapBuffer();
            }
            return buffer;
        }

        private IndexBuffer GetBufferInUShort(int length)
        {
            IndexBuffer buffer = GLBuffer.Create(IndexBufferElementType.UShort, length, BufferUsage.StaticDraw);
            unsafe
            {
                IntPtr pointer = buffer.MapBuffer(MapBufferAccess.WriteOnly);
                var array = (ushort*)pointer;
                for (int i = 0; i < Triangles.Length; i++)
                {
                    if (Triangles[i] == uint.MaxValue) { array[i] = ushort.MaxValue; }
                    else { array[i] = (ushort)Triangles[i]; }
                }
                buffer.UnmapBuffer();
            }
            return buffer;
        }

        private IndexBuffer GetBufferInBytes(int length)
        {
            IndexBuffer buffer = GLBuffer.Create(IndexBufferElementType.UByte, length, BufferUsage.StaticDraw);
            unsafe
            {
                IntPtr pointer = buffer.MapBuffer(MapBufferAccess.WriteOnly);
                var array = (byte*)pointer;
                for (int i = 0; i < Triangles.Length; i++)
                {
                    if (Triangles[i] == uint.MaxValue) { array[i] = byte.MaxValue; }
                    else { array[i] = (byte)Triangles[i]; }
                }
                buffer.UnmapBuffer();
            }
            return buffer;
        }
    }

}
