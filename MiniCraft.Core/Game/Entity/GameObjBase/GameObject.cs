using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public enum TransparentMode
    {
        Opaque = 0,
        SemiTransparent = 1,
        Transparent = 2,
    }

    public abstract partial class GameObject : MNodeBase
    {
        /// <summary>
        /// Render before/after children? Render children? 
        /// RenderAction cares about this property. Other actions, maybe, maybe not, your choice.
        /// </summary>
        protected MRenderUnit renderUnit;
        public MRenderUnit RenderUnit
        {
            get
            {
                if (renderUnit == null) { InitRenderParams(); }
                return renderUnit;
            }
            set
            {
                renderUnit = value;
            }
        }

        public MeshBase Mesh
        {
            get
            {
                return RenderUnit.Mesh;
            }
        }

        public Texture Texture { get; protected set; }


        public GameObject(bool genBoundingBox = true)
        {
            this.Color = new vec4(1);
            //GlobalPosition = (GetModelMatrixFromParent() * vec4.zero).ToVec3();
            //初始化物理参数
            Physics = new MPhysicsUnit();
            if (genBoundingBox)
                this.Physics.Wrapper = new AABB(this);

            //初始化事件系统(无法延迟初始化！)
            this.InitEventHandlers();
            //初始化渲染器数据(无法延迟)
            this.InitRenderParams();
        }


        bool renderParamsInited;
        protected virtual void InitRenderParams()
        {
            if(!renderParamsInited)
            {
                DoInitRenderParams();
                renderParamsInited = true;
            }
        }

        /// <summary>
        /// 创建、初始化如BoundingBox这类的物理相关的参数、对象。所有派生类必须实现并在适合的位置调用。
        /// </summary>
        /// <returns></returns>
        protected virtual void DoInitPhysicsParams()
        {
            Physics.Wrapper?.Initialize();
        }
        bool physicsParamsInited;
        protected virtual void InitPhysicsParams()
        {
            if (!physicsParamsInited)
            {
                DoInitPhysicsParams();
                physicsParamsInited = true;
            }
        }

        /// <summary>
        /// 总的初始化函数，负责调用所有子初始化过程
        /// </summary>
        protected override void DoInitialize()
        {
            //this.InitEventHandlers();
            //this.InitRenderParams();
            this.RenderUnit?.Initialize();
            this.InitPhysicsParams();
        }

        //TODO:考虑是否可以有公共实现
        protected override void DisposeUnmanagedResources()
        {
            if (!usePrefab)
                this.RenderUnit.Dispose();
        }

        /// <summary>
        /// 对象拷贝
        /// </summary>
        /// <returns></returns>
        public virtual GameObject Clone()
        {
            throw new NotImplementedException();
        }
    }
}
