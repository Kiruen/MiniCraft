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
        static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>(16);

        public static Shader GetShader(ShaderType type, string usage, string code="")
        {
            string name = ParseShaderName(type, usage);
            if (!shaders.TryGetValue(name, out Shader shader))
            {
                shader = GenShaderFromSrc(code, type);
                shaders.Add(name, shader);
            }
            return shader;
        }

        public static string ParseShaderName(ShaderType type, string usage)
        {
            string prefix = "com";
            switch (type)
            {
                case ShaderType.VertexShader:
                    prefix = "vs"; break;
                case ShaderType.FragmentShader:
                    prefix = "gs"; break;
            }
            return $"{prefix}_{usage}";
        }

        /// <summary>
        /// 获取texture文件夹下的纹理文件
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static Shader GenShaderFromSrc(string srcCode, ShaderType type, bool autoInit = false)
        {
            switch(type)
            {
                case ShaderType.VertexShader:
                    return new VertexShader(srcCode);
                case ShaderType.FragmentShader:
                    return new FragmentShader(srcCode);
                default:
                    return new VertexShader(srcCode);
            }
        }
    }
}
