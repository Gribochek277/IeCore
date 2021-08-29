namespace IeCore.DefaultImplementations.Shaders
{
	public class DefaultDiffuseShaderAnimated
	{


		//public const string VertexShader = @"#version 330
		//                                       layout(location = 0) in vec3 aPosition;
		//                                       layout(location = 1) in vec2 aTexCoord;
		//                                       uniform mat4 model;
		//                                       uniform mat4 view;
		//                                       uniform mat4 projection;
		//                                       out vec2 texCoord;
		//                                       void main()
		//                                       {
		//                                           texCoord = aTexCoord;
		//                                           gl_Position = projection * view * model * vec4(aPosition, 1.0);
		//                                       }";


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




		public const string VertexShader = @"#version 330
                                               layout(location = 0) in vec3 aPosition;
                                               layout(location = 1) in vec2 aTexCoord;
                                               layout(location = 2) in ivec4 BoneIDs;
                                               layout(location = 3) in vec4 Weights;
                        
                                               const int MAX_BONES = 100;

                                               uniform mat4 model;
                                               uniform mat4 view;
                                               uniform mat4 projection;

                                               uniform mat4 Bones[MAX_BONES];

                                               out vec2 texCoord;

                                               void main()
                                               {
                                                   vec4 newPosition = vec4(0.0);
                                                   int index = 0;
                                                   
                                                    for(int i=0; i<4; i++)
                                                    {		
                                                      index = int(BoneIDs[i]);	
                                                      newPosition += (Bones[index] * vec4(aPosition, 1.0)) * Weights[i];		
                                                    }
         
                                                   texCoord = aTexCoord;
                                                   gl_Position = projection * view * model  * newPosition;//vec4(aPosition, 1.0);
                                               }";
	}
}
