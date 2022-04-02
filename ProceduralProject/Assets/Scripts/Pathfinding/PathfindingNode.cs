using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode
{
    private GridSystem<PathfindingNode> grid;

    public int x;
    public int z;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool canBePassed;
    public PathfindingNode previousNode;

    public PathfindingNode(GridSystem<PathfindingNode> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        canBePassed = true;
    }

    public void CalculateF()
    {
        fCost = gCost + hCost;
    }


}
