using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public class LeafBlock : Block
    {
        public LeafBlock(/*Chunk chunk*/) : base(/*chunk*/)
        {
            this.ModelSize = new vec3(1, 1, 1);
            this.Color = new vec4(0.5f, 1.1f, 0.5f, 1);
            this.TransparentMode = TransparentMode.SemiTransparent;
        }

        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            //arg.EnableRendering = false;
            base.DoRenderSelf(arg, false);
            RenderMethod method = RenderUnit.Methods[arg.MethodName];
            ShaderProgram program = method.Program;
            if(arg.MethodName == RenderStrategy.STD_TEX_COLOR)
            {
                program.SetUniform(u_modelMat, this.GetModelMatrixFromParent());
                program.SetUniform(u_texture0, Texture);
                program.SetUniform(u_filter_on, true);
                program.SetUniform(u_main_color, Color);
            }
            else if (arg.MethodName == RenderStrategy.INSTANCING)
            {
                //program.SetUniform(u_texture0, Texture);
                program.SetUniform(u_filter_on, true);
                program.SetUniform(u_main_color, Color);
            }

            method.Render();
            return method;
        }
    }
}
