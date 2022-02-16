
class Agent{
  color colour;
  
  PVector pos = new PVector();
  PVector vel = new PVector();
  PVector force = new PVector();
  
  float size;
  float mass;
  
  boolean hasUpdatedGrav = false;
  
  Agent(float massMin, float massMax){
     pos.x = random(0, width);
     pos.y = random(0, height);
     
     mass = random(massMin, massMax);
     size = sqrt(mass);
     
     colorMode(HSB);
     colour = color(random(0, 255), 255, 255);
  }
  
  void update(){
    
    for(Agent a : agents){
      if (a == this) continue; // skip the div by 0
      if (a.hasUpdatedGrav) continue;
      
      PVector f = findGravityForce(a);
      force.add(f);
    }
    hasUpdatedGrav = true;
   
    // a = f/m
    PVector acceleration = PVector.div(force, mass);
    
    // clear force
    force.set(0, 0);
    
    // v += a
    vel.add(acceleration);
    
    // p += v
    pos.add(vel);
    
  }
  
  void draw(){
    fill(colour);
    ellipse(pos.x, pos.y, size, size);  
    
  }
  
  PVector findGravityForce(Agent a){
      
    PVector vToAgent = PVector.sub(a.pos, pos); 
    
    // clip the vector to be 1 unit long
    float r = vToAgent.mag();
    
    // directionless gravity
    float gravForce = kG * (a.mass * mass) / (r * r);
    
    if (gravForce > maxForce) gravForce = maxForce;
    
    vToAgent.normalize();
    vToAgent.mult(gravForce);
    
    // add force to agent a
    a.force.add(PVector.mult(vToAgent, -1));
    
    return vToAgent;
  }
  
}
