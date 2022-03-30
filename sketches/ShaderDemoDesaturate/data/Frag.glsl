#define PROCESSING_TEXTURE_SHADER

// values from Processing:
uniform sampler2D texture; // texture to use on the surface
uniform vec2 texOffset; // size of a texel

// values from vertex shader
varying vec4 vertTexCoord; // uv value at a pixel
varying vec4 vertColor; // vertex color at a pixel

// runs once per pixel:
void main(){

    // lookup pixel color at uv coord vec2 using .xy
    vec4 color = texture2D(texture, vertTexCoord.xy);

    // desaturate (each channel should have the same value)
    float v = (color.r + color.g + color.b) / 3;
    color.rgb = vec3(v);

    // set the pixel color of gl_FragColor
    gl_FragColor = color;

}
