using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniCraft.Core.Constant;

namespace MiniCraft.Core.Game
{
    public enum LogicActionStage : byte
    {
        GameLogic = 1,
        Physics = 2,
        Input = 3,
        BeforeAll = 4,
        AfterAll = 5,
        Load = 6,
    }

    /// <summary>
    /// 游戏逻辑动作。对指定场景中所有对象进行除渲染动作以外的逻辑操作。
    /// </summary>
    public class MLogicAction : ActionBase
    {
        private LogicActionStage stage;
        private Scene scene;


        /// <summary>
        /// Render <see cref="IRenderable"/> objects.
        /// </summary>
        /// <param name="scene"></param>
        public MLogicAction(Scene scene, LogicActionStage stage)
        {
            this.scene = scene ?? throw new ArgumentNullException("Where is the scene?");
            this.stage = stage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public override void Act(ActionParams param)
        {
            if (stage == LogicActionStage.Load)
                HandleLoadingWorkAsync();
            else if (stage == LogicActionStage.Physics)
                HandlePhysics();
            else if (stage == LogicActionStage.Input)
                HandleInput();
            else if (stage == LogicActionStage.GameLogic)
                HandleFrameGameLogic();
        }

        private async void HandleLoadingWorkAsync()
        {
            //var arg = new RenderEventArgs(param, scene.Camera);
            var world = GameState.currWorld;
            var camera = scene.Camera;
            //GameState.chosingVoxel = chosing as VoxelBase;

            //var oldIndex = Utility.PosToChunkIndex(GameState.manipulater.LastCameraPosition);
            //var index = Utility.PosToChunkIndex(camera.Position);
            //GameState.CurrChunkIndex = index;

            //var delta = index - oldIndex;
            //var delta = GameState.player.WorldPosition - GameState.player.LastWorldPos;
            //GameState.player.LastWorldPos = GameState.player.WorldPosition;

            //TODO:优化
            ChunkCache.SamplePosition();
            //world.chunkCache.Flush();
            if (ChunkCache.IsUpdateRequested)
            {
                int loadCount = await world.ChunkCache.Update(GameState.CurrChunkIndex);
                if(loadCount > 0)
                {
                    //及时清理掉不用的区块，并立即释放内存
                    world.ChunkCache.Clean(c => (c.Index - GameState.CurrChunkIndex).length() > RENDER_CHUNK_X_RADIUS * 2);
                    world.ChunkCache.Flush();
                    //及时回收卸载掉的区块数据
                    //GC.Collect(2);
                }
            }
        }

        private void HandleInput()
        {
            var world = GameState.currWorld;
            var camera = scene.Camera;
            //
            ////GameState.player.Pitch = MathExt.RadianToAngle(GameState.manipulater.pitch);
            //GameState.player.Roll = MathExt.RadianToAngle(GameState.manipulater.roll);
            //GameState.player.Pitch = MathExt.RadianToAngle(GameState.manipulater.pitch);
            //拾取物体
            GameState.raycastResult = PhysicsEngine.Raycast(world, 
                    new Ray(camera.Position, camera.GetFront(), 24));
            //
            GameState.currChunk = world[GameState.player.WorldPosition];
            //按键处理
            Input.HanleInputs();
            //处理输入的命令
            Commander.Handle();
        }

        private void HandlePhysics()
        {
            PhysicsEngine.Work();
        }

        private void HandleFrameGameLogic()
        {
            var last = GameState.currVoxel;
            var gpos = GameState.currPosition + new vec3(0, -0.6f, 0);
            var curr = GameState.currVoxel = GameState.currWorld[gpos][gpos];
            if(last != curr)
            {
                last?.OnStepOut(GameState.player, new MVoxelEventArgs());
                curr?.OnStepIn(GameState.player, new MVoxelEventArgs());
            } 
        }
    }
}
