#define PROCESSING_TEXTURE_SHADER

// values from Processing:
uniform sampler2D texture; // texture to use on the surface
uniform vec2 texOffset; // size of a texel

// values from vertex shader
varying vec4 vertTexCoord; // uv value at a pixel
varying vec4 vertColor; // vertex color at a pixel

uniform float time; // time in seconds
uniform vec2 mouse;

// runs once per pixel:
void main(){

    float ratio = texOffset.x / texOffset.y;

    vec2 uv = vertTexCoord.xy - mouse;
    float dis = length(uv);
    
    // lookup pixel color at uv coord vec2 using .xy
    vec4 color = vec4(dis, dis, dis, 1);

    // set the pixel color of gl_FragColor
    gl_FragColor = color;

}
