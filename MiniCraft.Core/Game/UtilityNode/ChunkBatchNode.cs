using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static MiniCraft.Core.Game.GameObject;
namespace MiniCraft.Core.Game
{
    /// <summary>
    /// 透明渲染批处理结点
    /// </summary>
    public class ChunkBatchNode : BatchNode
    {
        public const int MAX_INSTANCE_COUNT_ONCE = 192;
        public const int GROUPS_ARRAY_LENGTH = 4;
        //Dictionary<int, List<GameObject>> opaqueGroups = new Dictionary<int, List<GameObject>>(16);
        //Dictionary<int, List<GameObject>> transparentGroups = new Dictionary<int, List<GameObject>>(16);
        //Dictionary<int, List<GameObject>> semitransparentGroups = new Dictionary<int, List<GameObject>>(16);

        Dictionary<int, LinkedList<GameObject>>[] groupsArray = new Dictionary<int, LinkedList<GameObject>>[GROUPS_ARRAY_LENGTH];

        //Dictionary<int, List<mat4>> modelMatsGroup = new Dictionary<int, List<mat4>>(16);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public ChunkBatchNode(Chunk chunk) : base(null)
        {
            for (int i = 0; i < GROUPS_ARRAY_LENGTH; i++)
            {
                groupsArray[i] = new Dictionary<int, LinkedList<GameObject>>(16);
            }
        }

        //TODO:注意，严重依赖于TransparentMode，故TransparentMode不能随便修改！只读吧
        public void Push(GameObject obj)
        {
            int tmode = (int)obj.TransparentMode;
            //TODO:注意可能会出错
            int objTypeId = obj.TypeId;//AssetManager.GetObjId(obj.GetType());
            var groups = groupsArray[tmode];
            if(!groups.ContainsKey(objTypeId))
            {
                groups.Add(objTypeId, new LinkedList<GameObject>());
                //modelMatsGroup.Add(objTypeId, new List<mat4>(4096));
            }
            groups[objTypeId].AddLast(obj);
            //modelMatsGroup[objTypeId].Add(obj.GetModelMatrixFromParent());
        }

        public void Pop(GameObject obj)
        {
            int tmode = (int)obj.TransparentMode;
            //TODO:注意可能会出错
            int objTypeId = obj.TypeId; //AssetManager.GetObjId(obj.GetType());
            var groups = groupsArray[tmode];
            if (groups.ContainsKey(objTypeId))
                groups[objTypeId].Remove(obj);
        }

        private unsafe void RenderGroups(Dictionary<int, LinkedList<GameObject>> group, MRenderEventArgs arg)
        {
            if (group.Count == 0) return;

            //TODO:解决创建组合模型时集合被修改的问题
            var keys = group.Keys.ToArray();
            foreach (var theSameTypeId in keys) //group.Keys
            {
                var theSameType = group[theSameTypeId];
                //var modelMatGrp = theSameType
                //    .Where(o => o.Visible)
                //    .Select(o => o.GetModelMatrixFromParent())
                //    .ToArray();
                //var modelMatGrp = theSameType
                //                    .AsParallel()
                //                    .Where(o => o.Visible)
                //                    .Select(o => o.GetModelMatrixFromParent())
                //                    .ToArray();
                //var visibleObjs = theSameType.Where(o => o.Visible);
                var modelMatGrp = new UnmanagedArray<mat4>(theSameType.Count);
                mat4* ptr = (mat4*)modelMatGrp.Header;

                var count_use_batch = Copy(theSameType
                    .Where(o => o.Visible)
                    .Select(o => o.GetModelMatrixFromParent()), ptr);
                var batch_count = count_use_batch / MAX_INSTANCE_COUNT_ONCE + 1;

                GameState.visibleVoxelAcc += count_use_batch;

                var modelMats = new mat4[MAX_INSTANCE_COUNT_ONCE];
                for (int i = 0; i < batch_count; i++, count_use_batch -= MAX_INSTANCE_COUNT_ONCE)
                {
                    if (theSameType.First == null)
                        break;
                    var firstObj = theSameType.First.Value;
                    //必须先初始化，才能得到计算矩阵的必要数据
                    //if (!firstObj.IsInitialized) { firstObj.Initialize(); }
                    //拷贝矩阵到托管数组中

                    if (MAX_INSTANCE_COUNT_ONCE > count_use_batch)
                    {
                        Copy(ptr, modelMats, i * MAX_INSTANCE_COUNT_ONCE, count_use_batch);
                        //Array.Copy(modelMatGrp, i * MAX_INSTANCE_COUNT_ONCE, modelMats, 0, count_use_batch);
                    }
                    else
                    {
                        Copy(ptr, modelMats, i * MAX_INSTANCE_COUNT_ONCE, MAX_INSTANCE_COUNT_ONCE);
                        //Array.Copy(modelMatGrp, i * MAX_INSTANCE_COUNT_ONCE, modelMats, 0, MAX_INSTANCE_COUNT_ONCE);
                    }
                    //modelMats = modelMatGrp
                    //.Skip(i * MAX_INSTANCE_COUNT_ONCE)
                    //.Take(MAX_INSTANCE_COUNT_ONCE)
                    //.ToArray();
                    //TODO:确认一下是不是：同一个RenderUnit的不同方法都是针对同一种模型进行渲染，且有固定的cmd调用流程(不同的cmd只为一种渲染策略服务)
                    arg.Obj = modelMats;
                    arg.MethodName = RenderStrategy.INSTANCING;
                    firstObj.RenderSelf(arg);
                }
                //释放非托管数组
                modelMatGrp.Dispose();
            }
        }

        private unsafe static int Copy(IEnumerable<mat4> src, mat4* dest)
        {
            int count = 0;
            var temp = dest;
            foreach (var item in src)
            {
                *temp = item;
                temp++;
                count++;
            }
            return count;
        }

        /// <summary>
        /// 从某个指针处开始向目标数组拷贝数据
        /// </summary>
        /// <param name="src">源指针</param>
        /// <param name="dest">目标数组</param>
        /// <param name="offset">指针基础偏移量</param>
        /// <param name="length">实际需要拷贝的长度</param>
        private unsafe static void Copy(mat4* src, mat4[] dest, int offset, int length)
        {
            for (int i = 0; i < length; i++)
            {
                dest[i] = *(src + offset + i);
            }
            for (int i = length; i < dest.Length; i++)
            {
                dest[i] = *(src + offset + length - 1);
            }
        }


        public override RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            GLState.CullFace.On();
            RenderGroups(groupsArray[(int)TransparentMode.Opaque], arg);
            //GLState.CullFace.Off();

            GLState.DepthMask.On();
            var blending = GLState.Blending;
            blending.SourceFactor = BlendSrcFactor.SrcAlpha;
            blending.DestFactor = BlendDestFactor.OneMinusSrcAlpha;
            GLState.Blending.On();

            //GLState.CullFace.On();
            RenderGroups(groupsArray[(int)TransparentMode.Transparent], arg);
            GLState.CullFace.Off();

            RenderGroups(groupsArray[(int)TransparentMode.SemiTransparent], arg);
            GLState.Blending.Off();
            GLState.DepthMask.Off();

            return null;
        }
    }
}
