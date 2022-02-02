using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public GameObject basicHallway;
    public GameObject playerStart;
    public GameObject playerEnd;
    public GameObject pointOfInterest;
    private GameObject objectToSpawn;

    public int roomSize = 10;
    public int resolution = 50;
    int[,] rooms;

    public int smallPerLarge = 5;
    int lowRes() { return resolution / smallPerLarge; }
    int[,] bigRooms;

    

    private void Update()
    {
        float px = roomSize;
        for (int x = 0; x < rooms.Length; x++)
        {
            for (int y = 0; y < rooms.GetLength(x); y++)
            {
                int value = rooms[x, y];
                if (value > 0)
                {
                    switch (value)
                    {
                        case 1:
                            objectToSpawn = basicHallway;
                            break;
                        case 2:
                            objectToSpawn = pointOfInterest;
                            break;
                        case 3:
                            objectToSpawn = playerStart;
                            break;
                        case 4:
                            objectToSpawn = playerEnd;
                            break;

                    }
                    transform.position += x * px + y * px;
                    Instantiate(objectToSpawn,);
                }
            }
        }

        px = roomSize * smallPerLarge;
        for (int x = 0; x < bigRooms.Length; x++)
        {
            for (int y = 0; y < bigRooms.GetLength(x); y++)
            {
                int value = rooms[x, y];
                if (value > 0)
                {
                    switch (value)
                    {
                        case 1:
                            objectToSpawn = basicHallway;
                            break;
                        case 2:
                            objectToSpawn = pointOfInterest;
                            break;
                        case 3:
                            objectToSpawn = playerStart;
                            break;
                        case 4:
                            objectToSpawn = playerEnd;
                            break;
                    }
                }
            }
        }
    }
    void GenerateRooms()
    {
        rooms = new int[resolution, resolution];

        // generate a start, end, and 6 POIs
        WalkRooms(3, 4);
        WalkRooms(2, 2);
        WalkRooms(2, 2);
        WalkRooms(2, 2);

        MakeBigRooms();

        RemoveRooms();
    }

    int GetRoom(int x, int y)
    {
        // Make sure x and y don't fall out of bounds of the array
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x > rooms.Length) return 0;
        if (y > rooms.GetLength(x)) return 0;

        // If the room is within the array, return it out of the function
        return rooms[x, y];
    }

    void SetRoom(int x, int y, int temp)
    {
        // Make sure x and y don't fall out of bounds of the array
        if (x < 0) return;
        if (y < 0) return;
        if (x > rooms.Length) return;
        if (y > rooms.GetLength(x)) return;

        // Store the room that the GetRoom function finds in a temporary value
        int t = GetRoom(x, y);

        // Check to see that the room from the GetRoom function
        // is smaller than the temp value set
        if (t < temp) rooms[x, y] = temp;
    }

    int GetBigRoom(int x, int y)
    {
        if (x < 0) return 0;
        if (y < 0) return 0;
        if (x >= bigRooms.Length) return 0;
        if (y >= bigRooms.GetLength(x)) return 0;

        return bigRooms[x, y];
    }
    void SetBigRoom(int x, int y, int temp)
    {
        if (x < 0) return;
        if (y < 0) return;
        if (x >= bigRooms.Length) return;
        if (y >= bigRooms.GetLength(x)) return;

        bigRooms[x, y] = temp;
    }

    void MakeBigRooms()
    {
        int resolution = lowRes();
        bigRooms = new int[resolution, resolution];
        
        for(int x = 0; x < rooms.Length; x++)
        {
            for(int y = 0; y < rooms.GetLength(x); y++)
            {
                int value = GetRoom(x, y);

                int value2 = bigRooms[x / smallPerLarge, y / smallPerLarge];

                if (value > value2) bigRooms[x / smallPerLarge, y / smallPerLarge] = value;
            }
        }

    }
    void WalkRooms(int roomType1, int roomType2)
    {
        // Get the length of half of the room's width and height
        int halfWidth = rooms.Length / 2;
        int halfHeight = rooms.GetLength(0) / 2;

        int x = Random.Range(0, rooms.Length);
        int y = Random.Range(0, rooms.GetLength(x));

        // Generate random temporary values between 0 and half Width and Height
        int tempX = Random.Range(0, halfWidth);
        int tempY = Random.Range(0, halfHeight);

        // If the starting point is left, move to the right
        if (x < halfWidth) tempX += halfWidth;
        // If the starting point is on top, move to the bottom
        if (y < halfHeight) tempY += halfHeight;

        // Set the start and end points of the dungeon
        SetRoom(x, y, roomType1);
        SetRoom(tempX, tempY, roomType2);

        while(x != tempX || y != tempY)
        {
            int dir = Random.Range(0, 4); // 0 to 3
            int dis = Random.Range(1, 4); // 1 to 3

            if (Random.Range(0, 100) > 50)
            {
                int dx = tempX - x;
                int dy = tempY - y;

                // Check to see if we are closer to the end on x or y axis
                if (Mathf.Abs((dx)) < Mathf.Abs((dy)))
                {
                    dir = (dy < 0) ? 0 : 1;
                }
                else
                {
                    dir = (dx < 0) ? 3 : 2;
                }
            }

            for (int i = 0; i < dis; i++)
            {
                switch (dir)
                {
                    case 0:
                        y--; // move north
                        break;
                    case 1:
                        y++; // move south
                        break;
                    case 2:
                        x++; // move east
                        break;
                    case 3:
                        x--; // move west
                        break;
                }
                x = Mathf.Clamp(x, 0, resolution - 1);
                y = Mathf.Clamp(y, 0, resolution - 1);
                SetRoom(x, y, 1);

            }

        }

    }

    void RemoveRooms()
    {
        for (int x = 0; x < bigRooms.Length; x++)
        {
            for (int y = 0; y < bigRooms.GetLength(x); y++)
            {
                int value = GetBigRoom(x, y);

                // Check to see if the chosen room is a POI
                if (value != 1) continue;

                // Punch a hole 25% of the time
                if (Random.Range(0, 100) < 25) continue;

                // Make an array of integers for the 8 neighbor rooms
                int[] neighborRoom = new int[8];

                neighborRoom[0] = GetBigRoom(x, y - 1); // get the room above this by subtracting 1
                neighborRoom[1] = GetBigRoom(x + 1, y - 1);
                neighborRoom[2] = GetBigRoom(x + 1, y); // right
                neighborRoom[3] = GetBigRoom(x + 1, y + 1);
                neighborRoom[4] = GetBigRoom(x, y + 1); // below
                neighborRoom[5] = GetBigRoom(x - 1, y + 1);
                neighborRoom[6] = GetBigRoom(x - 1, y); // left
                neighborRoom[7] = GetBigRoom(x - 1, y - 1);

                bool solid = neighborRoom[7] > 0;
                int tally = 0;
                foreach (int n in neighborRoom)
                {
                    bool s = n > 0;

                    if (s != solid) tally++;
                    solid = s;
                }
                // If there are 2 or less neighboring rooms, punch a hole
                if (tally <= 2) SetBigRoom(x, y, 0);
            }
        }
    }
}
