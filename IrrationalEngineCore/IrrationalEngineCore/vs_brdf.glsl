#version 330 core
layout (location = 0) in vec3 f_pos;
layout (location = 1) in vec2 f_texcoord;

out vec2 TexCoords;

void main()
{
    TexCoords = f_texcoord;
	gl_Position = vec4(f_pos, 1.0);
}