#version 130

in vec2 f_texcoord;
out vec4 outputColor;

uniform float ambientStr;
uniform vec4 lightColor;
uniform sampler2D maintexture;

void
main()
{
	vec2 flipped_texcoord = vec2(f_texcoord.x, 1.0 - f_texcoord.y);
    outputColor = vec4(texture(maintexture, flipped_texcoord)*(lightColor*ambientStr));
}