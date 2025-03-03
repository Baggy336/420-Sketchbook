PImage img;
PShader shader;

void setup(){
  size(800, 600, P2D);
  img = loadImage("Background.jpg"); 
  imageMode(CENTER);
  
  shader = loadShader("Frag.glsl", "Vert.glsl");
  
  shader(shader);
}

void draw(){
  background(100);
  
  image(img, mouseX, mouseY);
  
  //filter(shader);
}
