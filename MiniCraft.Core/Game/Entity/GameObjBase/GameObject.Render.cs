using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public abstract partial class GameObject
    {
        //static Camera camera = 
        //    new Camera(new vec3(2), new vec3(0), new vec3(0, 1, 0), CameraType.Perspective, 10, 10);

        public virtual RenderMethod RenderOtherwhere(MRenderEventArgs arg)
        {
            arg.MethodName = RenderStrategy.STD_TEX;
            var camera = arg.Camera;
            var method = RenderSelf(arg, false);

            //TODO:让物体绕人物的center转动roll角度即可
            var p = method.Program;
            p.SetUniform(u_modelMat, glm.rotate(glm.scale(
                glm.translate(mat4.identity(), 
                camera.Position + (camera.GetFront() + camera.GetRight()).normalize() * 0.2f), new vec3(0.2f))
                , GameState.manipulater.pitch, new vec3(0, 1, 0)));
            //p.SetUniform(u_projectionMat, camera.GetProjectionMatrix());
            //p.SetUniform(u_viewMat, glm.translate(camera.GetViewMatrix());
            //p.SetUniform(u_modelMat, mat4.identity()); //glm.scale(glm.translate(mat4.identity(), new vec3(0.9f)), new vec3(0.2f))
            method.Render();
            return method;
        }

        protected bool usePrefab = true;
        protected static Dictionary<Type, MRenderUnit> sharedRenderUnits = new Dictionary<Type, MRenderUnit>(16);
        /// <summary>
        /// 创建、初始化如RenderUnit这类的渲染相关的参数、对象。Gameobj提供了一个可缓存的框架，我们可以直接覆写CreateRenderUnit、CreateRenderBuilders
        /// </summary>
        /// <returns></returns>
        protected virtual void DoInitRenderParams()
        {
            EnableRendering = RenderingFlags.Self;
            var thisType = GetType();
            MRenderUnit sharedRenderUnit = null;
            if (!usePrefab || usePrefab && !sharedRenderUnits.TryGetValue(thisType, out sharedRenderUnit))
            {
                RenderUnit = this.InitRenderUnit(); //new MRenderUnit(new BlockMesh(), builders)
                if (sharedRenderUnit == null)
                {
                    sharedRenderUnit = RenderUnit;
                    sharedRenderUnits[thisType] = sharedRenderUnit;
                }
            }
            else if (usePrefab)
                RenderUnit = sharedRenderUnit;
        }



        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            ICamera camera = arg.Camera;
            mat4 projection = camera.GetProjectionMatrix();
            mat4 view = camera.GetViewMatrix();
            mat4 model = this.GetModelMatrixFromParent();

            RenderMethod method = RenderUnit.Methods[arg.MethodName];
            ShaderProgram program = method.Program;
            program.SetUniform(u_projectionMat, projection);
            program.SetUniform(u_viewMat, view);

            if (arg.MethodName == RenderStrategy.SIMPLE)
            {
                program.SetUniform(u_modelMat, model);
                program.SetUniform(u_color, this.Color);
            }
            else if (arg.MethodName == RenderStrategy.STD_TEX)
            {
                program.SetUniform(u_modelMat, model);
                program.SetUniform(u_texture0, Texture);
                //program.SetUniform("wire_frame_mode", false);
            }
            else if (arg.MethodName == RenderStrategy.INSTANCING)
            {
                //UseInstancingArray(method, arg.Obj as mat4[]);
                program.SetUniform(u_texture0, Texture);
                program.SetUniform(u_modelMats, arg.Obj as mat4[]);
            }
            if (doRender) //arg.EnableRendering
                method.Render();
            return method;
        }

        //VertexBuffer vbo;
        //private void UseInstancingArray(RenderMethod method, mat4[] mats)
        //{
        //    if (vbo == null)
        //    {
        //        var vao = method.VertexArrayObjects[0];
        //        (vao.DrawCommand as DrawElementsInstancedCmd).InstanceCount = mats.Length;

        //        vbo = mats.GenVertexBuffer(VBOConfig.Mat4, BufferUsage.StaticDraw);

        //        vao.Bind();
        //        vbo.Bind();
        //        GLBuffer.glEnableVertexAttribArray(2);
        //        GLBuffer.glVertexAttribPointer(2, 4, GL.GL_FLOAT, false, 16 * sizeof(float), IntPtr.Zero);

        //        GLBuffer.glEnableVertexAttribArray(3);
        //        GLBuffer.glVertexAttribPointer(3, 4, GL.GL_FLOAT, false, 16 * sizeof(float), IntPtr.Zero + 16 * sizeof(float));

        //        GLBuffer.glEnableVertexAttribArray(4);
        //        GLBuffer.glVertexAttribPointer(4, 4, GL.GL_FLOAT, false, 16 * sizeof(float), IntPtr.Zero + 32 * sizeof(float));

        //        GLBuffer.glEnableVertexAttribArray(5);
        //        GLBuffer.glVertexAttribPointer(5, 4, GL.GL_FLOAT, false, 16 * sizeof(float), IntPtr.Zero + 48 * sizeof(float));


        //        GLBuffer.glVertexAttribDivisor(2, 1); //第一个参数2表示layout索引，第二个参数指定顶点属性的更新方式
        //        GLBuffer.glVertexAttribDivisor(3, 1);
        //        GLBuffer.glVertexAttribDivisor(4, 1);
        //        GLBuffer.glVertexAttribDivisor(5, 1);

        //        vbo.Unbind();
        //        vao.Unbind();
        //    }
        //}

        /// <summary>
        /// 创建渲染此对象所需要的renderunit。通常需要自己指定mesh和builders。其中builders同样可以使用createxxx创建。
        /// 此方法通常被DoInitRenderParams自动调用，来完成缓存式创建。
        /// </summary>
        /// <param name="forInstancing"></param>
        /// <param name="builders"></param>
        /// <returns></returns>
        protected virtual MRenderUnit InitRenderUnit()
        {
            var unit = new MRenderUnit();

            var vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.SIMPLE, vertexCode);
            var fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.SIMPLE, fragmentCode);
            var provider1 = new ShaderArray(vs, fs);

            vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.STD_BLOCK_TEX, vertexCodeWithTex);
            fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.STD_BLOCK_TEX, fragmentCodeWithTex);
            var provider2 = new ShaderArray(vs, fs);

            //vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.STD_BLOCK_TEX);
            //fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.STD_BLOCK_TEX_COLOR, fragmentCodeWithTexColored);
            //var provider3 = new ShaderArray(vs, fs);

            vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.STD_BLOCK_TEX_INSTANCING, vertexCodeWithInstancingTex);
            fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.STD_BLOCK_TEX_COLOR, fragmentCodeWithTexColored);
            var provider4 = new ShaderArray(vs, fs);

            var map1= new AttributeMap
            {
                { u_inPosition, BlockMesh.strPosition },
            };

            var map2 = new AttributeMap
            {
                { u_inPosition, BlockMesh.strPosition },
                { u_inTexCoord, BlockMesh.strTexCoord }
            };
            //TODO:注意，添加的map是同一个
            unit.AddBuilder(RenderStrategy.SIMPLE, new RenderMethodBuilder(provider1, map1));
            unit.AddBuilder(RenderStrategy.STD_TEX, new RenderMethodBuilder(provider2, map2));
            //unit.AddBuilder(, new RenderMethodBuilder(provider3, map2));
            unit.AddBuilder(RenderStrategy.INSTANCING, new RenderMethodBuilder(provider4, map2));
            return unit;
        }
    }
}
