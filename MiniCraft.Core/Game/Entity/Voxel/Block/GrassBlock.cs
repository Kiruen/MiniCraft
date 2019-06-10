using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class GrassBlock : Block
    {
        public vec4 FaceColor { get; set; } = new vec4(0.3f, 1.2f, 0.5f, 1);

        public GrassBlock()
        {

        }

        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            //arg.EnableRendering = false;
            base.DoRenderSelf(arg, false);
            RenderMethod method = RenderUnit.Methods[arg.MethodName];
            ShaderProgram program = method.Program;

            if(arg.MethodName == RenderStrategy.STD_TEX_COLOR)
            {
                program.SetUniform(u_modelMat, GetModelMatrixFromParent());
                program.SetUniform(u_texture0, Texture);
                program.SetUniform(u_filter_on, true);
                program.SetUniform(u_main_color, new vec4(0, 1, 0, 1));
            }
            else if (arg.MethodName == RenderStrategy.INSTANCING)
            {
                program.SetUniform(u_main_color, FaceColor);
                program.SetUniform(u_filter_on, true);
            }

            method.Render();
            return method;
        }


        public override void RenderAfter(MRenderEventArgs arg)
        {
        }

        protected override void DoInitRenderParams()
        {
            base.DoInitRenderParams();
            //RenderUnit.Mesh.Triangles[]
        }

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            unit.Mesh.UVs = MeshBase.GenSplitUV(new vec2(3, 1), 1, 1, 2, 1, 1, 0);
            return unit;
        }
    }
}
