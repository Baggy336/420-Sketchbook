class FlowFieldGrid {

  int res = 10;
  float[][] data;
  float zoom = 10;
  boolean isHidden = false;

  FlowFieldGrid() {
    Build();
  }
  void Build() {
    data = new float[res][res];

    int threshold = 3;
    float w = GetCellWidth();
    float h = GetCellHeight();

    // seed with perlin noise
    for (int x = 0; x < data.length; x++) {
      for (int y = 0; y < data[x].length; y++) {
        float val = noise(x / zoom, y / zoom);
        float threshVal = noise(x / zoom, y / zoom);
        val = map(val, 0, 1, -PI * 2, PI * 2);

        // Detect if the cell is near the side of the screen
        // if it is, calculate a value that points back to the center. (atan2)

        if (x < threshold || y < threshold || x >= data.length - threshold || y >= data[0].length - threshold) {
          float dy = (height / (float) 2) - (y * h + h / 2);
          float dx = (width / (float) 2) - (x * w + w / 2);
          threshVal = atan2(dy, dx);
          val = lerp(threshVal, val, .16f);
        }
        

        data[x][y] = val;
      }
    }
  }

  void update() {
    boolean rebuild = false;

    if (Keys.onDown(32)) {
      isHidden = !isHidden;
      rebuild = true;
    }
    if (Keys.onDown(37)) {
      res--;
      rebuild = true;
    }
    if (Keys.onDown(39)) {
      res++;
      rebuild = true;
    }
    if (Keys.onDown(38)) {
      zoom += 1;
      rebuild = true;
    }
    if (Keys.onDown(40)) {
      zoom -= 1;
      rebuild = true;
    }

    res = constrain(res, 4, 50);
    zoom = constrain(zoom, 5, 50);

    if (rebuild) Build();
  }
  void draw() {

    if (isHidden) return;

    float w = GetCellWidth();
    float h = GetCellHeight();

    for (int x = 0; x < data.length; x++) {
      for (int y = 0; y < data[x].length; y++) {
        float val = data[x][y];

        float topLeftX = x * w;
        float topLeftY = y * h;

        pushMatrix();
        translate(topLeftX + w / 2, topLeftY + h / 2);
        rotate(val);

        float hue = map(val, -PI, PI, 0, 255);

        stroke(255);
        fill(hue, 255, 255);
        ellipse(0, 0, 10, 10);

        line(0, 0, 20, 0);
        popMatrix();
      }
    }
  }

  // Convenience methods for getting the size of each cell
  float GetCellWidth() {
    return width / res;
  }
  float GetCellHeight() {
    return height / res;
  }
  float getDirectionAt(PVector p) {
    return getDirectionAt(p.x, p.y);
  }
  float getDirectionAt(float x, float y) {
    int iX = (int) (x / GetCellWidth());
    int iY = (int) (y / GetCellHeight());

    if (iX < 0 || iY < 0 || iX >= data.length || iY >= data[0].length) {
      float dy = (height / (float) 2) - y;
      float dx = (width / (float) 2) - x;

      return atan2(dy, dx);
    }

    return data[iX][iY];
  }
}
