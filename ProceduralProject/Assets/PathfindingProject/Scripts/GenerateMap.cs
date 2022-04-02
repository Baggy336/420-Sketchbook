using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    delegate Pathfinder.Square Lookup(int x, int z);

    public static GenerateMap _instance;
    public static GenerateMap instance { get; private set; }

    public Terrain tileArt;

    private Terrain[,] tiles;
    private Pathfinder.Square[,] squares;

    public int mapSize = 19;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        MakeMap();
    }

    void MakeMap()
    {
        tiles = new Terrain[mapSize, mapSize];
        float zoom = 10;
        float amp = 10;

        for (int x = 0; x < mapSize; x++)
        {
            for (int z = 0; z < mapSize; z++)
            {
                float verticalPos = Mathf.PerlinNoise(x / zoom, z / zoom) * amp;

                verticalPos = 0;

                tiles[x, z] = Instantiate(tileArt, new Vector3(x, verticalPos, z), Quaternion.identity);
            }
        }
    }

    void MakeTiles()
    {
        squares = new Pathfinder.Square[tiles.GetLength(0), tiles.GetLength(1)];

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int z = 0; z < tiles.GetLength(1); z++)
            {
                Pathfinder.Square s = new Pathfinder.Square();

                s.pos = tiles[x, z].transform.position;

                s.moveCost = tiles[x, z].moveCost;

                squares[x, z] = s;
            }
        }

        Lookup findTile = (x, z) => {
            if (x < 0) return null;
            if (z < 0) return null;
            if (x >= squares.GetLength(0)) return null;
            if (z >= squares.GetLength(1)) return null;
            return squares[x, z];
        };

        for (int x = 0; x < squares.GetLength(0); x++)
        {
            for (int z = 0; z < squares.GetLength(1); z++)
            {
                Pathfinder.Square s = squares[x, z];

                Pathfinder.Square neighbor1 = findTile(x + 1, z);
                Pathfinder.Square neighbor2 = findTile(x - 1, z);
                Pathfinder.Square neighbor3 = findTile(x, z + 1);
                Pathfinder.Square neighbor4 = findTile(x, z - 1);
                Pathfinder.Square neighbor5 = findTile(x + 1, z + 1);
                Pathfinder.Square neighbor6 = findTile(x - 1, z + 1);
                Pathfinder.Square neighbor7 = findTile(x - 1, z - 1);
                Pathfinder.Square neighbor8 = findTile(x + 1, z - 1);

                if (neighbor1 != null) s.neighborSquares.Add(neighbor1);
                if (neighbor2 != null) s.neighborSquares.Add(neighbor2);
                if (neighbor3 != null) s.neighborSquares.Add(neighbor3);
                if (neighbor4 != null) s.neighborSquares.Add(neighbor4);
                if (neighbor5 != null) s.neighborSquares.Add(neighbor5);
                if (neighbor6 != null) s.neighborSquares.Add(neighbor6);
                if (neighbor7 != null) s.neighborSquares.Add(neighbor7);
                if (neighbor8 != null) s.neighborSquares.Add(neighbor8);
            }
        }
    }

    public Pathfinder.Square LookupTile(Vector3 pos)
    {
        if (squares == null) MakeMap();

        float w = 1;
        float h = 1;

        pos.x += w / 2;
        pos.z += h / 2;

        int x = (int)(pos.x / w);
        int z = (int)(pos.z / h);

        if (x < 0 || z < 0) return null;
        if (x >= squares.GetLength(0) || z >= squares.GetLength(1)) return null;

        return squares[x, z];
    }
}
