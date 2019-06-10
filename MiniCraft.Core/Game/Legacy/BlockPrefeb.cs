//using CSharpGL;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MiniCraft.Core.Game
//{
//    public class BlockPrefeb : VoxelBase
//    {
//        private ThreeFlags enableRendering = ThreeFlags.BeforeChildren | ThreeFlags.Children | ThreeFlags.AfterChildren;
//        /// <summary>
//        /// Render before/after children? Render children? 
//        /// RenderAction cares about this property. Other actions, maybe, maybe not, your choice.
//        /// </summary>
//        public override ThreeFlags EnableRendering
//        {
//            get { return this.enableRendering; }
//            set { this.enableRendering = value; }
//        }

//        private ModelBase model;
//        public override ModelBase Model { get => model; set => model = value; }

//        private ModernRenderUnit renderUnit;
//        public override ModernRenderUnit RenderUnit { get => renderUnit; set => renderUnit = value; }

//        protected Texture texture;

//        /// <summary>
//        /// Render propeller in modern opengl.
//        /// </summary>
//        /// <returns></returns>
//        public static BlockPrefeb Create()
//        {
//            var vs = new VertexShader(vertexCode);
//            var fs = new FragmentShader(fragmentCode);
//            var provider1 = new ShaderArray(vs, fs);

//            vs = new VertexShader(vertexCodeWithTex);
//            fs = new FragmentShader(fragmentCodeWithTex);
//            var provider2 = new ShaderArray(vs, fs);

//            vs = new VertexShader(vertexCodeWithTex);
//            fs = new FragmentShader(fragmentCodeWithTexColored);
//            var provider3 = new ShaderArray(vs, fs);

//            var map2 = new AttributeMap
//            {
//                { inPosition, BlockModel.strPosition },
//                { inTexCoord, BlockModel.strTexCoord }
//            };

//            //TODO:注意，添加的map是同一个
//            var builders = new[] {
//                //new RenderMethodBuilder(provider1, map),
//                new RenderMethodBuilder(provider2, map2),
//                new RenderMethodBuilder(provider3, map2)
//            };
//            var node = new BlockPrefeb(new BlockModel(), builders);
//            return node;
//        }
        
//        private BlockPrefeb() :base(Chunk.EmptyChunk) { }
//        public BlockPrefeb(ModelBase model, params RenderMethodBuilder[] builders)
//            : this()
//        {
//            renderUnit = new ModernRenderUnit(model, builders);
//            this.ModelSize = new vec3(1, 1, 1);// prefab.ModelSize;
//            this.Color = new vec4(1, 1, 1, 1);
//            this.Initialize();
//        }

//        public override void RenderBeforeChildren(RenderEventArgs arg)
//        {
//            if (!this.IsInitialized) { this.Initialize(); }

//            ICamera camera = arg.Camera;
//            mat4 projection = camera.GetProjectionMatrix();
//            mat4 view = camera.GetViewMatrix();
//            mat4 model = this.GetModelMatrix();

//            int routine = 1;
//            RenderMethod method = null;
//            if(routine == 0)
//            {
//                method = this.RenderUnit.Methods[0];
//                ShaderProgram program = method.Program;
//                program.SetUniform(projectionMat, projection);
//                program.SetUniform(viewMat, view);
//                program.SetUniform(modelMat, model);
//                program.SetUniform(color, this.Color);
//            }
//            else if (routine == 1)
//            {
//                method = this.RenderUnit.Methods[0];
//                ShaderProgram program = method.Program;
//                program.SetUniform(projectionMat, projection);
//                program.SetUniform(viewMat, view);
//                program.SetUniform(modelMat, model);
//                program.SetUniform(texture0, texture);
//            }
//            //TODO:加入第三个模式，画出边框

//            method?.Render();
//        }

//        public override void RenderAfterChildren(RenderEventArgs arg)
//        {
//        }

//        protected override void DoInitialize()
//        {
//            this.RenderUnit.Initialize();
//        }

//        protected override void DisposeUnmanagedResources()
//        {
//            this.RenderUnit.Dispose();
//        }
//    }
//}
