using System;
namespace IrrationalSpace
{
	public static class FragmentShaders
	{
		public static string FragmentShaderDefault = 
			@"#version 130
			uniform sampler2D colorTexture;
			uniform sampler2D normalTexture;
			uniform bool enable_lighting;
			uniform mat4 model_matrix;
			uniform bool enable_mapping;
			uniform float light_strenght;
			in vec3 normal;
			in vec2 uv;
			in vec3 light;
			uniform float alpha_str;
			uniform vec3 color;
			out vec4 fragment;

			void main(void)
			{
    			vec3 fragmentNormal = texture2D(normalTexture, uv).xyz * 2 - 1;
    			vec3 selectedNormal = (enable_mapping ? fragmentNormal : normal);
    			float diffuse = max(dot(selectedNormal, light)*light_strenght, 0);
    			float ambient = 0.3;
    			float lighting = (enable_lighting ? max(diffuse, ambient) : 1);
				vec4 sample = lighting * texture2D(colorTexture, uv);
   				fragment = vec4(sample.xyz*color, sample.a*alpha_str);
			}";
		public static string FragmentShaderOpenTKTest = @"
        #version 130
 
        in vec4 color;
        out vec4 outputColor;
 
        void main()
        {
          outputColor = color;
        }";
	}
}
