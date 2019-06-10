using CSharpGL;
using IrrKlang;
using MiniCraft.Core.Game;
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
    public static partial class SoundEngine
    {
        public static ISoundEngine audioEngine;
        public static ISound playingMusic;

        public static void Initialize()
        {
            audioEngine = new ISoundEngine();
        }

        public static void PlayAudio(vec3 position)
        {
            audioEngine.Play3D(AssetManager.GetAudioPath("explosion.wav"), position.x, position.y, position.z);
        }

        public static void PlayMusic()
        {
            playingMusic = audioEngine.Play3D(AssetManager.GetMusicPath("explosion.wav"), 0, 0, 0);
        }
    }
}
