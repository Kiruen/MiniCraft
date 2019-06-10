using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public partial class Water : VoxelBase
    {
        Texture Overlay { get; set; }
        int frame = 0;

        public Water()
        {
            TransparentMode = TransparentMode.Transparent;
            Physics.State = PhysicsStates.Liquid;
            //TODO:水要比周围的方块低一些
            Scale = new vec3(1, 0.9f, 1);
            WorldPosition -= new vec3(0, 0.05f, 0);
        }

        protected override void DoInitialize()
        {
            Overlay = AssetManager.GetTexture("water_overlay");
            base.DoInitialize();
        }


        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            //arg.EnableRendering = false;
            base.DoRenderSelf(arg, false);

            var method = RenderUnit.Methods[arg.MethodName];
            var program = method.Program;

            if (arg.MethodName == RenderStrategy.STD_TEX_DYNAMIC)
                program.SetUniform(u_modelMat, this.GetModelMatrixFromParent());
            
            program.SetUniform(u_texture0, Texture);
            //program.SetUniform(u_modelMats, arg.Obj as mat4[]);
            program.SetUniform(u_uv_offset, new vec2(0, frame / 32.0f));
            frame = (frame + 1) % 32;
            
            method.Render();
            return method;
        }

        public override RenderMethod RenderOtherwhere(MRenderEventArgs arg)
        {
            RenderBefore(arg);
            var method = base.RenderOtherwhere(new MRenderEventArgs(arg, false));
            method.Render();
            RenderAfter(arg);
            return method;
        }
        
        public override void RenderBefore(MRenderEventArgs arg)
        {
            GLState.DepthMask.On();
            var blending = GLState.Blending;
            blending.SourceFactor = BlendSrcFactor.SrcAlpha;
            blending.DestFactor = BlendDestFactor.OneMinusSrcAlpha;
            blending.On();
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            GLState.DepthMask.Off();
            GLState.Blending.Off();
        }

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            var vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.STD_BLOCK_TEX);
            var fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.STD_BLOCK_TEX_DYNAMIC, fragmentCodeWithTexDynamic);
            var provider1 = new ShaderArray(vs, fs);

            vs = AssetManager.GetShader(ShaderType.VertexShader, ShaderUsage.STD_BLOCK_TEX_INSTANCING, vertexCodeWithInstancingTex);
            fs = AssetManager.GetShader(ShaderType.FragmentShader, ShaderUsage.STD_BLOCK_TEX_DYNAMIC, fragmentCodeWithTexDynamic);
            var provider2 = new ShaderArray(vs, fs);

            var map2 = new AttributeMap
            {
                { u_inPosition, BlockMesh.strPosition },
                { u_inTexCoord, BlockMesh.strTexCoord }
            };
            unit.AddBuilder(RenderStrategy.STD_TEX_DYNAMIC, new RenderMethodBuilder(provider1, map2));
            unit.AddBuilder(RenderStrategy.INSTANCING, new RenderMethodBuilder(provider2, map2));

            var mesh = new BlockMesh
            {
                UVs = MeshBase.GenSplitUV(new vec2(1, 32), Enumerable.Range(0, 6).Select(x => 0).ToArray())
            };
            unit.Mesh = mesh;

            return unit;
        }
    }
}
