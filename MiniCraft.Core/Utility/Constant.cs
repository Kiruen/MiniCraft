using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    public enum Direction3D : byte
    {
        Unknown = 0,
        Front = 1,
        Right = 2,
        Up = 3,
        Back = 4,
        Left = 5,
        Down = 6,
    }

    public static class ShaderUsage
    {
        public const string SIMPLE = "simple";
        public const string SKY_BOX = "sky_box";
        public const string SKY_BOX_WITH_TIME = "sky_box_with_time";
        public const string STD_BLOCK_TEX = "standard_block_tex";
        public const string STD_BLOCK_TEX_INSTANCING = "standard_block_tex_instancing";
        public const string STD_BLOCK_TEX_COLOR = "standard_block_tex_color";
        public const string STD_BLOCK_TEX_DYNAMIC = "standard_block_tex_dynamic";
    }

    public static class RenderStrategy
    {
        public const string SIMPLE = "simple";
        public const string INSTANCING = "instacing";
        public const string DEFAULT = "sky_box";
        public const string STD_TEX = "tex";
        public const string STD_TEX_INSTANCING = "tex_instancing";
        public const string STD_TEX_COLOR = "tex_color";
        public const string STD_TEX_COLOR_INSTANCING = "tex_color_instancing";
        public const string STD_TEX_DYNAMIC = "tex_dynamic";
        public const string STD_TEX_DYNAMIC_INSTANCING = "tex_dynamic_instancing";
    }

    public static class Constant
    {
        public const int RENDER_CHUNK_X_RADIUS = 2,
                        RENDER_CHUNK_Y_RADIUS = 2,
                        RENDER_CHUNK_Z_RADIUS = 2;

        public const int CHUNK_X_HALF_LEN = 8,
                        CHUNK_Y_HALF_LEN = 8,
                        CHUNK_Z_HALF_LEN = 8;

        //public const int CHUNK_X_LEN_MAX = CHUNK_X_HALF_LEN_MAX * 2, 
        //                CHUNK_Y_LEN_MAX = CHUNK_Y_HALF_LEN_MAX * 2, 
        //                CHUNK_Z_LEN_MAX = CHUNK_Z_HALF_LEN_MAX * 2;
        public const int CHUNK_X_LEN = CHUNK_X_HALF_LEN * 2,
                        CHUNK_Y_LEN = CHUNK_Y_HALF_LEN * 2,
                        CHUNK_Z_LEN = CHUNK_Z_HALF_LEN * 2;
    }
}
