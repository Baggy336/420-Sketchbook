
ArrayList<PVector> blocks = new ArrayList<PVector>();

float threshold = 0.5;
float zoom = 10;

int dimOfBlocks = 10;
float blockSize = 30;

void setup() {
  size(800, 500, P3D);
  noStroke();
  generateTerrainData();
}

void generateTerrainData() {
  
  blocks.clear();
  
  float[][][] data = new float[dimOfBlocks][dimOfBlocks][dimOfBlocks];

  for (int x = 0; x < dimOfBlocks; x++) {
    for (int y = 0; y < dimOfBlocks; y++) {
      for (int z = 0; z < dimOfBlocks; z++) {
        data[x][y][z] = noise(x/zoom, y/zoom, z/zoom) + y / 10.0;
      }
    }
  }
  
  // Check for occlusion (take each position and check for neighbors).
  
  for (int x = 0; x < dimOfBlocks; x++) {
    for (int y = 0; y < dimOfBlocks; y++) {
      for (int z = 0; z < dimOfBlocks; z++) {
        if(data[x][y][z] > threshold){
          blocks.add(new PVector(x, y, z));
        }
      }
    }
  }
}

void checkInput(){
  boolean shouldRegen = false;
  
  if(Keys.PLUS()){
     threshold += .01;
     shouldRegen = true;
  }
  if(Keys.MINUS()){
     threshold -= .01;
     shouldRegen = true;
  }
  
  // Use the brackets to zoom in our out of the noise field
  if(Keys.BRACKET_LEFT()){
     zoom += .1;
     shouldRegen = true;
  }
  if(Keys.BRACKET_RIGHT()){
     zoom -= .1;
     shouldRegen = true;
  }
  
  if(shouldRegen){
    threshold = constrain(threshold, 0, 1);
    zoom = constrain(zoom, 1, 50);
    generateTerrainData();
  }
}

void draw() {
  checkInput();
  
  background(0);

  lights();
  
  // push and pop matrix for the camera
  pushMatrix();
  
  // Move the origin to the center of the screen
  translate(width/2, height/2);
  
  rotateY(map(mouseX, 0, width, -PI, PI));
  rotateX(map(mouseY, 0, height, -1, 1));
  
  // Move the chunk to the left
  translate(-dimOfBlocks * blockSize / 2, -dimOfBlocks * blockSize / 2);
  
  for(PVector pos : blocks){
    // Push and pop matrix for each cube
    pushMatrix();
    translate(pos.x * blockSize, pos.y * blockSize, pos.z * blockSize);
    box(blockSize, blockSize, blockSize);
    popMatrix();
  }
  
  popMatrix();
}
