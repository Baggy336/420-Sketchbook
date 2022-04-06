#define PROCESSING_TEXTURE_SHADER

// values from Processing:
uniform sampler2D texture; // texture to use on the surface
uniform vec2 texOffset; // size of a texel

// values from vertex shader
varying vec4 vertTexCoord; // uv value at a pixel
varying vec4 vertColor; // vertex color at a pixel

uniform float time; // time in seconds

// runs once per pixel:
void main(){

    float ratio = texOffset.x / texOffset.y;

    vec2 uv = vertTexCoord.xy - vec2(.5, .5);
    
    float mag = length(uv); // dis from center
    float angleRad = atan(uv.y, uv.x); // angle from center

    //mag -= .01 * sin(time);
    angleRad += .05 * sin(time);

    uv.x = mag * cos(angleRad);
    uv.y = mag * sin(angleRad);

    uv += vec2(.5, .5); // move origin back to 0, 0

    // lookup pixel color at uv coord vec2 using .xy
    vec4 color = texture2D(texture, uv);

    // set the pixel color of gl_FragColor
    gl_FragColor = color;

}
