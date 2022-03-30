#define PROCESSING_TEXTURE_SHADER

// values from Processing:
uniform sampler2D texture; // texture to use on the surface
uniform vec2 texOffset; // size of a texel

// values from vertex shader
varying vec4 vertTexCoord; // uv value at a pixel
varying vec4 vertColor; // vertex color at a pixel

// runs once per pixel:
void main(){

    float ratio = texOffset.x / texOffset.y;

    vec2 uv = vertTexCoord.xy;
    
    float mag = length(uv); // dis from 0, 0
    float angleRad = atan(uv.y, uv.x); // angle from 0, 0

    mag += .01;

    uv.x = mag * cos(angleRad);
    uv.y = mag * sin(angleRad);

    // lookup pixel color at uv coord vec2 using .xy
    vec4 color = texture2D(texture, uv);

    // set the pixel color of gl_FragColor
    gl_FragColor = color;

}
