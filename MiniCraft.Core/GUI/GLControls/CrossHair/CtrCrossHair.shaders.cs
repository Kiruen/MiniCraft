using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniCraft.Core.GUI
{
    /// <summary>
    /// A rectangle control that displays an image.
    /// </summary>
    public partial class MCrossHair
    {
        private const string inPosition = "inPosition";
        private const string inUV = "inUV";

        private const string vert =
            @"#version 330 core

        in vec3 " + inPosition + @";

        void main(void) {
	        gl_Position = vec4(inPosition, 1.0);
        }
        ";

        private const string frag =
            @"#version 330 core

layout(location = 0) out vec4 outColor;

void main(void) {
    outColor = vec4(1, 0, 0, 1);
}
";
    }
}
