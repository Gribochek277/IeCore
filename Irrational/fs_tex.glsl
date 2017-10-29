#version 130

in vec2 f_texcoord;
out vec4 outputColor;

uniform float ambientStr;
uniform vec3 lightColor;
uniform sampler2D maintexture;

void
main()
{
	vec2 flipped_texcoord = vec2(f_texcoord.x, 1.0 - f_texcoord.y);
    outputColor = vec4(texture(maintexture, flipped_texcoord)*vec4(lightColor*ambientStr,1));
}