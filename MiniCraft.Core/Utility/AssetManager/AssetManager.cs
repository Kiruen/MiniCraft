using CSharpGL;
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
        public static readonly string GameRootPath = @"..\..\..\"; //Application.StartupPath

        /// <summary>
        /// 获取texture文件夹下的纹理文件路径，需加后缀
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string GetTexturePath(string relativePath="")
        {
            return Path.Combine(GameRootPath, "Asset", "Texture", relativePath);
        }

        /// <summary>
        /// 获取音乐文件路径，需加后缀
        /// </summary>
        /// <returns></returns>
        public static string GetMusicPath(string relativePath = "")
        {
            return Path.Combine(GameRootPath, "Asset", @"Sound\Music", relativePath);
        }

        /// <summary>
        /// 获取音频文件路径，需加后缀
        /// </summary>
        /// <returns></returns>
        public static string GetAudioPath(string relativePath = "")
        {
            return Path.Combine(GameRootPath, "Asset", @"Sound\Audio", relativePath);
        }

        /// <summary>
        /// 获取NBT文件路径，无需加后缀
        /// </summary>
        /// <returns></returns>
        public static string GetNBTFilePath(string fileName = "", string folder="Map")
        {
            return Path.Combine(GameRootPath, "Asset", $@"{folder}", fileName == "" ? "" : $"{fileName}.nbt");
        }

        /// <summary>
        /// 获取JSON文件路径，无需加后缀
        /// </summary>
        /// <returns></returns>
        public static string GetJSONFilePath(string relativeFileName)
        {
            return Path.Combine(GameRootPath, "Asset", $@"{relativeFileName}.json");
        }

        /// <summary>
        /// 获取Glyph文件夹下的字形文件路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        private static string GetGlyphSheetPath(string relativePath = "")
        {
            return Path.Combine(GameRootPath, "Asset", "Glyph", relativePath);
        }
    }

    public static partial class AssetManager
    {
        static Dictionary<string, Texture> textures = new Dictionary<string, Texture>(16);
        static Dictionary<string, Image> images = new Dictionary<string, Image>(16);

        public static Texture GetTexture(string name, string subType="Entity")
        {
            Texture tex = null;
            var texName = Utility.ToTitle($"{subType}.{name}");
            if (!textures.TryGetValue(texName, out tex))
            {
                tex = GenTextureFromFile($@"{subType}\{name}.png");
                textures.Add(texName, tex);
            }
            return tex;
        }

        public static Image GetImage(string name, string subType = "Entity")
        {
            Image image = null;
            var imgName = Utility.ToTitle($"{subType}.{name}");
            if (!images.TryGetValue(imgName, out image))
            {
                image = GenImageFromFile($@"{subType}\{name}.png");
                images.Add(imgName, image);
            }
            return image;
        }

        public static Image GenImageFromFile(string relativePath)
        {
            return new Bitmap(GetTexturePath(relativePath));
        }

        /// <summary>
        /// 获取texture文件夹下的纹理文件
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static Texture GenTextureFromFile(string relativePath, int mipmapLevel=4, bool autoInit=true)
        {
            //string folder = System.Windows.Forms.Application.StartupPath;
            //var bmp = new Bitmap(System.IO.Path.Combine(folder, PathManager.GetTexture(@"cloth.png")));
            var bmp = new Bitmap(GenImageFromFile(relativePath));
            //TODO:研究一下mipmap等级为啥不能随便乱改？？改成4就出错了？因为你参数填错位置了！！！
            //TexImageBitmaps
            TexStorageBase storage = new TexImageBitmap(bmp, mipmapLevelCount:mipmapLevel);   //, GL.GL_RGBA, 1, true
            var texture = new Texture(storage,
                //new TexParameterfv(TexParameter.PropertyName.TextureBorderColor, 1, 0, 0),
                new TexParameteri(TexParameter.PropertyName.TextureWrapS, (int)GL.GL_CLAMP_TO_BORDER),
                new TexParameteri(TexParameter.PropertyName.TextureWrapT, (int)GL.GL_CLAMP_TO_BORDER),
                new TexParameteri(TexParameter.PropertyName.TextureWrapR, (int)GL.GL_CLAMP_TO_BORDER),
                new TexParameteri(TexParameter.PropertyName.TextureMinFilter, (int)GL.GL_LINEAR),
                new TexParameteri(TexParameter.PropertyName.TextureMagFilter, (int)GL.GL_LINEAR));
            if (autoInit)
            {
                texture.Initialize();
                //bmp.Dispose(); 不要自己销毁了！
            }
            return texture;
        }
    }
}
