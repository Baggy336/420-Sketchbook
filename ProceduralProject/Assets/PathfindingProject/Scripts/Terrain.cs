using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorType
{
    Basic,
    Wall,
    StartTile,
    EndTile,
    Tower
}
public class Terrain : MonoBehaviour
{
    public FloorType type = FloorType.Basic;
    public Transform wall;
    public Transform tower;
    private BoxCollider box;

    public float moveCost
    {
        get
        {
            if (type == FloorType.Basic) return 1;
            if (type == FloorType.Wall) return 9999;
            if (type == FloorType.Tower) return 9999;
            return 1;
        }
    }

    private void Start()
    {
        type = FloorType.Basic;
        box = GetComponent<BoxCollider>();
    }

    void OnMouseDown()
    {
        type += 1;

        UpdateArt();

        if (GenerateMap.instance) GenerateMap.instance.MakeTiles();
    }

    void UpdateArt()
    {
        bool isWall = type == FloorType.Wall;

        float yPos = isWall ? .44f : 0f;
        float h = isWall ? 1.1f : .2f;

        box.size = new Vector3(1, h, 1);
        box.center = new Vector3(0, yPos, 0);

        if (wall) wall.gameObject.SetActive(isWall);
        if (isWall && tower) tower.gameObject.SetActive(type == FloorType.Tower);
    }
}
