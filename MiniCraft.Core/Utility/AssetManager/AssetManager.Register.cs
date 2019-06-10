using CSharpGL;
using MiniCraft.Core.Game;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core
{
    public static partial class AssetManager
    {
        public const byte PLANT1 = 201,
                        PLANT2 = 202,
                        PLANT3 = 203,
                        PLANT4 = 204,
                        PLANT5 = 205;

        //public static int RegObjCount { get => IDToType.Count; }
        static JArray jsonVoxels;
        static AssetManager()
        {
            var _namespace = "MiniCraft.Core.Game";
            var path = GetJSONFilePath(@"Config\voxels");
            if(File.Exists(path))
            {
                jsonVoxels = (JArray)JToken.ReadFrom(
                    new JsonTextReader(new StreamReader(path)))["voxels"];
                foreach (var item in jsonVoxels.Values<JObject>())
                {
                    var system_type = Type.GetType($"{_namespace}.{item["system_type"].Value<string>()}", true, false);
                    var tex_name = item["texure_name"].Value<string>();
                    var type_id = item["type_id"].Value<int>();
                    Register(system_type, type_id, tex_name);
                }
            }
            else
            {
                Register(typeof(Block), 1, "stone");
                Register(typeof(SandBlock), 2, "sand");
                Register(typeof(GrassBlock), 3, "grass_block");
                Register(typeof(Water), 4, "water_still");
                Register(typeof(VertiHalfDoor), 5, "door_wood_upper");
                Register(typeof(VertiHalfDoor), 6, "door_wood_lower");
                Register(typeof(BrickBlock), 8, "brick");
                Register(typeof(GlassBlock), 9, "glass");
                Register(typeof(LogBlock), 10, "log_oak");
                Register(typeof(LeafBlock), 11, "leaves_spruce");
                Register(typeof(Block), 12, "dirt");
                Register(typeof(CrossPlant), 201, "plant1");
                Register(typeof(CrossPlant), 202, "plant2");
                Register(typeof(CrossPlant), 203, "plant3");
                Register(typeof(CrossPlant), 204, "plant4");
                Register(typeof(CrossPlant), 205, "plant5");
            }
            Register(typeof(AirBlock), 0);
        }

        public static readonly Dictionary<Type, int> typeToID = new Dictionary<Type, int>(16);
        public static readonly Dictionary<int, Type> IDToType = new Dictionary<int, Type>(16);
        public static readonly Dictionary<int, string> IDToName = new Dictionary<int, string>(16);

        public static void Register(Type objType, int id, string name= "stone")
        {
            if(!typeToID.ContainsKey(objType))
                typeToID.Add(objType, id);
            if(!IDToType.ContainsKey(id))
                IDToType.Add(id, objType);
            if (!IDToName.ContainsKey(id))
                IDToName.Add(id, name);
            
        }

        public static string GetObjName(int id)
        {
            if (!IDToName.ContainsKey(id))
                id = 1;
            return IDToName[id];
        }

        private static Type GetObjType(int id)
        {
            if (!IDToType.ContainsKey(id))
                id = 1;
            return IDToType[id];
        }

        public static int GetObjId(Type t)
        {
            return typeToID[t];
        }

        /// <summary>
        /// 只产生体素对象，不触发任何事件
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static VoxelBase GenVoxel(int typeId)
        {
            var voxel = typeId == 0 ? VoxelBase.None : Activator.CreateInstance(GetObjType(typeId)) as VoxelBase;
            //voxel.Texture = GetTexture(@"stone");
            voxel.TypeId = typeId;
            return voxel;
            //else if (typeId == 1)
            //    return new Block();
            //else if (typeId == 2)
            //    return new SandBlock();//new VertiHalfDoor();
            //else if (typeId == 3)
            //    return new GrassBlock();//new VertiHalfDoor();
            //else if (typeId == 4)
            //    return new Water();
            //else if (typeId == 5)
            //    return new VertiHalfDoor() { Part = DoorPart.Up };
            //else if (typeId == 6)
            //    return new VertiHalfDoor() { Part = DoorPart.Down };
            //else if (typeId == 7)
            //    return new CrossPlant();
            //else 
            //    return new BrickBlock();
        }

        /// <summary>
        /// 产生体素对象，放置在指定世界的指定位置中，并模拟被放置的事件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="world"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static VoxelBase GenVoxelAt(int id, WorldNode world, vec3 p)
        {
            var b = GenVoxel(id);
            var chunk = world[p];
            chunk[p] = b;
            if(b.Parent != null && !chunk.Temporary && b != VoxelBase.None)
                b.OnSettled(null, new MVoxelEventArgs());
            return b;
        }

        public static VoxelBase GenVoxelAt(Chunk chunk, int id, ivec3 rpos)
        {
            return GenVoxelAt(chunk, id, rpos.x, rpos.y, rpos.z);
        }

        /// <summary>
        /// 产生体素对象，放置在指定chunk中，并模拟被放置的事件。通常适合构造新区块时使用。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="world"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static VoxelBase GenVoxelAt(Chunk chunk, int id, int x, int y, int z)
        {
            var b = GenVoxel(id);
            chunk[x, y, z] = b;
            //新生成的方块，是要进行放置操作的。而且还指定了放置事件发生的具体区块！
            //如果超过了chunk界限，则chunk拒绝此体素成为其子对象，不会进行放置操作。
            if (b.Parent != null && !chunk.Temporary && b != VoxelBase.None)
                b.OnSettled(null, new MVoxelEventArgs() { Chunk = chunk });
            return b;
        }
    }
}
