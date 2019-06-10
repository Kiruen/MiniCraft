using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core
{
    public class MRenderEventArgs : RenderEventArgs
    {
        /// <summary>
        /// 指示本次渲染任务是否实际执行
        /// </summary>
        public bool EnableRendering { get; set; }

        /// <summary>
        /// 指示本次渲染任务所使用的方法名称
        /// </summary>
        public string MethodName { get; set; }

        public object Obj { get; set; }

        public MRenderEventArgs(ActionParams param, ICamera camera, bool enableRendering = true) 
            : base(param, camera)
        {
        }

        public MRenderEventArgs(RenderEventArgs args, bool enableRendering = true) 
            : base(args.Param, args.Camera)
        {
        }
    }
}
