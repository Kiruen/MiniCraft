using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    //定义游戏对象的诸多公共属性
    public abstract partial class GameObject : MNodeBase
    {
        public MPhysicsUnit Physics;

        public virtual bool Modified { get; set; } = false;

        //public MRenderUnit RenderUnit;
        [System.ComponentModel.Category("VoxelBase")]
        public vec4 Color { get; set; }

        //游戏对象的类型编号
        public int TypeId { get; set; }

        //透明相关
        public TransparentMode TransparentMode = TransparentMode.Opaque;
        public float Transparency = 1;
        public virtual bool Visible { get; set; } = true;

        protected vec3 globalPosition;
        protected bool globalPositionCaculated;
        public virtual vec3 GlobalPosition
        {
            get
            {
                if (Parent == null)
                    throw new Exception("未设置父对象！无法求得全局坐标！");
                else
                {
                    if (!globalPositionCaculated || worldSpacePropertyUpdated)
                    {
                        globalPosition = (GetModelMatrixFromParent() * vec4.zero).ToVec3();
                        globalPositionCaculated = true;
                    }
                    return globalPosition;
                }
            }
            set
            {
                var change = value - globalPosition;
                globalPosition = value;
                WorldPosition += change;
                //TODO:考虑更复杂的情况-是否需要同时改变parent？
                //worldSpacePropertyUpdated = true;
            }
        }
    }
}
