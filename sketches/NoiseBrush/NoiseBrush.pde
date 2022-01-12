// A visualizer for noise.

void setup(){
  size(600, 300);
}

void draw(){
  //background(0);
  
  float time = millis() / 1000.0;
  
  // Set circle diameter
  float d3 = map(noise(time), 0, 1, 50, 200);
  
  // Draw ellipses based on the diameter value
  ellipse(mouseX, mouseY, d3, d3);
}
