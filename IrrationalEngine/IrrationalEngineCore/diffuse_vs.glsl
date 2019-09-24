#version 330

in  vec3 vPosition;
out vec4 color;
uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;
layout (location = 1) in vec2 texcoord;
out vec2 f_texcoord;

void
main()
{
    gl_Position = projection * view * model * vec4(vPosition, 1.0);
	f_texcoord = texcoord;
    color = vec4(vPosition,1.0);
}