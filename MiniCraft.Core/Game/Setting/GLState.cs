using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharpGL.GL;

namespace MiniCraft.Core
{
    public static class GLState
    {
        public const int GL_FOG_COORDINATE_SOURCE = 0x8450;
        public const int GL_FOG_COORDINATE = 0x8451;
        public const int GL_FRAGMENT_DEPTH = 0x8452;

        public static GL gl;
        public static DepthMaskSwitch DepthMask { get; set; } = new DepthMaskSwitch(false);
        public static CullFaceSwitch CullFace { get; set; } = new CullFaceSwitch(CullFaceMode.Back);
        public static BlendFuncSwitch Blending { get; set; } = new BlendFuncSwitch();
        public static PolygonModeSwitch polygonModeSwitch { get; set; } = new PolygonModeSwitch(PolygonMode.Line);
        public static LineWidthSwitch lineWidthSwitch { get; set; } = new LineWidthSwitch(5);
        public static LogicOpSwitch logicOp = new LogicOpSwitch(LogicOperationCode.NoOp);
        /// <summary>
        /// Enable logic operation or not?
        /// </summary>
        //[Browsable(false)]
        //public bool LogicOp { get { return this.logicOp.Enabled; } set { this.logicOp.Enabled = value; } }


        public static void Initialize()
        {
            gl = Instance;
            //    this.logicOp.Enabled = false;
            //    var method = this.RenderUnit.Methods[0]; // the only render unit in this node.
            //    method.SwitchList.Add(this.logicOp);
            FogSwitchOn();
        }

        public static void FogSwitchOn()
        {
            gl.Enable(GL_FOG);
            gl.Fogfv(GL_FOG_COLOR, new vec4(0.5f, 0.69f, 1, 1).ToArray());
            gl.Hint(GL_FOG_HINT, GL_DONT_CARE);
            gl.Fogi(GL_FOG_MODE, (int)GL_LINEAR);
            gl.Fogf(GL_FOG_START, 40);
            gl.Fogf(GL_FOG_END, 100);
            gl.Fogi(GL_FOG_COORDINATE_SOURCE, GL_FOG_COORDINATE);
            
            //gl.Fogi(GL_FOG_COORDINATE_SOURCE, GL_FRAGMENT_DEPTH);
        }

        static BlendFuncSwitch blending;

    }
}
