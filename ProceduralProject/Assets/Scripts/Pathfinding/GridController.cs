using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // A delegate is used within another function
    delegate Pathfinding.Node LookupDelegate(int x, int y);

    public TerrainCube cubePrefab;

    private TerrainCube[,] cubes;

    public int size = 19;

    private void Start()
    {
        MakeGrid();
    }

    void MakeGrid()
    {

        cubes = new TerrainCube[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                cubes[x, y] = Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity);

            }
        }
    }

    void MakeNodes()
    {
        Pathfinding.Node[,] nodes = new Pathfinding.Node[cubes.GetLength(0), cubes.GetLength(1)];

        for(int x = 0; x < cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubes.GetLength(1); y++)
            {
                Pathfinding.Node n = new Pathfinding.Node();

                n.pos = cubes[x, y].transform.position;

                // If the cube has a wall, movecost is really high
                n.moveCost = cubes[x, y].isSolid ? 9999 : 1;

                

                nodes[x, y] = n;
            }
        }

        // Anonymous function only exists within this function
        LookupDelegate lookup = (x, y) => {
            if (x < 0) return null;
            if (y < 0) return null;
            if (x >= nodes.GetLength(0)) return null;
            if (y >= nodes.GetLength(1)) return null;
            return nodes[x, y];
        };

        // Lookup each neighbor within the nodes
        for (int x = 0; x < nodes.GetLength(0); x++)
        {
            for (int y = 0; y < nodes.GetLength(1); y++)
            {
                Pathfinding.Node n = nodes[x, y];

                Pathfinding.Node neighbor1 = lookup(x + 1, y);
                Pathfinding.Node neighbor2 = lookup(x - 1, y);
                Pathfinding.Node neighbor3 = lookup(x, y + 1);
                Pathfinding.Node neighbor4 = lookup(x, y - 1);

                if (neighbor1 != null) n.neighbors.Add(neighbor1);
                if (neighbor2 != null) n.neighbors.Add(neighbor2);
                if (neighbor3 != null) n.neighbors.Add(neighbor3);
                if (neighbor4 != null) n.neighbors.Add(neighbor4);
            }
        }

        Pathfinding.Node start = nodes[
            (int)Random.Range(0, size),
            (int)Random.Range(0, size)
        ];
        Pathfinding.Node end = nodes[
            (int)Random.Range(0, size),
            (int)Random.Range(0, size)
        ];

        List<Pathfinding.Node> path = Pathfinding.Solve(start, end);
    }
}
