using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MiniCraft.Core.Game.GameObject;
namespace MiniCraft.Core.Game
{
    /// <summary>
    /// 透明渲染批处理结点
    /// </summary>
    public class InstancingBatchNode : BatchNode
    {
        public const int MAX_INSTANCE_COUNT_ONCE = 192;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public InstancingBatchNode(IEnumerable<GameObject> objects)
            : base(objects)
        {
            //EnableRendering = RenderingFlags.BeforeChildren | RenderingFlags.Children | RenderingFlags.AfterChildren;
        }

        public void Test(MRenderEventArgs arg)
        {
            foreach (var item in objects)
            {
                uint id = (uint)item.Id;
                QueryManager.BeginQuery(QueryTarget.SamplesPassed, id);
                item.Physics.Wrapper.RenderSelf(arg);
                QueryManager.EndQuery(QueryTarget.SamplesPassed);
                if (QueryManager.SampleRendered(id))
                    item.RenderSelf(arg);
            }
        }

        #region 失败的优化
                static mat4[] modelMats;
                public RenderMethod RenderSelf1(MRenderEventArgs arg, bool doRender = true)
                {
                    GameObject firstObj = null;
                    foreach (var item in objects)
                    {
                        firstObj = item;
                        break;
                    }
                    if (firstObj == null)
                        return null;
            
                    //必须先初始化，才能得到计算矩阵的数据
                    if (!firstObj.IsInitialized) { firstObj.Initialize(); }

                    if(modelMats == null)
                        modelMats = objects
                            .Select(o => o.GetModelMatrixFromParent())
                            .ToArray();

                    GameState.visibleVoxelAcc += modelMats.Length;
                    //TODO:确认一下是不是：同一个RenderUnit的不同方法都是针对同一种模型进行渲染，且有固定的cmd调用流程(不同的cmd只为一种渲染策略服务)
                    arg.Obj = modelMats;
                    arg.MethodName = RenderStrategy.INSTANCING;
                    firstObj.RenderSelf(arg);
                    return null;
                }   
        #endregion
        public override RenderMethod RenderSelf(MRenderEventArgs arg, bool doRender = true)
        {
            foreach (var theSameType in objects.GroupBy(o => o.GetType()))
            {
                var count = theSameType.Count();
                var count_use_batch = count; // count - count % MAX_INSTANCE_COUNT_ONCE;
                var batch_count = count_use_batch / MAX_INSTANCE_COUNT_ONCE + 1;
                for (int i = 0; i < batch_count; i++)
                {
                    var firstObj = theSameType.ElementAt(0);
                    //必须先初始化，才能得到计算矩阵的数据
                    if (!firstObj.IsInitialized) { firstObj.Initialize(); }

                    var modelMats = theSameType
                        .Skip(i * MAX_INSTANCE_COUNT_ONCE)
                        .Take(MAX_INSTANCE_COUNT_ONCE)
                        .Select(o => o.GetModelMatrixFromParent())
                        .ToArray();

                    GameState.visibleVoxelAcc += modelMats.Length;
                    //TODO:确认一下是不是：同一个RenderUnit的不同方法都是针对同一种模型进行渲染，且有固定的cmd调用流程(不同的cmd只为一种渲染策略服务)
                    arg.Obj = modelMats;
                    arg.MethodName = RenderStrategy.INSTANCING;
                    firstObj.RenderSelf(arg);
                }
            }
            return null;
        }
    }
}
