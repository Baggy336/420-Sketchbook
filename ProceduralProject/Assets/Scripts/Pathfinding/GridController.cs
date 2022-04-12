using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // A delegate is used within another function
    delegate Pathfinding.Node LookupDelegate(int x, int y);

    // This is unneccesary, but nice to see
    public static GridController _singleton;
    public static GridController singleton { get; private set; }


    public TerrainCube cubePrefab;

    public Transform helperStart;
    public Transform helperEnd;

    private TerrainCube[,] cubes;

    private Pathfinding.Node[,] nodes;

    public int size = 19;

    private void Start()
    {
        // if we already have a gridcontroller
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }

        // set this object to be the singleton 
        singleton = this;
        DontDestroyOnLoad(gameObject);

        MakeGrid();
    }

    private void OnDestroy()
    {
        // clear the singleton if this object is somehow destroyed
        if (this == singleton) singleton = null;
    }

    private void Update()
    {
    }

    void MakeGrid()
    {

        cubes = new TerrainCube[size, size];
        float zoom = 10;
        float amp = 10;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // this can be used for generating an elevated terrain to pathfind across
                float verticalPos = Mathf.PerlinNoise(x / zoom, y / zoom) * amp;

                // Just setting this back to normal
                verticalPos = 0;

                cubes[x, y] = Instantiate(cubePrefab, new Vector3(x, verticalPos, y), Quaternion.identity);

            }
        }
    }

    public void MakeNodes()
    {
        nodes = new Pathfinding.Node[cubes.GetLength(0), cubes.GetLength(1)];

        for(int x = 0; x < cubes.GetLength(0); x++)
        {
            for (int y = 0; y < cubes.GetLength(1); y++)
            {
                Pathfinding.Node n = new Pathfinding.Node();

                n.pos = cubes[x, y].transform.position;

                // If the cube has a wall, movecost is really high
                n.moveCost = cubes[x, y].MoveCost;

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
                Pathfinding.Node neighbor5 = lookup(x + 1, y + 1);
                Pathfinding.Node neighbor6 = lookup(x - 1, y + 1);
                Pathfinding.Node neighbor7 = lookup(x - 1, y - 1);
                Pathfinding.Node neighbor8 = lookup(x + 1, y - 1);

                if (neighbor1 != null) n.neighbors.Add(neighbor1);
                if (neighbor2 != null) n.neighbors.Add(neighbor2);
                if (neighbor3 != null) n.neighbors.Add(neighbor3);
                if (neighbor4 != null) n.neighbors.Add(neighbor4);
                if (neighbor5 != null) n.neighbors.Add(neighbor5);
                if (neighbor6 != null) n.neighbors.Add(neighbor6);
                if (neighbor7 != null) n.neighbors.Add(neighbor7);
                if (neighbor8 != null) n.neighbors.Add(neighbor8);
            }
        }
    }

    public Pathfinding.Node Lookup(Vector3 pos)
    {
        if (nodes == null)
        {
            MakeNodes();
        }

        float w = 1;
        float h = 1;

        pos.x += w / 2;
        pos.z += h / 2;

        int x = (int)(pos.x / w);
        int y = (int)(pos.z / h);

        // Check to make sure values are within the bounds of the array
        if (x < 0 || y < 0) return null;
        if (x >= nodes.GetLength(0) || y >= nodes.GetLength(1)) return null;

        return nodes[x, y];
    }
}

[CustomEditor(typeof(GridController))]
class GridControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if ( GUILayout.Button("Find a path"))
        {
            (target as GridController).MakeNodes();
        }
    }
}
