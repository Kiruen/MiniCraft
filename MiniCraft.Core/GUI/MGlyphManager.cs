using CSharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    /// <summary>
    /// A rectangle control that displays an image.
    /// </summary>
    public class MGlyphManager : GlyphServer
    {
        public static GlyphServer CachedInstance
        {
            get
            {
                IntPtr context = GL.Instance.GetCurrentContext();
                if (context == null) { throw new Exception("Render context not exists!"); }

                GlyphServer server;
                var dict = GlyphServer.defaultServerDict;
                if (!dict.TryGetValue(context, out server))
                {
                    lock (synObj) // the process of creating a glyph serve may take a long time(several seconds), so we need a lock.
                    {
                        if (!dict.TryGetValue(context, out server))
                        {
                            var builder = new StringBuilder();
                            // ascii
                            for (char c = ' '; c <= '~'; c++)
                            {
                                builder.Append(c);
                            }
                            // Chinese characters
                            for (char c = (char)0x4E00; c <= 0x9FA5; c++)
                            {
                                builder.Append(c);
                            }
                            var font = new Font("Arial", 64, GraphicsUnit.Pixel);
                            string charSet = builder.ToString();

                            if (charSet == null || charSet.Count() == 0) { return new MGlyphManager(); }
                            List<GlyphChunkBase> chunkList = GetChunkList(font, charSet);
                            server = MGlyphManager.CreateCached(chunkList, 1024, 1024, 1000);

                            font.Dispose();
                            dict.Add(context, server);
                        }
                    }
                }

                return server;
            }
        }


        protected static MGlyphManager CreateCached(List<GlyphChunkBase> chunkList, int maxTextureWidth, int maxTextureHeight, int maxTextureCount)
        {
            var context = new PagesContext(maxTextureWidth, maxTextureHeight, maxTextureCount);
            foreach (var item in chunkList)
            {
                item.Put(context);
            }

            var bitmaps = AssetManager.GetGlyphSheets();

            Dictionary<string, GlyphInfo> dictionary = GetDictionary(chunkList, bitmaps[0].Width, bitmaps[0].Height);

            Texture texture = GenerateTexture(bitmaps);

            var server = new MGlyphManager
            {
                dictionary = dictionary,
                GlyphTexture = texture,
                TextureWidth = bitmaps[0].Width,
                TextureHeight = bitmaps[0].Height
            };

            //// test: save bitmaps to disk.
            //Test(server.dictionary, bitmaps);

            foreach (var item in bitmaps)
            {
                item.Dispose();
            }

            return server;
        }
    }


}

