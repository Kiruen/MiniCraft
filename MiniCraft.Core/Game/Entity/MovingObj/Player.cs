using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    //class Arm : GameObject
    //{
    //    public Arm()
    //    {
    //        Scale = new vec3(0.3f, 0.3f, 1.1f);
    //    }

    //    public override void RenderAfter(MRenderEventArgs arg)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void RenderBefore(MRenderEventArgs arg)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void InitEventHandlers()
    //    {
            
    //    }
    //}

    public partial class Player : Mob
    {
        BasicManipulater bindingManipulater;

        public ItemContainer Bag = new ItemContainer();
        public GameObject arm;

        //public vec3 LastWorldPos;
        //TODO:player也可能有parent chunk，因此其位置可能会受parent影响
        public override vec3 WorldPosition
        {
            get => base.WorldPosition;
            set
            {
                //LastWorldPos = WorldPosition;
                base.WorldPosition = value;
                //使用绑定的操纵器，控制摄像机到合适的位置
                if(bindingManipulater != null)
                    bindingManipulater.MoveCameraTo(WorldPosition + new vec3(0, 0.8f, 0));
            }
        }

        public Player(BasicManipulater bindingManipulater)
        {
            this.bindingManipulater = bindingManipulater;
            ModelSize = new vec3(0.5f, 2, 0.4f);
            arm = new Block()
            {
                Scale = new vec3(0.3f, 0.3f, 1.1f),
                WorldPosition = new vec3(1, 0, 0),
                Parent = this,
                Active = true
            };
            Children.Add(arm);
        }

        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            //var camera = arg.Camera;
            //RotationAngle = GameState.manipulater.yaw;
            //RotationAngle += 1;
            //worldSpacePropertyUpdated = true;
            //Children[0].RotationAngle = GameState.manipulater.yaw;
            //arm.Roll = MathExt.RadianToAngle(GameState.manipulater.roll);
            //arm.Pitch = MathExt.RadianToAngle(GameState.manipulater.pitch);
            //RotationAxis = (glm.rotate(-MathExt.RadianToAngle(GameState.manipulater.yaw), new vec3(0, 1, 0)) * new vec4(1, 0, 0, 1)).ToVec3();
            //RotationAngle = -MathExt.RadianToAngle(GameState.manipulater.yaw);
            //RotationAngle = 90 - MathExt.RadianToAngle(GameState.manipulater.yaw);

            //proj矩阵会缓存下来
            //mat4 projection = camera.GetProjectionMatrix();
            //mat4 view = camera.GetViewMatrix();
            //mat4 model = GetModelMatrixFromParent();

            //var method = RenderUnit.Methods[RenderStrategy.SIMPLE];

            //ShaderProgram program = method.Program;
            //program.SetUniform(u_projectionMat, projection);
            //program.SetUniform(u_viewMat, view);
            //program.SetUniform(u_modelMat, model);

            //GLState.lineWidthSwitch.On();
            //GLState.polygonModeSwitch.On();
            ////method.Render();
            //GLState.lineWidthSwitch.Off();
            //GLState.polygonModeSwitch.Off();
            //foreach (IGameRenderable item in Children)
            //{
            //    item.RenderSelf(new MRenderEventArgs(arg) { MethodName = RenderStrategy.STD_TEX });
            //}
            return null;
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            //foreach (var item in onMouseClicked.GetInvocationList())
            //{

            //}
            throw new NotImplementedException();
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        //protected override void DoInitRenderParams()
        //{
        //    base.DoInitRenderParams();
        //    EnableRendering = RenderingFlags.Self | RenderingFlags.Children;
        //}

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            unit.Mesh = new BlockMesh();
            return unit;
        }

        protected override void InitEventHandlers()
        {

        }
    }
}
