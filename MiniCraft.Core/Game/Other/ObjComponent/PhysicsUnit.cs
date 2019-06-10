using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BoundingBox = MiniCraft.Core.Game.BoundingBox;

namespace MiniCraft.Core.Game
{
    /// <summary>
    /// 物理功能单元
    /// </summary>
    public class MPhysicsUnit
    {
        //物体速度
        public vec3 Velocity;

        //物体的包围盒
        public BoundingBox Wrapper;

        /// 定义体素的物态
        public PhysicsStates State;
    }
}
