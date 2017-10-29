#version 330

in vec3 v_norm;
in vec3 f_pos;

in vec2 f_texcoord;
out vec4 outputColor;

uniform vec3 cameraPosition;

uniform float specularIntensivity;

uniform vec3 lightPos;

uniform float ambientStr;
uniform vec3 lightColor;
uniform sampler2D maintexture;



void
main()
{

	vec3 n = normalize(v_norm);

	vec2 flipped_texcoord = vec2(f_texcoord.x, 1.0 - f_texcoord.y);

	vec3 lightDir = normalize(lightPos - f_pos);

	float diff = max(dot(lightDir,n), 0.0);

	vec3 viewDir = normalize(cameraPosition - f_pos);

	vec3 reflectDir = reflect(-lightDir, n); 

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 128);

	vec3 specular = specularIntensivity * spec * lightColor;  

	vec3 diffuse = (diff * (lightColor + ambientStr + specular));
   
    outputColor = vec4(vec4(n,0)*vec4(diffuse,1.0));


	//outputColor = vec4( 0.5 + 0.5 * n, 1.0);
}