#version 330

in vec3 v_norm;
in vec3 f_pos;

in vec2 f_texcoord;
out vec4 outputColor;

in vec3 calcLightPos;

uniform float ambientStr;
uniform vec3 lightColor;
uniform sampler2D maintexture;



void
main()
{

	vec3 n = normalize(v_norm);

	vec2 flipped_texcoord = vec2(f_texcoord.x, 1.0 - f_texcoord.y);

	vec3 lightDir = normalize(calcLightPos - f_pos);

	float diff = max(dot(lightDir,n), 0.0);

	vec3 diffuse = (diff * (lightColor + ambientStr));

    outputColor = vec4(texture(maintexture, flipped_texcoord)*vec4(diffuse,1.0));

	

	//outputColor = vec4( 0.5 + 0.5 * n, 1.0);
}