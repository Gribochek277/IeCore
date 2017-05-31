using System;
namespace IrrationalSpace
{
	public static class VertexShaders
	{
public static string VertexShaderDefault = @"
                #version 130
                in vec3 vertexPosition;
				in vec3 vertexNormal;
				in vec3 vertexTangent;
				in vec2 vertexUV;
				uniform vec3 light_direction;
				out vec3 normal;
				out vec2 uv;
				out vec3 light;
				uniform mat4 projection_matrix;
				uniform mat4 view_matrix;
				uniform mat4 model_matrix;
				uniform bool enable_mapping;

				void main(void)
				{
    				normal = normalize((model_matrix * vec4(floor(vertexNormal), 0)).xyz);
    				uv = vertexUV;
    				mat3 tbnMatrix = mat3(vertexTangent, cross(vertexTangent, normal), normal);
    				light = (enable_mapping ? light_direction * tbnMatrix : light_direction);
    				gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
				}";
        public static string VertexShaderOpenTKTest = @"
        #version 130
 
                in vec3 vPosition;
                in  vec3 vColor;
                out vec4 color;
                uniform mat4 modelview;
 
            void main()
            {
                gl_Position = modelview * vec4(vPosition, 1.0);
 
                color = vec4( vColor, 1.0);
            }";
	}
}
