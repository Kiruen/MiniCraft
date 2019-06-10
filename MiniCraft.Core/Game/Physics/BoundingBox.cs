using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public abstract class BoundingBox : GameObject
    {
        public override vec3 ModelSize { get => Parent.ModelSize; }
        public override vec3 Scale { get => Parent.ModelSize; }

        //public GameObject Obj { get; protected set; }
        public BoundingBox(GameObject obj) : base(false)
        {
            Parent = obj;
            //this.Initialize();

            //TODO:确认-模型空间始终是以体心为原点的？？之前一直以为是以左(右?)下角为原点的
            //this.ScaleCenter = new vec3(-0.5f, -0.5f, -0.5f);
            //this.Scale = parent.ModelSize;/* * 16*/
        }

        protected override void InitEventHandlers()
        {
            
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

    public class AABB : BoundingBox
    {
        //TODO:缓存下来，无需实时计算，提高效率
        /// <summary>
        /// 包围盒左下角的全局坐标
        /// </summary>
        public vec3 Min { get => (GetModelMatrixFromParent() * Mesh.Vertices[0].ToVec4()).ToVec3(); }
        
        /// <summary>
        /// 包围盒右上角的全局坐标
        /// </summary>
        public vec3 Max { get => (GetModelMatrixFromParent() * Mesh.Vertices[15].ToVec4()).ToVec3(); }

        public AABB(GameObject obj) : base(obj)
        {
            
        }

        protected override void DoInitialize()
        {
            //if (Texture == null)
            //{
            //    Texture = AssetManager.GetTexture(@"cobblestone");
            //}
            base.DoInitialize();
        }


        public override RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            ICamera camera = arg.Camera;
            mat4 projection = camera.GetProjectionMatrix();
            mat4 view = camera.GetViewMatrix();
            mat4 model = GetModelMatrixFromParent();

            var method = RenderUnit.Methods[RenderStrategy.SIMPLE];
            
            ShaderProgram program = method.Program;
            program.SetUniform(u_projectionMat, projection);
            program.SetUniform(u_viewMat, view);
            program.SetUniform(u_modelMat, model);

            GLState.lineWidthSwitch.On();
            GLState.polygonModeSwitch.On();
            method.Render();
            GLState.lineWidthSwitch.Off();
            GLState.polygonModeSwitch.Off();
            return null;
        }

        protected override void DoInitRenderParams()
        {
            base.DoInitRenderParams();
            EnableRendering = RenderingFlags.Self;
        }

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            unit.Mesh = new BlockMesh();
            return unit;
        }
    }
}
