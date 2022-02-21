class AgentTarget{
  PVector pos;
  PVector target = new PVector();
  PVector target2 = new PVector();
  
  int cooldown = 0; 
  
  AgentTarget(){
    pos = new PVector(random(0, width), random(0, height));
  }
  
  void update(){
    if (--cooldown <= 0){
      target = new PVector(random(0, width), random(0, height));
      cooldown = (int) random(30, 60);
    }
     target2.x += (target.x - target2.x) * .01;
     target2.y += (target.y - target2.y) * .01;
     
     // ease position to target
     pos.x += (target2.x - pos.x) * .02;
     pos.y += (target2.y - pos.y) * .02;
   }
   void draw(){
     // ellipse(target2.x, target2.y, 10, 10);
     ellipse(pos.x, pos.y, 40, 40);
   }
  
}
