using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorType
{
    Basic,
    Wall,
    StartTile,
    EndTile
}
public class Terrain : MonoBehaviour
{
    public FloorType type = FloorType.Basic;

    public float moveCost
    {
        get
        {
            if (type == FloorType.Basic) return 1;
            if (type == FloorType.Wall) return 9999;
            if (type == FloorType.StartTile) return 2;
            if (type == FloorType.EndTile) return 1;
            return 1;
        }
    }
}
