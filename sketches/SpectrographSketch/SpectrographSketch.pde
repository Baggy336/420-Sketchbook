// Spectrogram sketch used for randomness, noise, and a sin wave

// Array for each of the 3 values
//float[] valsWave;
//float[] valsNoise;
//float[] valsRand;

// Can store Wave, Noise, and Random as X, Y, Z in a vector
ArrayList<PVector> vals = new ArrayList<PVector>();

// A collection that can hold floats.
//FloatList vals;

void setup(){
  size(500, 600, P2D);
  stroke(255);
  strokeWeight(2);
}

void draw(){
  background(0);
  
  // Every frame, push a value into an array, move each pixel to the right.
  // Add new values to the Array.
  float time = millis() / 1000.0;
  
  float valWave = map(sin(time), -1, 1, 0, 1); // Puts out -1 to 1. Map can push it to 0 to 1.
  float valRand = random(0 , 1);
  float valNoise = noise(time);
  
  vals.add(0, new PVector(valWave, valRand, valNoise));
  
  // Remove last item if greater than size of screen.
  if(vals.size() > width) vals.remove(vals.size() - 1);
  
  // Draw the Arrays to the screen.
  float third = height/3;
  
  for(int x = 0; x < vals.size(); x++){
    // Split the screen into 3 chunks.
    PVector set = vals.get(x);
    float v1 = set.x;
    float v2 = set.y;
    float v3 = set.z;
    
    float y1 = third - v1 * third;
    float y2 = third * 2 - v2 * third;
    float y3 = height - v3 * third;
    
    point(x, y1);
    point(x, y2);
    point(x, y3);
  }
}
