#version 300

in  vec3 vPosition;

in	vec3 vNormal;
out vec3 v_norm;

in vec2 texcoord;
out vec2 f_texcoord;

out vec3 f_pos;


uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

void
main()
{
    gl_Position =  projection * view * model * vec4(vPosition, 1.0);
    f_pos = vec3(model * vec4(vPosition, 1.0));
	f_texcoord = texcoord;
	v_norm = mat3(transpose(inverse(model))) * vNormal;
}