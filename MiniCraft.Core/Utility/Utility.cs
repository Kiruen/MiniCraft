using CSharpGL;
using IrrKlang;
using MiniCraft.Core.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MiniCraft.Core.Constant;
using static MiniCraft.Core.MathExt;

namespace MiniCraft.Core
{
    public static partial class Utility
    {
        //public static bool InRange<T>(this T x, T a, T b) where T: struct
        //{
        //    return a <= x && x < b;
        //}
        public static Random Random { get; private set; } = new Random();


        public static bool InRange(this int x, int a, int b)
        {
            return a <= x && x < b;
        }

        public static bool InRange(this float x, float a, float b)
        {
            return a <= x && x < b;
        }

        public static ivec3 RPosToArrayIndex(ivec3 rpos)
        {
            return rpos + new ivec3(CHUNK_X_HALF_LEN, CHUNK_Y_HALF_LEN, CHUNK_Z_HALF_LEN);
        }

        public static ivec3 RPosToArrayIndex(int x, int y, int z)
        {
            return new ivec3(x + CHUNK_X_HALF_LEN, y + CHUNK_Y_HALF_LEN, z + CHUNK_Z_HALF_LEN);
        }

        public static vec3 ChunkIndexToPos(int x, int y, int z)
        {

            return new vec3(x * CHUNK_X_LEN, y * CHUNK_Y_LEN, z * CHUNK_Z_LEN);
        }

        public static vec3 ChunkIndexToPos(ivec3 index)
        {
            return new vec3(index.x * CHUNK_X_LEN, index.y * CHUNK_Y_LEN, index.z * CHUNK_Z_LEN);
        }


        public static ivec3 PosToChunkIndex(vec3 gpos)
        {
            return PosToChunkIndex(gpos.x, gpos.y, gpos.z);
        }

        public static ivec3 PosToChunkIndex(float gx, float gy, float gz)
        {
            int ix = MathExt.Floor((gx + CHUNK_X_HALF_LEN) / CHUNK_X_LEN),
                iy = MathExt.Floor((gy + CHUNK_Y_HALF_LEN) / CHUNK_Y_LEN),
                iz = MathExt.Floor((gz + CHUNK_Z_HALF_LEN) / CHUNK_Z_LEN);
            return new ivec3(ix, iy, iz);
        }

        public static ivec3 LinerIndexToVoxelIndex(int i)
        {
            int ry = i / (CHUNK_X_LEN * CHUNK_Z_LEN) - CHUNK_Y_HALF_LEN,
                rz = (i % (CHUNK_X_LEN * CHUNK_Z_LEN)) / CHUNK_X_LEN - CHUNK_Z_HALF_LEN,
                rx = (i % (CHUNK_X_LEN * CHUNK_Z_LEN)) % CHUNK_X_LEN - CHUNK_X_HALF_LEN;
            return new ivec3(rx, ry, rz);
        }

        public static ivec3 LinerIndexToVoxelIndex(int i, int xhlen, int yhlen, int zhlen)
        {
            int ry = i / (xhlen * zhlen) - yhlen,
                rz = (i % (xhlen * zhlen)) / xhlen - zhlen,
                rx = (i % (xhlen * zhlen)) % xhlen - xhlen;
            return new ivec3(rx, ry, rz);
        }

        public static int VoxelIndexToLinerIndex(ivec3 gridIndex, int xhlen, int yhlen, int zhlen)
        {
            return (gridIndex.y + yhlen) * xhlen * 2 * zhlen * 2 + 
                    (gridIndex.z + zhlen) * xhlen * 2 + 
                    gridIndex.x + xhlen;
        }

        public static int Ivec3ToLinerIndex(ivec3 vec, int xlen, int zlen)
        {
            return vec.y * xlen * zlen + vec.z * xlen + vec.x;
        }

        //TODO;测试
        public static vec3 ToGlobalPos(Chunk chunk, int rx, int ry, int rz)
        {
            var chunkPos = chunk.WorldPosition;
            return new vec3(rx + chunkPos.x + 0.5f, ry + chunkPos.y + 0.5f, rz + chunkPos.z + 0.5f);
        }

        //public static vec3 ChunkIndexToPos(int ix, int iy, int iz)
        //{
        //    return new vec3(ix, iy, iz);
        //}

        //TODO:实现探测区域的函数
        const int threshold = 3;
        public static bool IsNearChunkBound(vec3 gpos, Chunk chunk)
        {
            return Math.Abs(gpos.x - chunk.WorldPosition.x) >= CHUNK_X_HALF_LEN - threshold ||
                Math.Abs(gpos.y - chunk.WorldPosition.y) >= CHUNK_Y_HALF_LEN - threshold ||
                Math.Abs(gpos.z - chunk.WorldPosition.z) >= CHUNK_Z_HALF_LEN - threshold;
        }

        public static bool IsPointInCuboid(vec3 pos, vec3 cubeCenter, vec3 halfLenth)
        {
            return cubeCenter.x - halfLenth.x <= pos.x && pos.x <= cubeCenter.x + halfLenth.x &&
                cubeCenter.y - halfLenth.y <= pos.y && pos.y <= cubeCenter.y + halfLenth.y &&
                cubeCenter.z - halfLenth.z <= pos.z && pos.z <= cubeCenter.z + halfLenth.z;
        }

        public static int ExtractSign(this float num)
        {
            return num == 0 ? 0 : (num > 0 ? 1 : -1);
        }

        public static Point Center(this Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }

        public static Point ToPoint(this ivec2 vecp)
        {
            return new Point(vecp.x, vecp.y);
        }

        public static ivec2 ToIVec2(this Point p)
        {
            return new ivec2(p.X, p.Y);
        }

        public static string ToTitle(string s)
        {
            return $"{char.ToUpper(s[0])}{s.Substring(1).ToLower()}";
        }

        public static int GenRandom(int a, int b)
        {
            return (int)Lerp(a, b, Random.NextDouble());
        }

        public static void FillArray<T>(this T[,] array, T val)
        {
            int a = array.GetLength(0), b = array.GetLength(1);
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    array[i, j] = val;
                }
            }
        }

        /// <summary>
        /// 产生一组随机的二维坐标。
        /// </summary>
        /// <param name="count">坐标个数</param>
        /// <param name="rangeA">左区间，闭</param>
        /// <param name="rangeB">右区间，开</param>
        /// <returns></returns>
        public static ivec2[] GenRandomIVec2s(int count, int rangeA, int rangeB)
        {
            rangeB--;
            return Enumerable.Range(0, count)
                .Select(n =>
                {
                    return new ivec2((int)Lerp(rangeA, rangeB, Random.NextDouble()),
                        (int)Lerp(rangeA, rangeB, Random.NextDouble()));
                })
            .ToArray();
        }
    }
}

