using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding1
{
    private const int StraightMoveCost = 1;
    private const int DiagonalMoveCost = 3;

    // Singleton, can assign pathfinding to multiple object  by referencing the instance
    public static Pathfinding1 _instance { get; private set; }

    private GridSystem<PathfindingNode> grid;
    private List<PathfindingNode> nodesToCheck;
    private List<PathfindingNode> checkedNodes;

    public Pathfinding1(int width, int height)
    {
        grid = new GridSystem<PathfindingNode>(width, height, 5f, Vector3.zero, (GridSystem<PathfindingNode> g, int x, int z) => new PathfindingNode(g, x, z));
        
        // Set the singleton to this Pathfinding script
        _instance = this;
    }

    public List<PathfindingNode> FindPath(int startX, int startZ, int endX, int endZ)
    {
        PathfindingNode startNode = grid.GetVal(startX, startZ);
        PathfindingNode endNode = grid.GetVal(endX, endZ);

        nodesToCheck = new List<PathfindingNode> { startNode };
        checkedNodes = new List<PathfindingNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int z = 0; z < grid.GetHeight(); z++)
            {
                PathfindingNode pathNode = grid.GetVal(x, z);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateF();
                pathNode.previousNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDis(startNode, endNode);
        startNode.CalculateF();

        while(nodesToCheck.Count > 0)
        {
            PathfindingNode currentNode = FindLowestCost(nodesToCheck);

            if (currentNode == endNode)
            {
                return FindPath(endNode);
            }

            nodesToCheck.Remove(currentNode);
            checkedNodes.Add(currentNode);

            foreach(PathfindingNode neighbor in FindNeighbors(currentNode))
            {
                if (checkedNodes.Contains(neighbor)) continue;
                if (!neighbor.canBePassed)
                {
                    checkedNodes.Add(neighbor);
                    continue;
                }

                int tempG = currentNode.gCost + CalculateDis(currentNode, neighbor);
                if (tempG < neighbor.gCost)
                {
                    neighbor.previousNode = currentNode;
                    neighbor.gCost = tempG;
                    neighbor.hCost = CalculateDis(neighbor, endNode);
                    neighbor.CalculateF();

                    if (!nodesToCheck.Contains(neighbor))
                    {
                        nodesToCheck.Add(neighbor);
                    }
                }
            }
        }

        // Done checking the open list and came out with nothing
        return null;
    }

    private List<PathfindingNode> FindNeighbors(PathfindingNode node)
    {
        List<PathfindingNode> neighbors = new List<PathfindingNode>();

        if (node.x - 1 >= 0)
        {
            // To the left
            neighbors.Add(GetNode(node.x - 1, node.z));
            // To the left, and down diagonally
            if (node.z - 1 >= 0) neighbors.Add(GetNode(node.x - 1, node.z - 1));
            // To the left, and up diagonally
            if (node.z + 1 < grid.GetHeight()) neighbors.Add(GetNode(node.x - 1, node.z + 1));
        }
        if (node.x + 1 < grid.GetWidth())
        {
            // To the right
            neighbors.Add(GetNode(node.x + 1, node.z));
            // To the right, and down diagonally
            if (node.z - 1 >= 0) neighbors.Add(GetNode(node.x + 1, node.z - 1));
            // To the right, and up diagonally
            if (node.z + 1 < grid.GetHeight()) neighbors.Add(GetNode(node.x + 1, node.z + 1));
        }
        // Straight down
        if (node.z - 1 >= 0) neighbors.Add(GetNode(node.x, node.z - 1));
        // Straight up
        if (node.z + 1 < grid.GetHeight()) neighbors.Add(GetNode(node.x, node.z + 1));

        return neighbors;
    }

    private PathfindingNode GetNode(int x, int z)
    {
        return grid.GetVal(x, z);
    }

    private List<PathfindingNode> FindPath(PathfindingNode endNode)
    {
        List < PathfindingNode > path = new List<PathfindingNode>();
        path.Add(endNode);
        PathfindingNode currentNode = endNode;
        while (currentNode.previousNode != null)
        {
            path.Add(currentNode.previousNode);
            currentNode = currentNode.previousNode;
        }

        path.Reverse();
        return path;
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 endPos)
    {
        grid.GetXZ(startPos, out int startX, out int startZ);
        grid.GetXZ(endPos, out int endX, out int endZ);

        List<PathfindingNode> path = FindPath(startX, startZ, endX, endZ);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> unitPath = new List<Vector3>();
            foreach(PathfindingNode node in path)
            {
                unitPath.Add(new Vector3(node.x, node.z) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return unitPath;
        }
    }

    private int CalculateDis(PathfindingNode a, PathfindingNode b)
    {
        int xDis = Mathf.Abs(a.x - b.x);
        int zDis = Mathf.Abs(a.z - b.z);
        int remainder = Mathf.Abs(xDis - zDis);

        return DiagonalMoveCost * Mathf.Min(xDis, zDis) + StraightMoveCost * remainder;
    }  

    private PathfindingNode FindLowestCost(List<PathfindingNode> nodesList)
    {
        PathfindingNode lowestCost = nodesList[0];
        for (int i = 1; i < nodesList.Count; i++)
        {
            if (nodesList[i].fCost < lowestCost.fCost)
            {
                lowestCost = nodesList[i];
            }
        }

        return lowestCost;
    }
}
