namespace IeCore.DefaultImplementations.Shaders
{
	public static class DefaultDiffuseShaderAnimated
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
		public const string VertexShader = @"#version 420
                                               layout(location = 0) in vec3 aPosition;
                                               layout(location = 1) in vec2 aTexCoord;
                                               layout(location = 2) in vec4 boneIds;
                                               layout(location = 3) in vec4 Weights;
                        
                                               const int MAX_BONES = 100;
											   const int MAX_BONE_INFLUENCE = 4;

                                               uniform mat4 model;
                                               uniform mat4 view;
                                               uniform mat4 projection;

                                               uniform mat4 finalBonesMatrices[MAX_BONES];

                                               out vec2 texCoord;

                                               void main()
                                               {
                                                   vec4 newPosition = vec4(0.0f);

 												   for(int i = 0 ; i < MAX_BONE_INFLUENCE ; i++)
 												   {
														
 												        
													     if(boneIds[i] == -1.0f) 
 												             continue;

 												         if(int(boneIds[i]) >= MAX_BONES) 
 												         {
 												             newPosition = vec4(aPosition,1.0f);
 												             break;
 												         }
 												         vec4 localPosition = finalBonesMatrices[int(boneIds[i])] * vec4(aPosition,1.0f);
 												         newPosition += localPosition * Weights[i];
 												         vec3 localNormal = mat3(finalBonesMatrices[int(boneIds[i])]) * vec3(1.0f,1.0f,1.0f);
														 
 												   }

												    gl_Position =  projection * view * model * newPosition;
													texCoord = aTexCoord;
										}";



		public const string FragmentShader = @"#version 420
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
