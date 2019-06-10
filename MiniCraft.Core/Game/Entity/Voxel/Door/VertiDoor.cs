using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public enum DoorPart : byte
    {
        Up = 1,
        Down = 2
    }

    public class VertiHalfDoor : VoxelBase, ILinkable
    {
        public DoorPart Part { get; set; } = DoorPart.Up;

        protected VertiDoorController controller;
        public MControllerBase Controller
        {
            get => controller;
            set
            {
                if (controller != null)
                    controller.Parts.Remove(this);
                controller = value as VertiDoorController;
                controller.Parts.Add(this);
            }
        }

        public VertiHalfDoor() //VertiDoorController controller = null : base(/*controller.Parent as Chunk*/)
        {
            this.ModelSize = new vec3(1, 1, 0.1f);
            this.WorldPosition += new vec3(0, 0, 0.49f);
            //this.controller = controller;
            //TODO:是否是最佳方案？？我们能确定哪些体素不会改变吗？
            Physics.Wrapper.Active = true;
            TransparentMode = TransparentMode.Transparent;
            //if (Part == DoorPart.Up)
            //{
            //    //WorldPosition = new vec3(0, 1, 0.5f);
            //    TransparentMode = TransparentMode.Transparent;
            //}
            //else
            //{
            //    //WorldPosition = new vec3(0, 0, 0.5f);
            //    TransparentMode = TransparentMode.Opaque;
            //}
        }

        protected override void InitEventHandlers()
        {
            base.InitEventHandlers();
            //OnSettledEvent += VertiHalfDoor_OnSettledEvent;
            //OnMouseClickedEvent += (args) =>;
        }

        bool opened = false;
        public override void OnMouseClicked(MMouseEventArgs args)
        {
            if(args.Button == GLMouseButtons.Right)
            {
                //尝试连接
                this.OnLinkage(null, null);
                foreach (VertiHalfDoor part in Controller.Parts)
                {
                    part.opened = !part.opened;
                    if(part.opened)
                    {
                        part.Pitch = 90;
                        part.WorldPosition -= new vec3(0.49f, 0, 0.49f);
                    }
                    else
                    {
                        part.Pitch = 0;
                        part.WorldPosition += new vec3(0.49f, 0, 0.49f);
                    }
                }
            }
            else
            {
                base.OnMouseClicked(args);
            }
        }

        public override void OnDestroy(GameObject sender, MEventArgs args)
        {
            foreach (VoxelBase part in Controller.Parts)
            {
                //正好不再检测六面方块。
                part.Chunk?.OnObjDestroy(part, args);
            }
        }


        public void OnLinkage(VoxelBase sender, MVoxelEventArgs args)
        {
            //无需考虑面遮掩情况，故直接覆盖
            var world = World; //Chunk.Parent as WorldNode;
            //var other_part_enum = Part == DoorPart.Down ? DoorPart.Up : DoorPart.Down;
            var gpos = GlobalPosition + new vec3(0, (Part == DoorPart.Down ? 1 : -1), 0);
            var other_part_chunk = world[gpos];
            if (other_part_chunk?[gpos] is VertiHalfDoor other_part)
            {
                Controller = other_part.Controller;
            }
            else
            {
                Controller = new VertiDoorController();
            }
        }

        public override void OnSettled(GameObject sender, MVoxelEventArgs args)
        {
            //尝试连接
            this.OnLinkage(null, null);
            //无需考虑面遮掩情况，故直接覆盖
            var world = Chunk.Parent as WorldNode;
            var other_part_enum = Part == DoorPart.Down ? DoorPart.Up : DoorPart.Down;
            var gpos = GlobalPosition + new vec3(0, (Part == DoorPart.Down ? 1 : -1), 0);

            var other_part_chunk = world[gpos];
            var other_part = other_part_chunk?[gpos] as VertiHalfDoor;
            if (other_part == null)
            {
                other_part = AssetManager.GenVoxel(other_part_enum == DoorPart.Up ? 5 : 6) as VertiHalfDoor;
                other_part.Part = other_part_enum;
                other_part_chunk[gpos] = other_part;
                //放置事件
                other_part.OnSettled(sender, args);
                //连接事件
                other_part.OnLinkage(null, null);
            }
            //TODO:很少会出现另一个chunk没有被加载的情况，不过还是要小心
        }

        //protected override void DoInitialize()
        //{
        //    //无需对base进行初始化，因为体素只是一个容器，prefeb_RENDERUNIT才是真正的渲染实体
        //    if (Texture == null)
        //    {
        //        Texture = (Part == DoorPart.Up ?
        //            AssetManager.GetTexture(@"door_wood_upper") :
        //            AssetManager.GetTexture(@"door_wood_lower"));
        //    }
        //    base.DoInitialize();
        //}

        protected override MRenderUnit InitRenderUnit()
        {
            var unit = base.InitRenderUnit();
            unit.Mesh = new VertiDoorMesh();
            return unit;
        }

        protected override void DoInitRenderParams()
        {
            EnableRendering = RenderingFlags.Self;
            base.DoInitRenderParams();
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

    /// <summary>
    /// 立式门控制器
    /// </summary>
    public class VertiDoorController : MControllerBase
    {
        public VertiDoorController()
        {

        }

        //protected override void InitEventHandlers()
        //{
        //    base.InitEventHandlers();
        //    OnSettledEvent += (args) =>
        //    {
        //        var world = Chunk.Parent as WorldNode;
        //        var gpos = GlobalPosition; gpos.y += 1;

        //        var chunk_up = world[gpos];
        //        //var index_up = chunk_up[pos].Index;
        //        //var index_up = new GridIndex(index.X, chunk_up == Chunk ? index.Y + 1 : 0, index.Z);

        //        var up = Children[0] as VertiHalfDoor;
        //        var down = Children[1] as VertiHalfDoor;

        //        chunk_up[gpos] = up;
        //        this.Chunk[args.Index] = down;
        //        //up.OnPlace(new VoxelEventArgs() { Index = index_up, Parent = chunk_up });
        //        //down.OnPlace(args);
        //    };
        //}

        //protected override void DoInitialize()
        //{
        //    foreach (var item in Children)
        //    {
        //        item.Initialize();
        //    }
        //    base.DoInitialize();
        //}
    }
}
