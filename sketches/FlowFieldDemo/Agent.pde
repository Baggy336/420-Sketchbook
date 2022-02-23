class Agent{
  PVector pos = new PVector();
  PVector vel = new PVector();
  PVector force = new PVector();
  PVector target = new PVector();
  
  float mass = 1;
  float maxSpeed = 10;
  float maxForce = 10;
  float targetAngle = 0;
  float targetRad = 100;
  float targetSpin = 0;
  
  Agent(){
    pos = new PVector(mouseX, mouseY);
    vel = new PVector(random(-5, 5), random(-5, 5));
    mass = random(50, 100);
    maxForce = random(5, 15);
    targetAngle = random(-PI, PI);
    targetRad = random(50, 150);
    maxSpeed = random(2, 15);
    targetSpin = map(maxForce, 5, 15, .005, .05);
  }
  
  void update(){
    float angle = grid.getDirectionAt(pos);
    
    PVector offset = PVector.fromAngle(angle);
    offset.mult(100);
    
    target = PVector.add(pos, offset); 
    
    SteeringForce();
    DoEuler();
  }
  
  void SteeringForce(){
    // find desired vel
    // desired vel = target pos - current pos (normalized)
    PVector desiredVel = PVector.sub(target, pos);
    desiredVel.normalize();
    desiredVel.mult(maxSpeed);
    // desiredVel.setMag(maxSpeed); (same as 2 above)
    
    // find steeringforce
    // steering force = desired vel - current vel (clamped)
    PVector steeringForce = PVector.sub(desiredVel, vel);
    steeringForce.limit(maxForce); // if steering force is longer than max force
    force.add(steeringForce);
  }
  
  void DoEuler(){
    // euler integration
    PVector acceleration = PVector.div(force, mass);   
    vel.add(acceleration);
    pos.add(vel); // pos += vel
    force.mult(0);
  }
  
  void draw(){
    //ellipse(target.x, target.y, 10, 10);
    
    float a = vel.heading();
    
    pushMatrix();
    
    translate(pos.x, pos.y);
    rotate(a);
    triangle(5, 0, -10, 5, -10, -5);
    
    popMatrix();
  }  
}
