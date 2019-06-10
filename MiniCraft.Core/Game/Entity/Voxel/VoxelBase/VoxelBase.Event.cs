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
        //private MVoxelEventHandler onPlacedOn;
        public event MVoxelEventHandler OnPlacedOnEvent;
        //{
        //    add { onPlacedOn += value; }
        //    remove { onPlacedOn -= value; }
        //}
        /// <summary>
        /// 当体素被覆盖上(或连接上)其他体素时将被调用
        /// </summary>
        /// <param name="args"></param>
        public void OnPlacedOn(GameObject sender, MVoxelEventArgs args)
        {
            OnPlacedOnEvent?.Invoke(sender, args);
        }


        public event MVoxelEventHandler OnSettledEvent;
        /// <summary>
        /// 当体素被放置到实体上时将被调用
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnSettled(GameObject sender, MVoxelEventArgs args)
        {
            OnSettledEvent?.Invoke(sender, args);
        }

        public event MVoxelEventHandler OnStepInEvent;
        /// <summary>
        /// 当某个对象接触到体素时
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnStepIn(GameObject sender, MVoxelEventArgs args)
        {
            OnStepInEvent?.Invoke(sender, args);
        }


        public event MVoxelEventHandler OnStepOutEvent;
        /// <summary>
        /// 当某个对象从该体素离开时
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnStepOut(GameObject sender, MVoxelEventArgs args)
        {
            OnStepOutEvent?.Invoke(sender, args);
        }

        protected override void InitEventHandlers()
        {
            OnSettledEvent += VoxelBase_OnSettledEvent;
            OnMouseClickedEvent += VoxelBase_OnMouseClickedEvent;
            OnPlacedOnEvent += VoxelBase_OnPlacedOnEvent;
            OnDestroyEvent += VoxelBase_OnDestroyEvent;
        }


        private void VoxelBase_OnPlacedOnEvent(GameObject sender, MVoxelEventArgs args)
        {
            var gpos = GlobalPosition;
            if (args.Face == Direction3D.Up)
                gpos += new vec3(0, 1, 0);
            else if (args.Face == Direction3D.Down)
                gpos -= new vec3(0, 1, 0);
            else if (args.Face == Direction3D.Right)
                gpos += new vec3(1, 0, 0);
            else if (args.Face == Direction3D.Left)
                gpos -= new vec3(1, 0, 0);
            else if (args.Face == Direction3D.Front)
                gpos += new vec3(0, 0, 1);
            else if (args.Face == Direction3D.Back)
                gpos -= new vec3(0, 0, 1);
            //放到指定栅格中，将设置基本的数据
            World[gpos][gpos] = args.Voxel;
            //还将触发被放置事件
            args.Voxel.OnSettled(null, new MVoxelEventArgs()
            {
                //Index = args.Index,
                //Chunk = args.Chunk,
            });
        }

        private void VoxelBase_OnMouseClickedEvent(MMouseEventArgs args)
        {
            if (args.Button == GLMouseButtons.Right)
            {
                var block = GameState.player.Bag.SelectedItem.Spawn();
                OnPlacedOn(block, new MVoxelEventArgs()
                {
                    Voxel = block,
                    Face = GameState.raycastResult.Face
                });
            }
            else
            {
                //TODO:销毁事件？鼠标按住的事件？？
                OnDestroy(null, new MVoxelEventArgs());
            }
        }

        protected static vec3[] faceCheckSeq = new[]
        {
            new vec3(0, 0, 1), new vec3(1, 0, 0), new vec3(0, 1, 0),
            new vec3(0, 0, -1), new vec3(-1, 0, 0), new vec3(0, -1, 0),
        };
        protected static int[] opposites = new[]
        {
            3, 4, 5, 0, 1, 2
        };
        private void VoxelBase_OnSettledEvent(GameObject sender, MVoxelEventArgs args)
        {
            var origpos = GlobalPosition;
            for (int i = 0; i < 6; i++)
            {
                var gpos = origpos + faceCheckSeq[i];
                //如果参数没有指定发生事件的区块，则从全局访问
                var chunk = args.Chunk ?? World[gpos];
                //若访问到的方块所在的区块未加载，则忽略此方块
                if (chunk.Temporary == true) continue;
                var voxel = chunk[gpos]; /*World[gpos]?[gpos] ?? None;*/
                if (voxel != None)
                {
                    if(voxel.TransparentMode == TransparentMode.Opaque)
                    {
                        this.FaceState[i] = false;
                        if (this.FaceState.StateValue == 0)
                            this.Visible = false;
                    }
                    if(this.TransparentMode == TransparentMode.Opaque)
                    {
                        voxel.FaceState[opposites[i]] = false;
                        if (voxel.FaceState.StateValue == 0)
                            voxel.Visible = false;
                    }
                }
            }
        }

        private void VoxelBase_OnDestroyEvent(GameObject sender, MEventArgs args)
        {
            var origpos = GlobalPosition;
            for (int i = 0; i < 6; i++)
            {
                var gpos = origpos + faceCheckSeq[i];
                //如果参数没有指定发生事件的区块，则从全局访问
                var chunk = args.Chunk ?? World[gpos];
                //若访问到的方块所在的区块未加载，则忽略此方块
                if (chunk.Temporary == true) continue;
                var voxel = chunk[gpos]; //World[gpos]?[gpos] ?? None;
                if (voxel != None)
                {
                    voxel.FaceState[opposites[i]] = true;
                    voxel.Visible = true;
                }
            }
            //注意回收非托管资源。终接器会自动调用
            //this.Dispose();
            //通知区块自己被销毁了
            Chunk.OnObjDestroy(this, args);
        }
    }
}
