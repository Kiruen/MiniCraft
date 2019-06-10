using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    public static class Commander
    {
        static List<Action> commandList = new List<Action>(16);

        static object syncRoot = new object();

        public static void Parse(string cmd)
        {
            if (cmd == null || cmd.Length == 0)
                return;
            try
            {
                if (!cmd.StartsWith("/"))
                    return;
                else
                    cmd = cmd.Replace("/", "");

                var args = cmd.Split(' ');
                var cmdName = args[0].ToLower();
                args = args.Skip(1).ToArray();
                if (cmdName == "tp")
                    TP(GameState.player, GameState.currWorld, vec3.Parse(args[0]));
                else if (cmdName == "sethome")
                    GameState.homePosition = args.Length > 0 ? vec3.Parse(args[0]) : GameState.currPosition;
                else if (cmdName == "home")
                    TP(GameState.player, GameState.currWorld, GameState.homePosition);
                else if (cmdName == "clear")
                    Clear(args);
                else if (cmdName == "load")
                    Load(args);
                else if (cmdName == "save")
                    Save(args);
            }
            catch { }
        }

        public static void TP(Mob obj, WorldNode world, vec3 pos)
        {
            Push(/*async */() =>
            {
                obj.WorldPosition = pos;
                //var cache = GameState.currWorld.ChunkCache;
                //await Task.Factory.StartNew(async () =>
                //{
                //    int count = await cache.Update(Utility.PosToChunkIndex(pos), true);
                //    cache.Flush();
                //    obj.WorldPosition = pos;
                //});
            });
        }

        public static void Clear(params string[] args)
        {
            Push(() =>
            {
                if(args.Length == 2)
                {
                    CompoundCreator.SamplePosition(vec3.Parse(args[0]));
                    CompoundCreator.SamplePosition(vec3.Parse(args[1]));
                }
                CompoundCreator.ClearVoxels(GameState.currWorld);
            });
        }

        public static void Load(params string[] args)
        {
            Push(() =>
            {
                if (args[0] == "comp" && args.Length == 2)
                {
                    CompoundCreator.LoadCompound(args[1]);
                }
            });
        }

        public static void Save(params string[] args)
        {
            Push(() =>
            {
                if (args[0] == "comp")
                {
                    var name = args.Length == 2 ? args[1] : DateTime.Now.ToLongDateString();
                    CompoundCreator.SaveCompound(name);
                }
            });
        }

        public static void Push(Action action)
        {
            lock (syncRoot)
            {
                commandList.Add(action);
            }
        }

        public static void Handle()
        {
            lock(syncRoot)
            {
                try
                {
                    foreach (var command in commandList)
                    {
                        command();
                    }
                    commandList.Clear();
                }
                catch { }
            }
        }
    }
}
