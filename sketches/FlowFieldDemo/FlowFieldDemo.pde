ArrayList<Agent> agents = new ArrayList<Agent>();

FlowFieldGrid grid;

int cooldown = 0;
int stayColor = 0;

float newR = 0;
float newG = 0;
float newB = 0;

void setup() {
  size(1000, 800);
  grid = new FlowFieldGrid();
  background(0);
  noStroke();
  colorMode(HSB);
}

void draw() {
  noStroke();
  //fill(0, 0, 0, 70);
  //rect(0, 0, width, height);

  background(0);
  if (mousePressed) {
    agents.add( new Agent() );
  }

  grid.update();
  grid.draw();

  // if the cooldown to change color is up
  if (--cooldown <= 0) {
    // choose new values
    newR = random(0, 255);
    newG = random(0, 255);
    newB = random(0, 255);
    fill(newR, newG, newB);
    // reset the timer
    cooldown = (int) random(60, 120);
  } // else if the staycolor timer is up
  else if (--stayColor >= 0) {
    fill(newR, newG, newB);
  } else { // otherwise fill white
    fill(255);
    // and reset the timer
    stayColor = (int) random(200, 500);
  }

  // update and draw each agent with a new color

  for (Agent a : agents) {
    a.update();
    a.draw();
  }

  Keys.update();
}
