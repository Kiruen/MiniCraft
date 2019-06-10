using CSharpGL;
using IrrKlang;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core.Game
{
    public static partial class GameState
    {
        public static int chunkInFrustumCount;
        public static int visibleVoxelCount;
        public static int visibleVoxelAcc;

        public static vec3 homePosition;

        public static vec3 currPosition
        {
            get
            {
                //var camera = scene.Camera as Camera;
                //return camera.Position;
                return player.WorldPosition;
            }
        }

        public static GameContainer gameContainer;

        public static RaycastHit raycastResult;
        public static Chunk chosingChunk;
        public static GameObject ChosingObj { get => raycastResult?.Obj ?? VoxelBase.None; }
        public static vec3 ChosingPosition { get => ChosingObj.GlobalPosition; }

        public static VoxelBase currVoxel;
        public static WorldNode currWorld;
        public static Chunk currChunk;
        public static ivec3 CurrChunkIndex { get => Utility.PosToChunkIndex(player?.WorldPosition ?? new vec3(0)); }

        public static Scene scene;
        public static BasicManipulater manipulater;
        public static Camera MainCamera { get { return scene.Camera as Camera; } }

        public static Form gameWindow;
        public static IGLCanvas canvas; //WinGLCanvas

        public static ActionList actionList;

        public static Player player;

        private static bool initialized;
        public static void InitializeBase(Form gameWindow, IGLCanvas canvas)
        {
            if (!initialized)
            {
                initialized = true;
            }
            else return;

            GameState.gameWindow = gameWindow;
            GameState.canvas = canvas;
            if (gameWindow == null || canvas == null)
                throw new Exception("Game initializing fails!");

            var position = new vec3(0, 6, 0);
            var target = new vec3(1, 4, 1);
            var up = new vec3(0, 1, 0);
            var camera = new Camera(position, target, up, CameraType.Perspective, canvas.Width, canvas.Height);
            scene = new Scene(camera);
            scene.ClearColor = Color.Gray.ToVec4();

            manipulater = new BasicManipulater(gameWindow);
            manipulater.Bind(camera, canvas);
            manipulater.StepLength = 0.2f;
            //manipulater.OriginalVelocity = 0.3f;

            canvas.TimerTriggerInterval = 25; //(int)(1000 / winGLCanvas1.FPS);

            SoundEngine.Initialize();
            PhysicsEngine.Initialize();
            //GameState.InitializePlayer();
            GLState.Initialize();
            Time.Initialize();
        }

        public static void InitializeActions()
        {
            if (scene.RootControl == null)
                throw new Exception("controls initialization fails!");

            var list = new ActionList
            {
                //new TransformAction(scene),
                new MLogicAction(scene, LogicActionStage.Load),
                new MLogicAction(scene, LogicActionStage.Physics),
                new MLogicAction(scene, LogicActionStage.Input),
                new MLogicAction(scene, LogicActionStage.GameLogic),
                new MRenderAction(scene),
                new GUIRenderAction(scene.RootControl)
            };
            actionList = list;
        }

        public static void InitializePlayer()
        {
            player = new Player(manipulater)
            {
                manipulater = manipulater,
                WorldPosition = new vec3(0, 30, 0)
            }; 
            player.Initialize();
            AssetManager.ChunkFileManager.LoadPlayerData();
            //加入游戏对象容器中
            gameContainer.Children.Add(player);
        }

        public static void InitializeGameContainer()
        {
            gameContainer = new GameContainer();
            gameContainer.Initialize();
            scene.RootNode = gameContainer;
            currWorld = gameContainer.DefaultWorld;
            currChunk = chosingChunk = Chunk.None;
        }
    }
}

