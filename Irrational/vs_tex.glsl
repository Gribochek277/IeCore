#version 330

in  vec3 vPosition;
in vec2 texcoord;
out vec2 f_texcoord;

uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

void
main()
{
     gl_Position =  projection * view * model * vec4(vPosition, 1.0);
  
    f_texcoord = texcoord;
}