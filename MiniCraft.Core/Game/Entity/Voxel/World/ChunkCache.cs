using CSharpGL;
using MiniCraft.Core.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static MiniCraft.Core.Constant;
using System.Threading;

namespace MiniCraft.Core
{
    /// <summary>
    /// 区块缓存区，以二级缓冲的方式动态装卸区块。
    /// </summary>
    public class ChunkCache : IEnumerable<Chunk>
    {
        private WorldNode bindingWorld;
        private Dictionary<ivec3, Chunk> cached = new Dictionary<ivec3, Chunk>(512);
        private Dictionary<ivec3, Chunk> buffer = new Dictionary<ivec3, Chunk>(512);

        private object SyncRoot = new object();

        public uint BufferingTaskCount { get; set; } 

        public ChunkCache(WorldNode bindingWorld)
        {
            this.bindingWorld = bindingWorld;
        }

        public void Add(ivec3 index, Chunk value)
        {
            //if (!cached.ContainsKey(index))
                cached.Add(index, value);
            //else
                //cached[index] = value;
        }

        public bool TryGetValue(ivec3 index, out Chunk chunk)
        {
            return cached.TryGetValue(index, out chunk);
        }

        public void Flush()
        {
            lock (SyncRoot)
            {
                foreach (var item in buffer)
                {
                    Add(item.Key, item.Value);
                }
                buffer.Clear();
            }
        }

        public void Buffer(Chunk value)
        {
            lock(SyncRoot)
            {
                buffer.Add(value.Index, value);
            }
        }

        public Task task;
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        CancellationToken token;

        public void CancelCurrentTask()
        {
            tokenSource.Cancel();
        }

        public async Task<int> Update(ivec3 centerIndex, bool compelling=false)
        {
            //if(task != null && !task.IsCompleted)
            //{
            //    this.CancelCurrentTask();
            //}
            if (BufferingTaskCount > 0)
            {
                this.Flush();
                if (compelling)
                {
                    this.CancelCurrentTask();
                    tokenSource = new CancellationTokenSource();
                }
                else return 0;
            }
            token = tokenSource.Token;
            return await Task.Run(() =>
            {
                this.BufferingTaskCount += 1;
                var frustum = GameState.MainCamera.ExtractFrustum();
                int loadCount = 0;
                for (int i = 1; i <= RENDER_CHUNK_X_RADIUS * 2 + 1; i++)
                {
                    for (int j = 1; j <= RENDER_CHUNK_Y_RADIUS * 2 + 1; j++)
                    {
                        for (int k = 1; k <= RENDER_CHUNK_Z_RADIUS * 2 + 1; k++)
                        {
                            //TODO:把pos和lastPos放到参数里
                            var delta = pos - lastPos;
                            //centerIndex = GameState.CurrChunkIndex;
                            //TODO:若玩家在某个方向上有位移，则只生成该方向的区块，反方向的区块不再生成
                            ivec3 chunkIndex;
                            chunkIndex.x = centerIndex.x + (delta.x != 0 ? delta.x.ExtractSign() : (i % 2 == 0 ? -1 : 1)) * (i / 2);
                            chunkIndex.y = centerIndex.y + (delta.y != 0 ? delta.y.ExtractSign() : (j % 2 == 0 ? -1 : 1)) * (j / 2);
                            chunkIndex.z = centerIndex.z + (delta.z != 0 ? delta.z.ExtractSign() : (k % 2 == 0 ? -1 : 1)) * (k / 2);
                            //var chunkIndex = new ivec3(x, y, z);
                            //如果缓存中不存在该位置对应的区块，且这个位置处于玩家视野之内
                            if (!buffer.ContainsKey(chunkIndex) && !cached.ContainsKey(chunkIndex) &&
                                //bindingWorld[x, y, z].Temporary == true &&
                                frustum.IsCubeInFrustum
                                    (Utility.ChunkIndexToPos(chunkIndex).ToVec4(), new vec3(CHUNK_X_HALF_LEN)))
                            {
                                loadCount++;
                                var chunk = ChunkLoader.Load(bindingWorld, chunkIndex);
                                //chunk.Parent = world;
                                this.Buffer(chunk);
                                if (token.IsCancellationRequested)
                                    goto cancel;
                            }
                        }
                    }
                }
                cancel: this.BufferingTaskCount -= 1;
                return loadCount;
            });
        }

        public bool ContainsKey(ivec3 key)
        {
            return cached.ContainsKey(key);
        }

        int cleanedCount;
        public int Clean(Func<Chunk, bool> selector)
        {
            int count = 0;
            var values = cached.ToArray();
            foreach (var pair in values)
            {
                var chunk = pair.Value;
                if (selector(chunk))
                {
                    AssetManager.ChunkFileManager.Update(chunk);
                    cached.Remove(pair.Key);
                    count++;
                    cleanedCount++;
                }
                //cached[index] = null;
            }
            if(cleanedCount >= 512) //count > 32
            {
                //Task.Run(() => GC.Collect());
                GC.Collect();
                cleanedCount = 0;
            }
            return count;
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            return cached.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        static vec3 pos, lastPos;
        //static int seq = 0;
        public static void SamplePosition()
        {
            lastPos = pos;
            pos = GameState.player.WorldPosition;
            //else if (seq == 1)
            //    pos = GameState.player.WorldPosition;
            //seq = (seq + 1) % 2;
        }

        public static bool IsUpdateRequested
        {
            get
            {
                var chunk = GameState.currChunk;
                //如果向远离chunk体心的方向移动了，并且接近chunk的边界，则说明需要更新区块
                return Utility.IsNearChunkBound(pos, chunk)
                      && (pos - lastPos).length() > 0;
                     //&& (pos - chunk.WorldPosition).length() > (lastPos - chunk.WorldPosition).length();
            }
        }
        //(index - GameState.CurrChunkIndex).length() > RENDER_CHUNK_X_RADIUS
    }
}
