class Polygon{
  
  // transform components (position, rotation, scale)
  PVector pos = new PVector();
  float rot = 0;
  float scale = 1;
  
  // Local space points
  ArrayList<PVector> points = new ArrayList<PVector>();
  
  // cached values 
  
  // World space points
  ArrayList<PVector> worldPoints = new ArrayList<PVector>();
  
  // World space edge normals
  ArrayList<PVector> dirs = new ArrayList<PVector>();
  
  PVector min = new PVector();
  PVector max = new PVector();
   
  boolean isColliding = false; 
  
  Polygon(int steps){   
    
    pos.x = width / 2;
    pos.y = height / 2;
    
    // Add 10 points to the array
    for (int i = 0; i < steps; i++){
      float radians = TWO_PI * i / (float)steps; // Gives a percentage of the way around the circle
      float mag = 50;
      
      points.add(new PVector(mag * cos(radians), mag * sin(radians)));
    }
  }
  
  void update(){
    cacheValues();
  }
  
  void cacheValues(){
    PMatrix2D xform = new PMatrix2D();
    
    xform.translate(pos.x, pos.y);
    xform.rotate(rot);
    xform.scale(scale);
    
    worldPoints.clear();
    for(PVector p : points){
      PVector temp = new PVector();
      xform.mult(p, temp);
      worldPoints.add(temp);
      
    }
    
    dirs.clear();
    // find min and max
    int i = 0;
    for (PVector p : worldPoints){
      // find the min and max for the object
      if (i == 0 || p.x > max.x) max.x = p.x;
      if (i == 0 || p.x < min.x) min.x = p.x;
      if (i == 0 || p.y > max.y) max.y = p.y;
      if (i == 0 || p.y < min.y) min.y = p.y;
      
      { // calculate the normals 
        // Get previous point, if there is no previous point, get the last point
        PVector p0 = worldPoints.get(i == 0 ? worldPoints.size() - 1 : i - 1);
        PVector p1 = worldPoints.get(i);
      
        PVector d = PVector.sub(p1, p0); // the vector from p0 to p1
        d.normalize(); // Turn the vector into just a direction
        d = new PVector(d.y, -d.x); // flip x and y to get perpendicular values
        
        dirs.add(d);
      }
      i++;
    }
  }
  
  boolean doesOverlap(Polygon gon){
    // AABB check:
    if (this.min.x > gon.max.x) return false; // to the right
    if (this.max.x < gon.min.x) return false; // to the left
    
    if (this.min.y > gon.max.y) return false; // below
    if (this.max.y < gon.min.y) return false; // above
    
    // TODO: check all axis
    ArrayList<PVector> axes = new ArrayList<PVector>();
    axes.addAll(this.dirs);
    axes.addAll(gon.dirs);
    
    for (PVector axis : axes){
       MinMax a = this.projectOn(axis);
       MinMax b = gon.projectOn(axis);
       
       if (a.min > b.max) return false;
       if (a.max < b.min) return false;
     }
    // project every point onto each axis
    // look for a gap, return false if there is a gap
    
    return true; // the objects are overlapping
  }
  
  // Convenience method for setting min and max
  MinMax projectOn(PVector axis){
    MinMax mm = new MinMax();
    
    int i = 0;
    for (PVector p : worldPoints){
       float val = PVector.dot(axis, p);
       
       if (i == 0 || val < mm.min) mm.min = val;
       if (i == 0 || val > mm.max) mm.max = val;
       i++;
    }
    
    return mm;
  }
  
  void draw(){  
    
    noFill();
    stroke(160);
    rect(min.x, min.y, max.x - min.x, max.y - min.y);
    
    stroke(0);
    if (isColliding) fill(255, 0, 0);
    else fill(255);
    beginShape();
    // pull values from array
    int i = 0;
    for(PVector p : worldPoints){
      vertex(p.x, p.y);
      
      PVector p0 = worldPoints.get(i == 0 ? worldPoints.size() - 1 : i - 1);
      
      PVector mid = PVector.div(PVector.add(p, p0), 2);
      
      PVector d = dirs.get(i);
      line(mid.x, mid.y, mid.x + d.x * 20, mid.y + d.y * 20);
      i++;
    }
    
    endShape(CLOSE);
  }
}
