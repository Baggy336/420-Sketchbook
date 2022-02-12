using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public RoadController prefabRoad;

    [Range(5, 20)]
    public int citySize = 10;

    [Range(1, 10)]
    public int objectSpacing = 5;

    private void Start()
    {
        CityData city = new CityData();
        city.Generate(citySize);

        int[,] roads = city.GetRoads();

        for (int x = 0; x < roads.GetLength(0); x++)
        {
            for (int y = 0; y < roads.GetLength(1); y++)
            {
                if (roads[x, y] == 0) continue;

                Vector3 pos = new Vector3(x, 0, y) * objectSpacing;

                RoadController newRoom = Instantiate(prefabRoad, pos, Quaternion.identity);

                newRoom.SpawnRoad((BuildingType)roads[x, y]);

            }
        }
    }
}
