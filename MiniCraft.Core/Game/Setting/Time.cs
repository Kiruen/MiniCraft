using CSharpGL;
using IrrKlang;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniCraft.Core
{
    public static partial class Time
    {
        public static DateTime StartUpTime { get; private set; }

        public static float RealtimeSinceStartup { get => (float)(DateTime.Now - StartUpTime).TotalSeconds; }

        public static float FixedRealTimeSpan { get; set; } = 0.05f;

        public static int FrameCount { get; private set; }

        public static float DeltaTime { get; private set; }
        //public static float DeltaTime { get =>  }

        private static DateTime lastRealTimePoint;
        public static float DeltaRealTime
        {
            get
            {
                var now = DateTime.Now;
                var delta = (float)(now - lastRealTimePoint).TotalSeconds;
                lastRealTimePoint = now;
                return delta;
            }
        }

        public static float TotalTime { get; private set; }

        private static Stopwatch watch = new Stopwatch();

        public static void Initialize()
        {
            StartUpTime = DateTime.Now;
        }

        public static void MainTimeLinePropel()
        {
            watch.Reset();
            watch.Restart();
        }

        public static void MainTimeLinePause()
        {
            watch.Stop();
            DeltaTime = (float)watch.Elapsed.TotalSeconds;
            TotalTime += DeltaTime;
            FrameCount++;
            //gameTimeMS += watch.ElapsedMilliseconds;
            //watch.Restart
        }
    }
}

