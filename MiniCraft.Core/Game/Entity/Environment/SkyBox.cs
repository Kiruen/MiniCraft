using CSharpGL;
using MiniCraft.Core.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class SkyBox : GameObject
    {
        public const string u_sky = "sky";
        public const string u_time = "time";

        private new static readonly string vertexCode = 
            $@"#version 330 core

layout(location = 0) in vec3 { u_inPosition };

uniform mat4 { u_mvpMat };

out vec3 passTexCoord;

void main()
{{
    vec4 position = mvpMat * vec4(inPosition, 1.0); 
    gl_Position = position.xyww;
    passTexCoord = { u_inPosition };
}}";

        private new static readonly string fragmentCode = 
            $@"#version 330 core

    uniform samplerCube { u_sky };

    in vec3 passTexCoord;

    out vec4 color;

    void main()
    {{
        color = texture({ u_sky }, passTexCoord);
    }}";


        private static readonly string vertexCodeWithTime =
            $@"#version 330 core

layout(location = 0) in vec3 { u_inPosition };
layout(location = 1) in vec2 { u_inTexCoord };

uniform mat4 { u_mvpMat };

out vec2 { u_passTexCoord };

void main()
{{
    vec4 position = mvpMat * vec4(inPosition, 1.0); 
    gl_Position = position.xyww;
    { u_passTexCoord } = { u_inTexCoord };
}}";

        private static readonly string fragmentCodeWithTime =
            $@"#version 330 core

    uniform sampler2D { u_sky };
    uniform float { u_time };
    in vec2 { u_passTexCoord };

    void main()
    {{
        gl_FragColor = texture({ u_sky }, vec2({ u_time }, { u_passTexCoord }.t)); //uv st 
    }}";

        public static SkyBox Create()
        {
            //var totalBmp = new Bitmap(AssetManager.GetTexturePath(@"cubemaps_skybox.png"));
            //var sky = new SkyBox(totalBmp);
            var sky = new SkyBox();
            return sky;
        }

        private SkyBox()
        {
            Scale *= ushort.MaxValue;
            Roll = 0; //Yaw = 90; 
            EnableRendering = RenderingFlags.Self;
            Texture = AssetManager.GetTexture("sky", "Environment");
            
        }

        private SkyBox(Bitmap totalBmp)
        {
            //this.ModelSize = model.ModelSize;
            Scale *= ushort.MaxValue;
            EnableRendering = RenderingFlags.Self;
            Texture = GetCubemapTexture(totalBmp);
            //renderUnit = new ModernRenderUnit(model, builders);
        }

        private Texture GetCubemapTexture(Bitmap totalBmp)
        {
            var dataProvider = GetCubemapDataProvider(totalBmp);
            TexStorageBase storage = new CubemapTexImage2D(GL.GL_RGBA, totalBmp.Width / 4, totalBmp.Height / 3, GL.GL_BGRA, GL.GL_UNSIGNED_BYTE, dataProvider);
            var texture = new Texture(storage,
                new TexParameteri(TexParameter.PropertyName.TextureMagFilter, (int)GL.GL_LINEAR),
                new TexParameteri(TexParameter.PropertyName.TextureMinFilter, (int)GL.GL_LINEAR),
                new TexParameteri(TexParameter.PropertyName.TextureWrapS, (int)GL.GL_CLAMP_TO_EDGE),
                new TexParameteri(TexParameter.PropertyName.TextureWrapT, (int)GL.GL_CLAMP_TO_EDGE),
                new TexParameteri(TexParameter.PropertyName.TextureWrapR, (int)GL.GL_CLAMP_TO_EDGE));
            texture.Initialize();

            return texture;
        }

        private CubemapDataProvider GetCubemapDataProvider(Bitmap totalBmp)
        {
            int width = totalBmp.Width / 4, height = totalBmp.Height / 3;
            var top = new Bitmap(width, height);
            using (var g = Graphics.FromImage(top))
            {
                g.DrawImage(totalBmp, new Rectangle(0, 0, width, height), new Rectangle(width, 0, width, height), GraphicsUnit.Pixel);
            }
            var left = new Bitmap(width, height);
            using (var g = Graphics.FromImage(left))
            {
                g.DrawImage(totalBmp, new Rectangle(0, 0, width, height), new Rectangle(0, height, width, height), GraphicsUnit.Pixel);
            }
            var front = new Bitmap(width, height);
            using (var g = Graphics.FromImage(front))
            {
                g.DrawImage(totalBmp, new Rectangle(0, 0, width, height), new Rectangle(width, height, width, height), GraphicsUnit.Pixel);
            }
            var right = new Bitmap(width, height);
            using (var g = Graphics.FromImage(right))
            {
                g.DrawImage(totalBmp, new Rectangle(0, 0, width, height), new Rectangle(width * 2, height, width, height), GraphicsUnit.Pixel);
            }
            var back = new Bitmap(width, height);
            using (var g = Graphics.FromImage(back))
            {
                g.DrawImage(totalBmp, new Rectangle(0, 0, width, height), new Rectangle(width * 3, height, width, height), GraphicsUnit.Pixel);
            }
            var bottom = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bottom))
            {
                g.DrawImage(totalBmp, new Rectangle(0, 0, width, height), new Rectangle(width, height * 2, width, height), GraphicsUnit.Pixel);
            }

            var flip = RotateFlipType.Rotate180FlipY;
            right.RotateFlip(flip); left.RotateFlip(flip);
            top.RotateFlip(flip); bottom.RotateFlip(RotateFlipType.Rotate180FlipX);
            back.RotateFlip(flip); front.RotateFlip(flip);
#if DEBUG
            right.Save("right.png"); left.Save("left.png");
            top.Save("top.png"); bottom.Save("bottom.png");
            back.Save("back.png"); front.Save("front.png");
#endif
            var result = new CubemapDataProvider(right, left, top, bottom, back, front);
            return result;
        }

        float time = 0;
        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            //Roll += 1;
            time += 0.001f;
            time -= (int)time;
            ICamera camera = arg.Camera;
            mat4 projectionMat = camera.GetProjectionMatrix();
            //mat4 viewMat = camera.GetViewMatrix();

            //Yaw += 0.1F;
            //var model = GetModelMatrixFromParent();
            mat4 modelMat = GetModelMatrixFromParent(); // * ();glm.scale(mat4.identity(), )   //
            //去除模型矩阵的平移变换部分(第四列数据清理)
            mat4 viewMat = camera.GetViewMatrix().ClearTranslation(); //new mat4(new mat3()); .ClearTranslation()

            RenderMethod method = this.RenderUnit.Methods[RenderStrategy.DEFAULT];
            ShaderProgram program = method.Program;
            program.SetUniform(u_mvpMat, projectionMat * viewMat * modelMat); // 
            program.SetUniform(u_sky, Texture);
            program.SetUniform(u_time, time);

            method.Render();
            return method;
        }

        protected override void DisposeUnmanagedResources()
        {
            //TODO:考虑天空盒用不用prefab
            RenderUnit.Dispose();
        }

        protected override void DoInitRenderParams()
        {
            base.DoInitRenderParams();
            EnableRendering = RenderingFlags.Self;
        }

        protected override void InitEventHandlers()
        {

        }

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = new MRenderUnit();
            var vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.SKY_BOX, vertexCodeWithTime);
            var fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.SKY_BOX, fragmentCodeWithTime);
            var provider = new ShaderArray(vs, fs);

            var map = new AttributeMap()
            {
                { u_inPosition, SkyboxMesh.strPosition },
                { u_inTexCoord, SkyboxMesh.strTexCoord },
            };

            //unit.Mesh = new SkyboxMesh();
            //unit.Mesh = new BlockMesh();
            //unit.Mesh.UVs = MeshBase.GenSplitUV(new vec2(1, 4), 0, 0, 0, 0, 0, 0);
            unit.Mesh = new SphereMesh(1, 20, 40);
            //, new CullFaceSwitch(CullFaceMode.Front)
            unit.AddBuilder(new RenderMethodBuilder(provider, map));
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
    }


    class SkyboxMesh : MeshBase
    {
        public vec3 ModelSize { get; private set; }

        public SkyboxMesh()
        {
            this.ModelSize = new vec3(xLength * 2, yLength * 2, zLength * 2);
        }

        #region IBufferSource 成员

        public override IEnumerable<VertexBuffer> GetVertexAttribute(string bufferName)
        {
            if (bufferName == strPosition)
            {
                if (this.PositionBuffer == null)
                {
                    this.PositionBuffer = positions.GenVertexBuffer(VBOConfig.Vec3, BufferUsage.StaticDraw);
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
            if (this.commonDrawCommand == null)
            {
                this.commonDrawCommand = new DrawArraysCmd(DrawMode.Quads, positions.Length);
            }

            yield return this.commonDrawCommand;
        }

        #endregion

        private const float xLength = 1;
        private const float yLength = 1;
        private const float zLength = 1;
        /// <summary>
        /// six quads' vertexes.
        /// </summary>
        private static readonly vec3[] positions = new vec3[]
        {
                new vec3(-xLength, -yLength, +zLength),//  0
				new vec3(+xLength, -yLength, +zLength),//  1
				new vec3(+xLength, +yLength, +zLength),//  2
				new vec3(-xLength, +yLength, +zLength),//  3

				new vec3(+xLength, -yLength, +zLength),//  4
				new vec3(+xLength, -yLength, -zLength),//  5
				new vec3(+xLength, +yLength, -zLength),//  6
				new vec3(+xLength, +yLength, +zLength),//  7
				
				new vec3(-xLength, +yLength, +zLength),//  8
				new vec3(+xLength, +yLength, +zLength),//  9
				new vec3(+xLength, +yLength, -zLength),// 10
				new vec3(-xLength, +yLength, -zLength),// 11
				
				new vec3(+xLength, -yLength, -zLength),// 12
				new vec3(-xLength, -yLength, -zLength),// 13
				new vec3(-xLength, +yLength, -zLength),// 14
				new vec3(+xLength, +yLength, -zLength),// 15
				
				new vec3(-xLength, -yLength, -zLength),// 16
				new vec3(-xLength, -yLength, +zLength),// 17
				new vec3(-xLength, +yLength, +zLength),// 18
				new vec3(-xLength, +yLength, -zLength),// 19
				
				new vec3(+xLength, -yLength, -zLength),// 20
				new vec3(+xLength, -yLength, +zLength),// 21
				new vec3(-xLength, -yLength, +zLength),// 22
				new vec3(-xLength, -yLength, -zLength),// 23
        };

    }
}
