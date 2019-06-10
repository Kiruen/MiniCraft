using CSharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCraft.Core.Game
{
    partial class GameObject
    {
        #region 着色器代码
        internal protected const string u_inPosition = "inPosition";
        internal protected const string u_projectionMat = "projectionMat";
        internal protected const string u_viewMat = "viewMat";
        internal protected const string u_modelMat = "modelMat";
        internal protected const string u_modelMats = "modelMats";
        internal protected const string u_mvpMat = "mvpMat";
        internal protected const string u_color = "color";

        internal protected const string u_texture0 = "texture0";
        internal protected const string u_inTexCoord = "inTexCoord";
        internal protected const string u_passTexCoord = "passTexCoord";

        internal protected const string u_alpha = "alpha";
        internal protected const string u_main_color = "main_color";
        internal protected const string u_filter_on = "filter_on";

        internal protected const string u_uv_offset = "uv_offset";
        

        protected static readonly string vertexCode =
$@"#version 330 core

in vec3 { u_inPosition };

uniform mat4 { u_projectionMat };
uniform mat4 { u_viewMat };
uniform mat4 { u_modelMat };

void main(void) {{
	gl_Position = projectionMat * viewMat * modelMat * vec4(inPosition, 1.0);
}}
";
        protected static readonly string fragmentCode =
            $@"#version 330 core

uniform vec4 { u_color };

layout(location = 0) out vec4 outColor;

void main(void) {{
    outColor = { u_color };
}}
";

        protected static readonly string vertexCodeWithTex =
$@"#version 410 core

in vec3 { u_inPosition };
in vec2 { u_inTexCoord };

uniform mat4 { u_projectionMat };
uniform mat4 { u_viewMat };
uniform mat4 { u_modelMat };

out vec2 { u_passTexCoord };

void main(void) {{
	gl_Position = projectionMat * viewMat * modelMat * vec4({ u_inPosition }, 1.0);
    { u_passTexCoord } = inTexCoord;
}}
";

        protected static readonly string fragmentCodeWithTex =
            $@"#version 410 core

uniform sampler2D { u_texture0 };
uniform bool wire_frame_mode = false;
uniform float { u_alpha } = 1;

vec4 frame_color = vec4(0, 0, 0, 1);
in vec2 { u_passTexCoord };

out vec4 outColor;

void main(void) {{
    outColor = texture({ u_texture0 }, { u_passTexCoord });
}}
";
        /*
             if (!wire_frame_mode) {{
        outColor = texture({ u_texture0 }, { u_passTexCoord });
    }} else {{
        outColor = frame_color;
    }}
             */
        protected static readonly string fragmentCodeWithTexColored =
            $@"#version 410 core

uniform sampler2D { u_texture0 };
uniform vec4 { u_main_color } = vec4(255, 255, 255, 1);
uniform bool { u_filter_on };

in vec2 { u_passTexCoord };

out vec4 outColor;

void main(void) {{
    vec4 color = texture({ u_texture0 }, { u_passTexCoord });
    if({ u_filter_on } && color.x == color.y && color.y == color.z)
    {{
        color = color * { u_main_color }; //(float(color.x) / 128.0)
    }}
    outColor = color;
}}
";

        
        protected static readonly string vertexCodeWithInstancingTex =
$@"#version 330 core

in vec3 { u_inPosition };
in vec2 { u_inTexCoord };

uniform mat4 { u_projectionMat };
uniform mat4 { u_viewMat };
uniform mat4 {u_modelMats}[192];


out vec2 { u_passTexCoord };

//uniform vec3 camera_pos;
//out float fog_factor;

void main(void) {{
	gl_Position = { u_projectionMat } * { u_viewMat } * {u_modelMats}[gl_InstanceID] * vec4({ u_inPosition }, 1.0);
    { u_passTexCoord } = { u_inTexCoord };
    
/*
    float fog_distance = 50;
    float camera_dist = distance(camera_pos, { u_inPosition });
    fog_factor = pow(clamp(camera_dist / fog_depth, 0.0, 1.0), 4.0);
    float dy = { u_inPosition }.y - camera_pos.y;
    float dxz = distance({ u_inPosition }.xz, camera_pos.xz);
    fog_depth = (atan(dy, dxz) + pi / 2) / pi;*/
}}";

/* 失败的优化尝试，原因未知
        protected static readonly string vertexCodeWithInstancingTex =
$@"#version 330 core

layout(location = 0) in vec3 { u_inPosition };
layout(location = 1) in vec2 { u_inTexCoord };
layout(location = 2) in mat4 modelMat;

uniform mat4 { u_projectionMat };
uniform mat4 { u_viewMat };


out vec2 { u_passTexCoord };

void main(void) {{
	gl_Position = { u_projectionMat } * { u_viewMat } * modelMat * vec4({ u_inPosition }, 1.0);
    { u_passTexCoord } = { u_inTexCoord };
}}";*/


        protected static readonly string fragmentCodeWithTexDynamic =
$@"#version 420 core

in vec2 { u_passTexCoord };
uniform sampler2D { u_texture0 };
uniform vec2 { u_uv_offset };

out vec4 outColor;

void main(void) {{
    outColor = texture({ u_texture0 }, { u_passTexCoord } - { u_uv_offset });
}}
";
        //outColor = vec4(texture({ texture0 }, { passTexCoord }).xyz, {alpha});
        #endregion
    }
}
