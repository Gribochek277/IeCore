namespace IeCore.DefaultImplementations.Shaders
{
    public static class DefaultDiffuseShader
    {
        public const string VertexShader = @"#version 330
                                               layout(location = 0) in vec3 aPosition;

                                               void main()
                                               {
                                                   gl_Position = vec4(aPosition, 1.0);
                                               }";

        public const string FragmentShader = @"#version 330
                                                  out vec4 FragColor;
                                                  uniform vec4 Color;
                                                  uniform int isTextured;

                                                  void main()
                                                  {
                                                      FragColor = vec4(Color);
                                                  }";
    }
}
