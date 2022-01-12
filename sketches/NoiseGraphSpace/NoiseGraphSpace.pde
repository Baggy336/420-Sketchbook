// Spectrogram sketch used for randomness, noise, and a sin wave

void setup(){
  size(500, 600, P2D);
  stroke(255);
  strokeWeight(2);
}

void draw(){
  background(0);
  
  // Every frame, push a value into an array, move each pixel to the right.
  float time = millis() / 1000.0;
  
  float zoomAmt = map(mouseX, 0, width, 10, 100);
  
  float third = height/3;
  
  for(int x = 0; x < width; x++){
    // Split the screen into 3 chunks.
    float v1 = map(sin(x/zoomAmt + time), -1, 1, 0, 1);
    float v2 = random(0 , 1);
    float v3 = noise(x/zoomAmt + time);
    
    float y1 = third - v1 * third;
    float y2 = third * 2 - v2 * third;
    float y3 = height - v3 * third;
    
    point(x, y1);
    point(x, y2);
    point(x, y3);
  }
}
