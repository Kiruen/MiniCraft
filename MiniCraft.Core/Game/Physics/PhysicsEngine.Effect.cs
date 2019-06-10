using CSharpGL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MiniCraft.Core.Game
{
    public abstract class PhysicsAction
    {
        public PhysicsAction(GameObject obj)
        {
            Obj = obj;
        }

        public GameObject Obj;
        public float ElapsedTime;
        public float LastingTime;
        public bool Finished;
        public abstract void Do();
    }
    
    /// <summary>
    /// 物理平滑移动。只定义一个方向上的移动，不影响其他非共线方向上的移动。
    /// </summary>
    public class MoveAction : PhysicsAction
    {
        public const float DEFAULT_SPAN = 1F;
        public vec3 Start;
        public vec3 Delta;
        public vec3 End;

        public vec3 LastPos { get; private set; }
        public vec3 LastMovement { get; private set; }
        public vec3 CurrPos { get; private set; }

        public MoveAction(GameObject obj, vec3 start, vec3 delta, float lastingTime=DEFAULT_SPAN) : base(obj)
        {
            if (delta.length() == 0)
                Finished = true;
            else
            {
                Start = start;
                Delta = delta;
                End = Start + Delta;
                LastPos = CurrPos = start;
                LastingTime = lastingTime;
            }
        }

        public override void Do()
        {
            if (Finished == true) return;

            ElapsedTime += Time.FixedRealTimeSpan;
            if (ElapsedTime > LastingTime)
            {
                ElapsedTime = LastingTime;
                //GameState.manipulater.StepLength = GameState.manipulater.OriginalVelocity;
                Finished = true;
            }
            LastPos = CurrPos;
            CurrPos = MathExt.Lerp(Start, End, ElapsedTime / LastingTime);
            LastMovement = CurrPos - LastPos;
            Obj.WorldPosition += LastMovement;
        }
    }

    public static partial class PhysicsEngine
    {
        public const float g = -0.98f;

        //private static ConcurrentQueue<>
        private static List<MoveAction> moves = new List<MoveAction>(128);
        private static HashSet<GameObject> objsEffectedByGravity = new HashSet<GameObject>();

        private static object SyncRoot = new object();

        public static bool GravityOn { get; set; } = true;

        public static void PostAction(PhysicsAction action)
        {
            lock(SyncRoot)
            {
                moves.Add(action as MoveAction);
                objsEffectedByGravity.Add(action.Obj);
            }
        }

        public static void Work()
        {
            lock (SyncRoot)
            {
                var temp = new List<MoveAction>(32);
                int workCount = (int)MathExt.Clamp(1 + Time.DeltaTime / Time.FixedRealTimeSpan, 8, 15);
                for (int i = 0; i < workCount; i++)
                {
                    if(GravityOn)
                    {
                        foreach (var obj in objsEffectedByGravity)
                        {
                            Gravity(obj);
                            if (obj.Physics.Velocity.y != 0)
                                PostAction(new MoveAction(obj, obj.WorldPosition,
                                    new vec3(0, obj.Physics.Velocity.y * Time.FixedRealTimeSpan, 0)));
                        }
                    }
                    foreach (var move in moves)
                    {
                        Move(move);
                        Collide(move);

                        if (!move.Finished)
                            temp.Add(move);
                    }
                }
                if (temp.Count != moves.Count)
                    moves = temp;
            }
        }

        private static void Move(MoveAction move)
        {
            move?.Do();
        }

        private static void Gravity(GameObject obj)
        {
            var pos = obj.WorldPosition;
            var testPos = pos - new vec3(0, 1, 0);
            //GameState.currWorld[testPos][testPos] == VoxelBase.None
            //var colli = Raycast(GameState.currWorld, new Ray(pos, testPos)); 
            //if (colli.Obj == null || colli.Obj == VoxelBase.None)
            var downChunk = GameState.currWorld[testPos];
            var downObj = downChunk[testPos];   //
            //downObj == VoxelBase.None || downObj != VoxelBase.None &&
            //若下方的物体是可穿行的，或者是可穿行的但没有接触到，则坠落
            if (downChunk.Skipable || IsPassable(downObj.Physics) ||
                !CollideAABB(obj.Physics.Wrapper as AABB, downObj.Physics.Wrapper as AABB))
            {
                float accSpeed = g;
                if (downObj.Physics.State == PhysicsStates.Liquid)
                    accSpeed *= 0.15f;
                //根据加速度改变速度
                obj.Physics.Velocity.y += accSpeed * Time.FixedRealTimeSpan;
                if (obj.Physics.Velocity.y < 2f * accSpeed)
                    obj.Physics.Velocity.y = 2f * accSpeed;
                //发生接触事件

                //TODO:判断是否是主玩家
                if(obj is Player)
                {
                    Input.GetKeyState("jump").UserLocked = true;
                }
                //obj.WorldPosition = testPos;
                //PostAction(new MoveAction(obj, obj.WorldPosition, new vec3(0, -1, 0)));
            } //PhysicsEngine.CollideAABB()
        }

        static float[] checkPoints = new[] { 0.8f, 0, -0.3f};
        private static void Collide(MoveAction move)
        {
            for (int i = 0; i < 3; i++)
            {
                var testPos = move.Obj.WorldPosition;
                testPos.y += checkPoints[i];

                var testChunk = GameState.currWorld[testPos];
                var obj = testChunk[testPos];
                
                //obj != VoxelBase.None &&
                //若不可穿行，且碰撞到了，才会执行碰撞效果
                //TODO:要区分对待各种物态的碰撞检测
                    if (!testChunk.Skipable && !IsPassable(obj.Physics) &&
                    CollideAABB(obj.Physics.Wrapper as AABB,
                    move.Obj.Physics.Wrapper as AABB))
                {
                    //回退
                    move.Obj.WorldPosition -= move.LastMovement; //Utility.PosToChunkIndex(lastpos)

                    if (move.LastMovement.x != 0)
                        move.Obj.Physics.Velocity.x *= 0.1f;
                    if (move.LastMovement.y != 0)
                    {
                        move.Obj.Physics.Velocity.y = 0;
                        Input.GetKeyState("jump").UserLocked = false;
                    }
                    if (move.LastMovement.z != 0)
                        move.Obj.Physics.Velocity.z *= 0.1f;

                    move.Finished = true;
                    break;
                }//PhysicsEngine.CollideAABB()
            }
        }

        public static void AddForce(GameObject obj, vec3 dir)
        {
            obj.Physics.Velocity += (-g * 0.68f) * dir;
        }

        public static bool IsPassable(MPhysicsUnit unit)
        {
            return unit.State != PhysicsStates.Solid;
        }
    }
}
