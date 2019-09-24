#version 330

in vec4 color;
uniform sampler2D maintexture;
out vec4 outputColor;
in vec2 f_texcoord;
uniform int isTextured;

void main()
{
	if(isTextured == 1)
    {
		outputColor = color;//texture(maintexture, f_texcoord);
	}
	else
	{
		outputColor = color;
	}
}