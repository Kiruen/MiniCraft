using CSharpGL;
using IrrKlang;
using MiniCraft.Core.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MiniCraft.Core.Constant;

namespace MiniCraft.Core.Game
{
    public class GameContainer : MNodeBase
    {
        public MWinCtrlRoot rootControl;
        public SkyBox skybox;
        public MCrossHair crossHair;

        protected Dictionary<string, WorldNode> worlds = new Dictionary<string, WorldNode>(4);
        public WorldNode this[string wname]
        {
            get => worlds[wname];
            protected set => worlds[wname] = value;
        }

        public WorldNode DefaultWorld
        {
            get
            {
                worlds.TryGetValue("default", out WorldNode world);
                if (world == null)
                {
                    this["default"] = world = new WorldNode();
                }
                return world;
            }
        }

        public GameContainer()
        {
            EnableRendering = RenderingFlags.Children;
        }

        protected override void DoInitialize()
        {
            base.DoInitialize();

            this.skybox = SkyBox.Create();
            Children.Add(skybox);

            this.rootControl = GetRootControl();
            this.rootControl.Bind(GameState.canvas);
            GameState.scene.RootControl = this.rootControl;
        }

        public async Task TestCreate()
        {
            GameState.currWorld = DefaultWorld;

            await DefaultWorld.ChunkCache.Update(GameState.CurrChunkIndex);

            DefaultWorld.ChunkCache.Flush();
            Children.Add(DefaultWorld);
        }

        private MWinCtrlRoot GetRootControl()
        {
            var root = new MWinCtrlRoot(GameState.canvas.Width, GameState.canvas.Height);

            //    {
            //        var control = new MLabel(100) { Anchor = GUIAnchorStyles.Left | GUIAnchorStyles.Top };

            //        control.Width = 100; control.Height = 30;
            //        control.Text = "Label Test!";
            //        control.RenderBackground = true;
            //        control.BackgroundColor = new vec4(1, 0, 0, 1);
            //        control.MouseUp += control_MouseUp;
            //        root.Children.Add(control);
            //        control.Location = new GUIPoint(10, root.Height - control.Height - 10);
            //        label = control;
            //    }
            //    {
            //        var control = new MButton() { Anchor = GUIAnchorStyles.Left | GUIAnchorStyles.Top };
            //        control.Width = 100; control.Height = 50;
            //        control.Focused = true;
            //        control.MouseUp += control_MouseUp;
            //        root.Children.Add(control);
            //        control.Location = new GUIPoint(10, root.Height - control.Height - 50);
            //    }
            /*{
                var bitmap = new Bitmap(AssetManager.GetTexturePath("cloth.png"));
                var control = new MImageBox(bitmap, false) { Anchor = GUIAnchorStyles.Left | GUIAnchorStyles.Top };
                control.Width = 100; control.Height = 50;
                bitmap.Dispose();
                //control.MouseUp += control_MouseUp;
                root.Children.Add(control);
                control.Location = new GUIPoint(10, root.Height - control.Height - 110);
            }*/
            {
                var control = new MCrossHair() { Anchor = GUIAnchorStyles.Left | GUIAnchorStyles.Top };
                control.Width = 20; control.Height = 20;
                root.Children.Add(control);
                control.Location = new GUIPoint((root.Width - control.Width) / 2, (root.Height - control.Height) / 2);
                crossHair = control;
            }
            
            return root;
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            throw new NotImplementedException();
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 单一世界的区块管理器(支持多人游戏的区块加载)
    /// </summary>
    public class WorldNode : MNodeBase
    {
        public static Framebuffer depthFrame;
        public static Texture tex;

        //public Chunk WorldCenter { get { return map[MAP_X_HALF_LEN_MAX, MAP_Y_HALF_LEN_MAX, MAP_Z_HALF_LEN_MAX]; } }

        //区块数据缓存
        public ChunkCache ChunkCache;

        //TODO:剔除不在渲染半径内的chunk。已实现
        vec3 renderRegion = new vec3(RENDER_CHUNK_X_RADIUS, RENDER_CHUNK_Y_RADIUS, RENDER_CHUNK_Z_RADIUS);
        public IEnumerable<Chunk> ChunkStream
        {
            get
            {
                foreach (var item in ChunkCache
                    .Where(item => Utility.IsPointInCuboid(
                        item.Index, GameState.CurrChunkIndex, renderRegion)))
                {
                    yield return item;
                }
            }
        }


        public WorldNode()
        {
            EnableRendering = RenderingFlags.Self;
            ChunkCache = new ChunkCache(this);
            //if (!this.IsInitialized) { this.Initialize(); }
        }


        #region 访问chunk的索引器
        /// <summary>
        /// 根据全局世界坐标访问对应位置的区块
        /// </summary>
        /// <returns></returns>
        public Chunk this[float gx, float gy, float gz]
        {
            get
            {
                var index = Utility.PosToChunkIndex(gx, gy, gz);
                return this[index.x, index.y, index.z];
            }
            set
            {
                var index = Utility.PosToChunkIndex(gx, gy, gz);
                this[index.x, index.y, index.z] = value;
            }
        }


        public Chunk this[vec3 gpos]
        {
            get
            {
                return this[gpos.x, gpos.y, gpos.z];
            }
            set
            {
                this[gpos.x, gpos.y, gpos.z] = value;
            }
        }

        public Chunk this[int ix, int iy, int iz]
        {
            get
            {
                var index = new ivec3(ix, iy, iz);
                ChunkCache.TryGetValue(index, out Chunk chunk);
                if(chunk == null)
                {
                    chunk = Chunk.None;
                    chunk.PutOn(this, index);
                }
                return chunk;
            }
            set
            {
                value.Parent = this;
                var index = new ivec3(ix, iy, iz);
                ChunkCache.Add(index, value);
            }
        }

        /// <summary>
        /// 根据全局世界坐标访问对应位置的体素
        /// </summary>
        /// <returns></returns>
        public VoxelBase GetVoxel(float gx, float gy, float gz)
        {
            return this[gx, gy, gz][gx, gx, gx];
        }

        public VoxelBase GetVoxel(vec3 gpos)
        {
            return GetVoxel(gpos.x, gpos.y, gpos.z);
        }
        #endregion


        void initDepthFrame(RenderEventArgs arg)
        {
            int width = arg.Param.Viewport.width,
                height = arg.Param.Viewport.height;
            depthFrame = new Framebuffer(width, height);
            var depthStorage = new TexImage2D(TexImage2D.Target.TextureRectangle, GL.GL_RG32F, width, height, GL.GL_RGB, GL.GL_FLOAT);
            tex = new Texture(depthStorage,
                new TexParameteri(TexParameter.PropertyName.TextureWrapS, (int)GL.GL_CLAMP),
                new TexParameteri(TexParameter.PropertyName.TextureWrapT, (int)GL.GL_CLAMP),
                new TexParameteri(TexParameter.PropertyName.TextureMagFilter, (int)GL.GL_NEAREST),
                new TexParameteri(TexParameter.PropertyName.TextureMinFilter, (int)GL.GL_NEAREST)
                );
            tex.Initialize();
            tex.TextureUnitIndex = 0;
            depthFrame.SetDrawBuffer(tex.Id);
        }

        /* 失败的优化
        private RenderMethod RenderSelf1(MRenderEventArgs arg, bool doRender = true)
        {
            //TODO: Frustum CULLING!!!去除看不到的chunk，只对看得到的chunk进行正常渲染(包括半透明处理等)
            var camera = arg.Camera as Camera;
            var frustum = camera.ExtractFrustum();

            var toRender = ChunkStream
                        .Where((c, i) => c.Visible)
                        .Where(chunk => 
                        frustum.IsAABBInFrustum(chunk.Wrapper as AABBBox))
                        .OrderByDescending(c => 
                        (c.GlobalPosition - camera.position).length())
                        .ToArray();
            GameState.chunkInFrustumCount = toRender.Length;
            var chosing = Chunk.None;


            GLState.CullFace.On();
            var instancingBatch = new InstancingBatchNode(toRender.SelectMany(c => c.theOpaque));
            instancingBatch.RenderSelf(arg);
            GLState.CullFace.Off();

            //Random ran = new Random();
            var blendingBatch = new TransparentBatchNode(toRender.SelectMany(c => c.toBlend)
                .OrderByDescending(b =>
                {
                    return (b.GlobalPosition - camera.position).length();
                })); //toBlend.Where(b => QueryManager.SampleRendered((uint)b.Id))
            blendingBatch.RenderSelf(arg);

            //设置当前光标指向的的chunk
            GameState.chosingChunk = chosing;
            //渲染选择边框
            GameState.chosingChunk?.Wrapper.RenderSelf(arg);
            GameState.chosingObj?.Wrapper.RenderSelf(arg);
            //画一下体心
            if(GameState.chosingObj != null)
            {
                var voxel = GameState.chosingObj as VoxelBase;

                //GL.Instance.PointSize(8);
                //new PointSprite(voxel.GlobalPosition).RenderSelf(arg);
                //voxel?.RenderOtherwhere(new MRenderEventArgs(arg));
            }
            GameState.player.Wrapper.RenderSelf(new MRenderEventArgs(arg) { MethodName = RenderStrategy.STD_TEX });
            return null;
        }
        */
        public override RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            //TODO: Frustum CULLING!!!去除看不到的chunk，只对看得到的chunk进行正常渲染(包括半透明处理等)
            var camera = arg.Camera as Camera;
            var frustum = camera.ExtractFrustum();

            var toRender = ChunkStream
                        .Where((c, i) => c.Visible)
                        .Where(chunk => frustum.IsAABBInFrustum(chunk.Physics.Wrapper as AABB))
                        .OrderByDescending(c =>
                        (c.GlobalPosition - camera.position).length());
            //.ToArray();
            int count = 0;
            var chosing = Chunk.None;
            foreach (var chunk in toRender)
            {
                count++;
                chunk.RenderSelf(arg);
                if (PhysicsEngine.IntersectAABB(new Ray(camera.Position, camera.GetFront(), 128),
                    chunk.Physics.Wrapper as AABB))
                {
                    chosing = chunk;
                }
            }
            GameState.chunkInFrustumCount = count;
            //设置当前光标指向的的chunk
            GameState.chosingChunk = chosing;
            //渲染选择边框
            //GameState.chosingChunk?.Physics.Wrapper.RenderSelf(arg);
            GameState.ChosingObj?.Physics.Wrapper.RenderSelf(arg);
            //画一下体心
            if (GameState.ChosingObj != VoxelBase.None)
            {
                var voxel = GameState.ChosingObj as VoxelBase;
                //GL.Instance.PointSize(8);
                //new PointSprite(voxel.GlobalPosition).RenderSelf(arg);
                //voxel?.RenderOtherwhere(new MRenderEventArgs(arg));
            }
            //GameState.player.Physics.Wrapper.RenderSelf(
            //    new MRenderEventArgs(arg) { MethodName = RenderStrategy.STD_TEX });
            return null;
        }
        

        private void TestRenderWithBackCulling(MRenderEventArgs arg)
        {
            var toRender = ChunkStream
                .Where(c => c.Visible)
                .SelectMany(c => c.Voxels)
                .Where(b => b.Visible)
                .ToArray();
            foreach (var item in toRender) //order
            {
                var wrap = item.Physics.Wrapper;
                QueryManager.BeginQuery(QueryTarget.SamplesPassed, (uint)item.Id);
                wrap.RenderSelf(arg);
                QueryManager.EndQuery(QueryTarget.SamplesPassed);
            }
            foreach (var block in toRender) //order
            {
                if (QueryManager.SampleRendered((uint)block.Id))
                    block.RenderSelf(arg);
            }
        }

        public override void RenderBefore(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }

        protected override RenderMethod DoRenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            throw new NotImplementedException();
        }

        public override void RenderAfter(MRenderEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }
}
