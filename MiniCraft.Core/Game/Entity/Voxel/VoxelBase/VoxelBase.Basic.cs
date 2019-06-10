using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiniCraft.Core.Game
{
    public abstract partial class VoxelBase : GameObject
    {
        private ivec3 index;
        public ivec3 Index
        {
            get { return index; }
            set
            {
                index = value;
                //TODO:注意这里是+=，之前的体心可能并不在0,0处。
                WorldPosition += (index + new vec3(0.5f));
            }
        }

        public Chunk Chunk { get { return Parent as Chunk; } }
        public WorldNode World { get { return Parent.Parent as WorldNode; } }

        private static VoxelBase emptyBlock;
        public static VoxelBase None
        {
            get
            {
                if (emptyBlock == null)
                {
                    emptyBlock = new AirBlock();
                }
                return emptyBlock;
            }
        }

        public VoxelBase() 
        {
            //this.Parent = parent;
            Physics.State = PhysicsStates.Solid;
        }

        protected override void DoInitialize()
        {
            if (Texture == null)
            {
                Texture = AssetManager.GetTexture(AssetManager.GetObjName(TypeId));
            }
            base.DoInitialize();
        }

        protected override void DisposeUnmanagedResources()
        {
            if (!usePrefab)
                this.RenderUnit.Dispose();
        }

        public override string ToString()
        {
            return $"{base.ToString()} {Index} Face:{FaceState} Visible:{Visible} TypeId:{TypeId}";
        }
    }
}
