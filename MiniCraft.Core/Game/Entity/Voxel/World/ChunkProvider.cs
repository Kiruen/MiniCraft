using CSharpGL;
using fNbt;
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
    public interface IChunkProvider
    {
        Chunk Fetch(WorldNode parentWorld, ivec3 index);
    }

    /// <summary>
    /// 由NBT文件获取chunk数据的提供器
    /// </summary>
    public class NBTChunkManager : IChunkProvider
    {
        //static bool fileLoaded = false;
        NbtFile myFile;
        NbtCompound rootTag;
        public Func<int, VoxelBase> generator;

        public NBTChunkManager(string fileName)
        {
            myFile = new NbtFile();
            myFile.LoadFromFile(fileName);
            rootTag = myFile.RootTag;
        }

        /// <summary>
        /// 加载数据到代表玩家的对象中
        /// </summary>
        public void LoadPlayerData()
        {
            var playerDataTag = rootTag["player_data"];
            GameState.player.WorldPosition = vec3.Parse(playerDataTag["position"].StringValue);
            GameState.homePosition = vec3.Parse(playerDataTag["home_position"].StringValue);
            //GameState.player.Yaw
        }

        private static string GetChunkTag(ivec3 index)
        {
            return $"chunk {index.x},{index.y},{index.z}";
        }

        public Chunk Fetch(WorldNode parentWorld, ivec3 index)
        {
            //Write();
            //JsonTextReader jtr = new JsonTextReader(new StreamReader(""));
            //jtr.Read();
            //var chunk = myCompoundTag.Skip(6 * 6 * 1).ElementAt(0);
            var tagName = GetChunkTag(index);
            Chunk chunk = null;
            if (rootTag.Contains(tagName))
            {
                chunk = new Chunk(index, parentWorld);
                var chunkInfo = rootTag[tagName];
                var blocks = chunkInfo["blocks"].ByteArrayValue;
                var face_states = chunkInfo["face_states"].ByteArrayValue;

                for (int i = 0; i < blocks.Length; i++)
                {
                    var rpos = Utility.LinerIndexToVoxelIndex(i);
                    //var block = generator(blocks[i]);
                    var block = AssetManager.GenVoxel(blocks[i]);
                    //如果不是空气方块，则加载一些元数据
                    if (block != VoxelBase.None)
                    {
                        block.FaceState.StateValue = face_states[i];
                        chunk[rpos.x, rpos.y, rpos.z] = block;
                        //如果是联动装置，则触发组件连接事件
                        if (block is ILinkable link)
                        {
                            link.OnLinkage(null, null);
                        }
                    } 
                }
                //从文件加载的区块视为未修改
                chunk.Modified = false;
            }
            else
            {
                //如果文件中没有，就调用地形生成函数生成一个区块
                chunk = GenChunk(parentWorld, index);
                //新生成的区块视为已修改
                chunk.Modified = true;
            }
            return chunk;
        }

        public void Update(Chunk chunk)
        {
            //不更新空区块和未修改的区块
            if (chunk.Temporary || !chunk.Visible || !chunk.Modified)
                return;

            var tagName = GetChunkTag(chunk.Index);

            if (!rootTag.Contains(tagName))
                rootTag.Add(new NbtCompound(tagName));

            var chunkTag = rootTag[tagName] as NbtCompound;

            //TODO:从注册表中取得ID
            UpdateTag(chunkTag, "blocks", new NbtByteArray(
                chunk.VoxelsWithStandardOrder
                .Select(v => 
                (byte)(v.TypeId))    //AssetManager.GetObjId(v.GetType())
                .ToArray()));
            UpdateTag(chunkTag, "face_states",
                new NbtByteArray(
                chunk.VoxelsWithStandardOrder
                .Select(v => (byte)(v == VoxelBase.None ? 0 : v.FaceState.StateValue))
                .ToArray()));
        }

        public static void UpdateTag(NbtCompound parent, string tagName, NbtTag value)
        {
            value.Name = tagName;
            if (!parent.Contains(tagName))
                parent.Add(value);
            else
                parent[tagName] = value;
        }

        public void Save(IEnumerable<Chunk> data)
        {
            try
            {
                //data.AsParallel().ForAll(c => Update(c));
                foreach (var chunk in data)
                {
                    Update(chunk);
                }
                UpdateTag(rootTag["player_data"] as NbtCompound, "position",
                        new NbtString(GameState.player.WorldPosition.ToString()));
                UpdateTag(rootTag["player_data"] as NbtCompound, "home_position",
                        new NbtString(GameState.homePosition.ToString()));

                myFile.SaveToFile(myFile.FileName, NbtCompression.GZip);
            }
            catch { }
        }

        int worldBottom = -64;
        public Chunk GenChunk(WorldNode world, ivec3 chunkIndex)
        {
            var heightBuffer = new int[CHUNK_X_LEN, CHUNK_Z_LEN];
            heightBuffer.FillArray(-CHUNK_Y_LEN);
            var chunk = new Chunk(chunkIndex, world);
            bool isTopPlantable = true, isEmpty = false;
            for (int rx = -CHUNK_X_HALF_LEN; rx < CHUNK_X_HALF_LEN; rx++)
            {
                for (int rz = -CHUNK_Z_HALF_LEN; rz < CHUNK_Z_HALF_LEN; rz++)
                {
                    //获取chunk底部的世界坐标y分量
                    var gpos = Utility.ToGlobalPos(chunk, rx, -CHUNK_Z_HALF_LEN, rz);
                    float chunkBottom = gpos.y, 
                        chunkTop = chunkBottom + CHUNK_Y_LEN - 1;
                    var noise = GenBiome(gpos);
                    int height = (int)(90 + noise * 30);
                    int pTop = worldBottom + height;
                    int remaingCount = (int)(pTop - chunkBottom);
                    //若该chunk不再需要生成基础体素，则跳过。当然，也可以进行发挥
                    //if(inChunkCount <= 0)
                    //    return chunk;
                    int ry;
                    for (ry = -CHUNK_Y_HALF_LEN; ry < -CHUNK_Y_HALF_LEN + remaingCount; ry++, gpos.y++)
                    {
                        //y在0以下的部分填充石头
                        if(gpos.y < 0)
                            AssetManager.GenVoxelAt(chunk, 1, rx, ry, rz);
                        else
                            AssetManager.GenVoxelAt(chunk, 12, rx, ry, rz);
                        //到达该区块的最大高度，结束填充
                        if (ry >= CHUNK_Y_HALF_LEN - 1) break;
                    }
                    //如果在当前坐标上没有填充任何方块，则判定为空区块
                    isEmpty = remaingCount <= 0;
                    //若该chunk未填满，则说明到达了该坐标处的最大高度，其上面的部分可以自由发挥
                    if (ry < CHUNK_Y_HALF_LEN)
                    {
                        //如果不是空区块
                        if (!isEmpty)
                        {
                            //铺草块
                            AssetManager.GenVoxelAt(chunk, 3, rx, ry++, rz);
                            //铺花草
                            if (gpos.y++ >= 18 && ry < CHUNK_Y_HALF_LEN)
                            {
                                var noise2 = Math.Abs(Perlin.PerlinNoise(rx * 0.1f, rz * 0.1f, 0.5F, 4));
                                if (noise2 > 0.17f)
                                {
                                    GenPlant(chunk, rx, ry++, rz);
                                }
                            }
                        }
                        for (; ry < CHUNK_Y_HALF_LEN && gpos.y < 18; ry++, gpos.y++)
                        {
                            AssetManager.GenVoxelAt(chunk, 4, rx, ry, rz);
                            isTopPlantable = false;
                        }
                    }
                    heightBuffer[rx + CHUNK_X_HALF_LEN, rz + CHUNK_Z_HALF_LEN] = ry;
                }
            }
            isTopPlantable &= !isEmpty;
            return isTopPlantable ? DecorateChunk(chunk, heightBuffer) : chunk;
        }

        static Random ran = new Random();
        public void GenPlant(Chunk chunk, int rx, int ry, int rz)
        {
            var num = ran.Next();
            int typeCount = 5;
            if (num % typeCount == 0)
                AssetManager.GenVoxelAt(chunk, AssetManager.PLANT1, rx, ry, rz);
            else if (num % typeCount == 1)
                AssetManager.GenVoxelAt(chunk, AssetManager.PLANT2, rx, ry, rz);
            else if (num % typeCount == 2)
                AssetManager.GenVoxelAt(chunk, AssetManager.PLANT3, rx, ry, rz);
            else if (num % typeCount == 3)
                AssetManager.GenVoxelAt(chunk, AssetManager.PLANT4, rx, ry, rz);
            else if (num % typeCount == 4)
                AssetManager.GenVoxelAt(chunk, AssetManager.PLANT5, rx, ry, rz);
        }

        public static double GenBiome(vec3 gpos)
        {
            if (gpos.x.InRange(-250, 250))
                return Perlin.PerlinNoise(gpos.x * 0.01f, gpos.z * 0.01f, 0.8, 4);
            else if (gpos.z.InRange(-250, 250))
                return Perlin.PerlinNoise(gpos.x * 0.01f, gpos.z * 0.01f, 1.2, 4);
            else if (gpos.z.InRange(500, 1000))
                return Perlin.PerlinNoise(gpos.x * 0.01f, gpos.z * 0.01f, 1.6, 4);
            else
                return Perlin.PerlinNoise(gpos.x * 0.01f, gpos.z * 0.01f, 2, 4);
        }

        public Chunk DecorateChunk(Chunk chunk, int[,] heightBuffer)
        {
            ivec2[] array = Utility.GenRandomIVec2s(Utility.GenRandom(1, 8), -8, 8);
            foreach (var vec in array)
            {
                int rx = vec.x, rz = vec.y;
                var index = Utility.RPosToArrayIndex(rx, 0, rz);
                var ry = heightBuffer[index.x, index.z];
                if (rx.InRange(-5, 5) && rz.InRange(-5, 5) && ry.InRange(-6, 3))
                {
                    //TODO:一开始还有，然后一瞬间就没树干了
                    CompoundCreator.LoadCompound("tree");
                    var rpos = new ivec3(rx, ry - 2, rz);
                    CompoundCreator.PasteVoxels(chunk, rpos);
                    //return chunk;
                }
            }
            return chunk;
        }

        public void GenWorld1(WorldNode world)
        {
            for (int i = -3; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = -3; k < 3; k++)
                    {
                        var chunkInfo = new NbtCompound($"chunk {i},{j},{k}");
                        chunkInfo.Add(new NbtString("Wheather", "Sunny"));
                        chunkInfo.Add(new NbtInt("Size", 15));
                        chunkInfo.Add(new NbtIntArray("index", new[] { i, j, k }));
                        chunkInfo.Add(new NbtByteArray("blocks", new byte[] { 1 }));
                        rootTag.Add(chunkInfo);
                    }
                }
            }

            var serverFile = new NbtFile(rootTag);

            //serverFile.SaveToFile("server2.nbt", NbtCompression.GZip);
        }
    }
}
