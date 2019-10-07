#version 130

in  vec3 vPosition;
out vec4 color;
uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

void
main()
{
    gl_Position = projection * view * model * vec4(vPosition, 1.0);
    color = vec4( vPosition,1.0);
}