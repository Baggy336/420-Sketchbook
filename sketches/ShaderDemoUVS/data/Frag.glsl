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

    // lookup pixel color at uv coord vec2 using .xy
    vec4 color = texture2D(texture, vertTexCoord.xy);

    // make red border
    vec2 uv = vertTexCoord.xy;
    if(uv.x < .1 || uv.x > .9 || uv.y > .9 || uv.y < .1){
        color = vec4(1, 0, 0, 1);
    }

    // make blue circle in the center
    // points from this pixel to the center
    vec2 dis = vec2(.5) - uv;

    dis.x /= ratio;

    float d = length(dis); // units in percentage

    if (d < .1){
        color = vec4(0, 0, 1, 1);
    }
    

    // set the pixel color of gl_FragColor
    gl_FragColor = color;

}
