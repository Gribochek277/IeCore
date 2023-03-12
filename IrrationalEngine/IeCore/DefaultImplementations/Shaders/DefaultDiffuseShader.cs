namespace IeCore.DefaultImplementations.Shaders
{
	public static class DefaultDiffuseShader
	{
		public const string VertexShader = @"#version 330
                                               layout(location = 0) in vec3 aPosition;
                                               layout(location = 1) in vec2 aTexCoord;
                                               uniform mat4 model;
                                               uniform mat4 view;
                                               uniform mat4 projection;
                                               out vec2 texCoord;
                                               void main()
                                               {
                                                   texCoord = aTexCoord;
                                                   gl_Position = projection * view * model * vec4(aPosition, 1.0);
                                               }";

		public const string FragmentShader = @"#version 330
                                                  out vec4 FragColor;
                                                  uniform vec4 Color;
                                                  uniform int isTextured;
                                                  in vec2 texCoord;
                                                  uniform sampler2D texture0;

                                                  void main()
                                                  {
                                                      FragColor = texture(texture0, texCoord);
                                                  }";
	}
}
