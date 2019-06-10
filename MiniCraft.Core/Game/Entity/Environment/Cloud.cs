//using CSharpGL;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MiniCraft.Core.Game
//{
//    public class Cloud : GameObject
//    {

//        private new static readonly string vertexCode = 
//            $@"#version 330 core

//layout(location = 0) in vec3 { u_inPosition };

//uniform mat4 { u_mvpMat };

//out vec3 passTexCoord;

//void main()
//{{
//    vec4 position = mvpMat * vec4(inPosition, 1.0); 
//    gl_Position = position.xyww;
//    passTexCoord = { u_inPosition };
//}}";

//        private new static readonly string fragmentCode = 
//            $@"#version 330 core

//    uniform sampler2D { u_texture0 };

//    in vec3 passTexCoord;

//    out vec4 color;

//    void main()
//    {{
//        color = texture({u_texture0}, passTexCoord);
//    }}";

//        public override RenderMethod PrepareNormalRendering(RenderEventArgs arg)
//        {
//            ICamera camera = arg.Camera;
//            mat4 projectionMat = camera.GetProjectionMatrix();
//            //mat4 viewMat = camera.GetViewMatrix();
//            mat4 modelMat = this.GetModelMatrix();
//            mat4 viewMat = new mat4(new mat3(camera.GetViewMatrix()));

//            RenderMethod method = this.RenderUnit.Methods[0];
//            ShaderProgram program = method.Program;
//            program.SetUniform(u_mvpMat, projectionMat * viewMat * modelMat);
//            program.SetUniform(u_texture0, Texture);

//            return method;
//        }


//        public override void RenderBeforeChildren(RenderEventArgs arg)
//        {
//            throw new NotImplementedException();
//        }

//        public override void RenderAfterChildren(RenderEventArgs arg)
//        {
//            throw new NotImplementedException();
//        }

//        protected override void DoInitialize()
//        {
//            base.DoInitialize();
//        }

//        protected override void DisposeUnmanagedResources()
//        {
//            RenderUnit.Dispose();
//        }

//        protected override void DoInitRenderParams()
//        {
//            base.DoInitRenderParams();
//            EnableRendering = RenderingFlags.Self;
//        }

//        protected override void InitEventHandlers()
//        {

//        }

//        protected override ()
//        {
//            var vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.SKY_BOX, vertexCode);
//            var fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.SKY_BOX, fragmentCode);
//            var provider = new ShaderArray(vs, fs);

//            var map = new AttributeMap();
//            map.Add(u_inPosition, SkyboxMesh.strPosition);
//            return new[] { new RenderMethodBuilder(provider, map, new CullFaceSwitch(CullFaceMode.Front)) };
//        }

//        protected override RenderMethodBuilder[] CreateInstancingRenderBuilders()
//        {
//            throw new NotImplementedException();
//        }

//        protected override MRenderUnit CreateRenderUnit(bool forInstancing, params RenderMethodBuilder[] builders)
//        {
//            return new MRenderUnit(new SkyboxMesh(), builders);
//        }

//        protected override RenderMethod PrepareInstacingRendering(RenderEventArgs arg, mat4[] modelMats, MRenderUnit instancingModeRenderUnit)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
