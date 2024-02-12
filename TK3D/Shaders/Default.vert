#version 330 core
layout (location = 0) in vec3 aPosition; //ver coord
layout (location = 1) in vec2 aTexCoord; //tex coord

out vec2 texCoord;

//uniform variables
uniform mat4 model;

void main()
{
	gl_Position = vec4(aPosition, 1.0); //coords
	texCoord = aTexCoord;
}