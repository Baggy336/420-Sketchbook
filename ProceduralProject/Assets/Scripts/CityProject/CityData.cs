using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Nothing,      // 0
    Stubby,       // 1
    Skyscraper,   // 2
    Long,         // 3
    Average,      // 4
    Vegetation,   // 5
    Street        // 6
}
public class CityData
{
    // Holds the value for how many buildings make up a block
    private int buildingsPerBlock = 10;

    // Holds the value for how large the city will be
    private int res = 0;

    // Holds the x and z for buildings in the city
    private int[,] cityBuildings;

    // Holds x and z for the amount of blocks in the city
    private int[,] cityBlocks;

    public void Generate(int size)
    {
        res = size;
        cityBlocks = new int[res, res];

        MakeRoads(BuildingType.Street, BuildingType.Street);
        MakeRoads(BuildingType.Street, BuildingType.Street);
        MakeRoads(BuildingType.Street, BuildingType.Street);
        MakeRoads(BuildingType.Street, BuildingType.Street);

    }
    private void MakeRoads(BuildingType s, BuildingType e)
    {
        // Select the start of the road
        int x = Random.Range(0, res);
        int y = Random.Range(0, res);

        int half = res / 2;

        // Choose the ending of the roads
        int endX = Random.Range(0, half);
        int endY = Random.Range(0, half);

        // If the end is in the same quadrant as the start, push it to the opposite quadrant
        if (x < half) endX += half;
        if (y < half) endY += half;

        // Set a start and end to the road
        SetRoad(x, y, (int) s);
        SetRoad(endX, endY, (int)e);

        // Walk down the roads
        while (x != endX || y != endY)
        {
            int dir = Random.Range(0, 4);
            int dis = Random.Range(2, 6);

            int disX = endX - x;
            int disY = endY - y;

            // 25% of the time, move in a random direction
            if (Random.Range(0, 100) < 75)
            {
                // Pick whichever direction is closer to the target
                if (Mathf.Abs(disX) > Mathf.Abs(disY))
                {
                    dir = (disX > 0) ? 3 : 2;
                }
                else
                {
                    dir = (disY > 0) ? 1 : 0;
                }
            }

            for (int i = 0; i < dis; i++)
            {
                if (dir == 0) y--;
                if (dir == 1) y++;
                if (dir == 2) x--;
                if (dir == 3) x++;

                // Make sure the road does not go outside the map
                x = Mathf.Clamp(x, 0, res - 1);
                y = Mathf.Clamp(y, 0, res - 1);

                // If the road is in an empty spot
                if (GetRoad(x, y) == 0)
                {
                    // Set the road to a road
                    SetRoad(x, y, 6);
                }
            }
        }

    }
    // Create city blocks
    // Generate a height map based on noise

    public int[,] GetRoads()
    {
        int[,] copy = new int[cityBlocks.GetLength(0), cityBlocks.GetLength(1)];
        System.Array.Copy(cityBlocks, 0, copy, 0, cityBlocks.Length);

        return copy;
    }

    private int GetRoad(int x, int y)
    {
        // Check to see if the room chosen is in the bounds of the array
        if (cityBlocks == null) return 0;
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= cityBlocks.GetLength(0)) return 0;
        if (y >= cityBlocks.GetLength(1)) return 0;

        return cityBlocks[x, y];
    }

    private void SetRoad(int x, int y, int val)
    {
        if (cityBlocks == null) return;
        if (x < 0) return;
        if (y < 0) return;
        if (x >= cityBlocks.GetLength(0)) return;
        if (y >= cityBlocks.GetLength(1)) return;

        cityBlocks[x, y] = val;
    }
}
