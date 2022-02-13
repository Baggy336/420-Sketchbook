using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitySpawner: MonoBehaviour
{
    public GameObject prefabRoad;
    public GameObject[] prefabBuilding;
    public GameObject prefabPlant;

    [Range(5, 50)]
    public int citySize = 10;

    [Range(1, 10)]
    public int objectSpacing = 5;

    [Range(5, 15)]
    public int buildingSpacing = 10;

    [Range(1, 20)]
    public int plantSpacing = 5;

    private void Start()
    {
        CityData city = new CityData();
        city.Generate(citySize);

        int[,] roads = city.GetRoads();
        int[,] buildings = city.GetBuildings();
        int[,] plants = city.GetPlants();

        for (int x = 0; x < roads.GetLength(0); x++)
        {
            for (int y = 0; y < roads.GetLength(1); y++)
            {
                if (roads[x, y] == 0) continue;

                Vector3 pos = new Vector3(x, 0, y) * objectSpacing;

                Instantiate(prefabRoad, pos, Quaternion.identity);
            }
        }
        for (int x = 0; x < buildings.GetLength(0); x++)
        {
            for (int y = 0; y < buildings.GetLength(1); y++)
            {
                if (buildings[x, y] == 0) continue;

                Vector3 pos = new Vector3(x * buildingSpacing, 2, y * buildingSpacing);

                Instantiate(prefabBuilding[Random.Range(0, prefabBuilding.Length)], pos, Quaternion.identity);
            }
        }

        for (int x = 0; x < plants.GetLength(0); x++)
        {
            for (int y = 0; y < plants.GetLength(1); y++)
            {
                if (plants[x, y] == 0) continue;

                Vector3 pos = new Vector3(x * plantSpacing, 0, y * plantSpacing);

                Instantiate(prefabPlant, pos, Quaternion.identity);
            }
        }
    }
}
