using CSharpGL;
using MiniCraft.Core.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    /// <summary>
    /// A rectangle control that displays an image.
    /// </summary>
    public partial class MCrossHair : GLControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap">bitmap to be displayed.</param>
        /// <param name="autoDispose">auto dispose <paramref name="bitmap"/> after this object's initialization.</param>
        public MCrossHair(bool autoDispose = false)
            : base(GUIAnchorStyles.Left | GUIAnchorStyles.Top)
        {
            var model = new MCrossHairMesh();
            var vs = new VertexShader(vert);
            var fs = new FragmentShader(frag);
            var codes = new ShaderArray(vs, fs);
            var map = new AttributeMap();
            map.Add(inPosition, MeshBase.strPosition);
            var methodBuilder = new RenderMethodBuilder(codes, map, new BlendFuncSwitch(BlendSrcFactor.SrcAlpha, BlendDestFactor.OneMinusSrcAlpha));
            this.RenderUnit = new ModernRenderUnit(model, methodBuilder);

            this.autoDispose = autoDispose;

            this.Initialize();
        }

        private bool autoDispose;

        /// <summary>
        /// 
        /// </summary>
        public ModernRenderUnit RenderUnit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected override void DoInitialize()
        {
            this.RenderUnit.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        public override void RenderGUIBeforeChildren(GUIRenderEventArgs arg)
        {
            base.RenderGUIBeforeChildren(arg);

            ModernRenderUnit unit = this.RenderUnit;
            RenderMethod method = unit.Methods[0];
            GLState.lineWidthSwitch.On();
            method.Render();
            GLState.lineWidthSwitch.Off();
        }
    }
}
