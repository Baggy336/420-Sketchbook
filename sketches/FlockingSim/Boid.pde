class Boid {
  int TYPE = 1;

  PVector pos = new PVector();
  PVector vel = new PVector();
  PVector force = new PVector();
  PVector _dir = new PVector();

  float mass = 1;
  float speed = 10;

  float radiusCohesion = 200;
  float radiusAlignment = 100;
  float radiusSeparation = 50;

  float forceCohesion = .85;
  float forceAlignment = .2;
  float forceSeparation = 5;

  Boid(float x, float y) {
    pos.x = x;
    pos.y = y;

    vel.x = random(-3, 3);
    vel.y = random(-3, 3);
  }

  void calcForces(Flock f) {
    // Calc forces

    // pull to group center
    // check for nearby boids, push apart if needed
    // turn boid to nearby avg dir

    PVector groupCenter = new PVector();
    PVector avgAlignment = new PVector();
    int numCohesion = 0;
    int numAlignment = 0;

    for (Boid b : f.boids) {
      // if the boid is calculating against itself
      if (b == this) continue;

      float dx = b.pos.x - pos.x;
      float dy = b.pos.y - pos.y;
      float dis = sqrt(dx * dx + dy * dy);

      if (TYPE == 1 && b.TYPE == 1) {
        if (dis < radiusCohesion) {
          groupCenter.add(b.pos);
          numCohesion++;
        }
        if (dis < radiusSeparation) {
          PVector awayB = new PVector(-dx / dis, -dy / dis);
          awayB.mult(forceSeparation / dis);

          force.add(awayB);
        }
        if (dis < radiusAlignment) {
          avgAlignment.add(b._dir);
          numAlignment++;
        }
      }
    }

    if (numCohesion > 0) {
      groupCenter.div(numCohesion);
      // steer towards group center
      PVector dirToCenter = PVector.sub(groupCenter, pos);
      dirToCenter.setMag(speed);

      PVector cohesionForce = PVector.sub(dirToCenter, vel);

      // Clamp the value to our predefined forceCohesion
      cohesionForce.limit(forceCohesion);
      force.add(cohesionForce);
    }

    if (numAlignment > 0) {
      avgAlignment.div(numAlignment);
      avgAlignment.mult(speed);

      PVector alignmentForce = PVector.sub(avgAlignment, vel);

      alignmentForce.limit(forceAlignment);
      force.add(alignmentForce);
    }
  }

  void updateAndDraw() {
    // euler physics in draw (for performance)
    PVector acceleration = PVector.div(force, mass);
    vel.add(acceleration);
    pos.add(vel);
    force = new PVector(0, 0, 0);

    // keep the flock within view
    if (pos.x < 0) pos.x += width;
    else if (pos.x > width) pos.x -= width;

    if (pos.y < 0) pos.y += height;
    else if (pos.y > height) pos.y -= height;

    _dir = PVector.div(vel, vel.mag());

    ellipse(pos.x, pos.y, 10, 10);
  }
}
