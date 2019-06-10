using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static MiniCraft.Core.MathExt;
using fNbt;

namespace MiniCraft.Core.Game
{
    public static class CompoundCreator
    {
        private static vec3 pos1, pos2;
        //pos1到pos2的各坐标变化量，可为负数
        private static int dx, dy, dz;
        private static int seq = 0;
        private static byte[] blockBuffer;

        //当前读取的compound文件
        static NbtFile myFile;
        static NbtCompound rootTag;

        public static void LoadFile(string fileName)
        {
            myFile = new NbtFile();
            myFile.LoadFromFile(AssetManager.GetNBTFilePath(fileName, "Compound"));
            rootTag = myFile.RootTag;
        }

        public static string GetTagName(string compoundName)
        {
            return $"compound_{compoundName}";
        }

        public static void LoadCompound(string compoundName)
        {
            if(myFile == null)
                LoadFile("compounds_01");
            var compoundTag = rootTag[GetTagName(compoundName)];
            blockBuffer = compoundTag["blocks"].ByteArrayValue;
            var delta = vec3.Parse(compoundTag["delta"].StringValue);
            dx = (int)delta.x; dy = (int)delta.y; dz = (int)delta.z;
        }

        public static void SaveCompound(string compoundName)
        {
            if (myFile == null)
                LoadFile("compounds_01");

            var tagName = GetTagName(compoundName);
            var compoundTag = new NbtCompound();
            compoundTag.Add(new NbtString("delta", new vec3(dx, dy, dz).ToString()));
            compoundTag.Add(new NbtString("name", compoundName));
            compoundTag.Add(new NbtByteArray("blocks", blockBuffer));
            NBTChunkManager.UpdateTag(rootTag, tagName, compoundTag);

            myFile.SaveToFile(myFile.FileName, NbtCompression.GZip);
        }

        public static void SamplePosition(vec3 pos)
        {
            if (seq % 2 == 0)
                pos1 = pos;
            else
            {
                pos2 = pos;
                //dx = (int)Math.Abs(pos2.x - pos1.x);
                //dy = (int)Math.Abs(pos2.y - pos1.y);
                //dz = (int)Math.Abs(pos2.z - pos1.z);
            }
            seq = (seq + 1) % 2;
        }


        public static void CopyVoxels(WorldNode world, vec3 pos1, vec3 pos2)
        {
            pos1 = Floor(pos1);
            pos2 = Floor(pos2);
            dx = (int)(pos2.x - pos1.x); dy = (int)(pos2.y - pos1.y); dz = (int)(pos2.z - pos1.z);
            int di = dx > 0 ? 1 : -1, dj = dy > 0 ? 1 : -1, dk = dz > 0 ? 1 : -1;
            //重新确定长宽高，至少1X1X1
            dx += di; dy += dj; dz += dk;
            //dx = ; dy++; dz++;
            blockBuffer = new byte[Abs(dx * dy * dz)];

            for (int i = 0; i < Abs(dx); i++)
            {
                for (int j = 0; j < Abs(dy); j++)
                {
                    for (int k = 0; k < Abs(dz); k++)
                    {
                        var p = pos1 + new vec3(i * di, j * dj, k * dk);
                        var index = Utility.Ivec3ToLinerIndex(new ivec3(i, j, k), Abs(dx), Abs(dz));
                        var chunk = world[p];
                        var block = chunk[p];
                        blockBuffer[index] = (byte)block.TypeId;//(byte)AssetManager.GetObjId(block.GetType());
                    }
                }
            }
        }

        public static void CopyVoxels(WorldNode world)
        {
            CopyVoxels(world, pos1, pos2);
        }

        public static void PasteVoxels(Chunk chunk, ivec3 rpos, bool ignoreAir = true)
        {
            int lx = Abs(dx), ly = Abs(dy), lz = Abs(dz);
            DoForAllVoxels((i, j, k) =>
            {
                var p = rpos + new ivec3(i - lx / 2, j + 1, k - lz / 2);
                int id = blockBuffer[Utility.Ivec3ToLinerIndex(new ivec3(i, j, k), Abs(dx), Abs(dz))];
                if (!(id == 0 && ignoreAir))
                    AssetManager.GenVoxelAt(chunk, id, p);
                return true;
            });
        }


        public static void PasteVoxels(WorldNode world, vec3 pos, bool ignoreAir=true)
        {
            int lx = Abs(dx), ly = Abs(dy), lz = Abs(dz);
            DoForAllVoxels((i, j, k) =>
            {
                var p = pos + new vec3(i - lx / 2, j + 1, k - lz / 2);
                int id = blockBuffer[Utility.Ivec3ToLinerIndex(new ivec3(i, j, k), Abs(dx), Abs(dz))];
                if (!(id == 0 && ignoreAir))
                    AssetManager.GenVoxelAt(id, world, p);
                return true;
            });
        }

        public static void ClearVoxels(WorldNode world)
        {
            pos1 = Floor(pos1);
            pos2 = Floor(pos2);
            dx = (int)(pos2.x - pos1.x); dy = (int)(pos2.y - pos1.y); dz = (int)(pos2.z - pos1.z);
            int di = dx > 0 ? 1 : -1, dj = dy > 0 ? 1 : -1, dk = dz > 0 ? 1 : -1;
            dx += di; dy += dj; dz += dk;
            DoForAllVoxels((i, j, k) =>
            {
                var p = pos1 + new vec3(i * di, j * dj, k * dk);
                var old = world[p][p];
                if(old != VoxelBase.None)
                {
                    old.OnDestroy(null, new MEventArgs());
                    world[p][p] = VoxelBase.None;
                }
                return true;
            });
        }


        private static void DoForAllVoxels(Func<int, int, int, bool> actionPerBlock)
        {
            int lx = Abs(dx), ly = Abs(dy), lz = Abs(dz);
            for (int j = 0; j < ly; j++)
            {
                for (int i = 0; i < lx; i++)
                {
                    for (int k = 0; k < lz; k++)
                    {
                        if (!actionPerBlock(i, j, k))
                            break;
                    }
                }
            }
        }
    }
}
