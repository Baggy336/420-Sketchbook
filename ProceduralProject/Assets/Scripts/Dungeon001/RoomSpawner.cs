using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public RoomController prefabRoom;

    [Range(4, 15)]
    public int dungeonSize = 10;

    [Range(1, 10)]
    public int roomSpacing = 5;

    private void Start()
    {
        // spawn a dungeon layout
        GenerateData dungeon = new GenerateData();
        dungeon.Generate(dungeonSize);

        // Get rooms 
        int[,] rooms = dungeon.GetRooms();

        // Loop through rooms
        for(int x = 0; x < rooms.GetLength(0); x++)
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                if (rooms[x, y] == 0) continue;

                Vector3 pos = new Vector3(x, 0, y) * roomSpacing;
                RoomController newRoom = Instantiate(prefabRoom, pos, Quaternion.identity);

                newRoom.InitRoom((RoomType)rooms[x, y]);
            }
        }
    }
}
