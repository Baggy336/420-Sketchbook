// A visualizer for Sin wave, random, and noise.

void setup(){
  size(600, 300);
}

void draw(){
  background(0);
  
  float time = millis() / 1000.0;
  
  // Set circle diameters
  float d1 = map(sin(time), -1, 1, 50, 200);
  float d2 = random(50, 200);
  float d3 = map(noise(time), 0, 1, 50, 200);
  
  // Draw ellipses based on the diameter values
  ellipse(width*1/4, height/2, d1, d1);
  ellipse(width*1/2, height/2, d2, d2);
  ellipse(width*3/4, height/2, d3, d3);
}
