#version 330

in vec3 v_norm;
in vec3 f_pos;

in vec2 f_texcoord;
out vec4 outputColor;

uniform vec3 cameraPosition;

uniform float specStr;

uniform vec3 lightPos[1];

uniform float ambientStr;
uniform vec3 lightColor[1];
uniform sampler2D maintexture;
uniform sampler2D normaltexture;


vec3 getNormalFromMap(vec2 texcoord)
{
    vec3 tangentNormal = texture(normaltexture, texcoord).xyz * 2.0 - 1.0;

    vec3 Q1  = dFdx(f_pos);
    vec3 Q2  = dFdy(f_pos);
    vec2 st1 = dFdx(texcoord);
    vec2 st2 = dFdy(texcoord);

    vec3 N   = normalize(v_norm);
    vec3 T  = normalize(Q1*st2.t - Q2*st1.t);
    vec3 B  = -normalize(cross(N, T));
    mat3 TBN = mat3(T, B, N);

    return normalize(TBN * tangentNormal);
}

//TODO: split one shader into two 
//one for normal map and one for normal from model or calculated
void
main()
{
	vec3 n = getNormalFromMap(f_texcoord);

	vec3 lightDir = normalize(lightPos[0] - f_pos);

	float diff = max(dot(lightDir,texture(normaltexture, f_texcoord).rgb), 0.0);

	vec3 viewDir = normalize(cameraPosition - f_pos);

	vec3 reflectDir = reflect(-lightDir, texture(normaltexture, f_texcoord).rgb*2-1); 

	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 128);

	vec3 specular = specStr * spec * lightColor[0];  

	vec3 diffuse = (diff * (lightColor[0] + ambientStr + specular));
   
    outputColor = vec4(texture(maintexture, f_texcoord)*vec4(diffuse,1.0));


	//outputColor = vec4( 0.5 + 0.5 * n, 1.0);
}