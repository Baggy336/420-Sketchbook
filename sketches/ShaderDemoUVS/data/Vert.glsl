#define PROCESSING_TEXTURE_SHADER

// values from Processing:
uniform mat4 transform;
uniform mat4 texMatrix;

attribute vec4 vertex; // pos in local-space
attribute vec4 color; // color
attribute vec2 texCoord; // uv

varying vec4 vertColor;
varying vec4 vertTexCoord;

// Run once per vert
void main(){
    // gl_Position to the final vertex screen space pos
    gl_Position = transform * vertex;

    // set the color of this pixel to the color
    vertColor = color;

    vertTexCoord = texMatrix * vec4(texCoord, 1, 1); // make texCoord as wide as the matrix

}
