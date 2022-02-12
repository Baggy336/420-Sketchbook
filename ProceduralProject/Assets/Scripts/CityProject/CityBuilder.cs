using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuilder : MonoBehaviour
{
    public GameObject[] buildings;

    [Range(10, 20)]
    public int width = 30;

    [Range(10, 20)]
    public int height = 30;

    [Range(5, 30)]
    public float density = 20;

    [Range(0, 100000)]
    public int seed;

    int buildingSpace = 1;

    private void OnValidate()
    {

        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                int noiseRes = (int)(Mathf.PerlinNoise(w/density + seed, h/density + seed) * 10);
                Vector3 pos = new Vector3(w * buildingSpace, 0, h * buildingSpace);
                if (noiseRes < 2) Instantiate(buildings[0], pos, Quaternion.identity);
                else if (noiseRes < 4) Instantiate(buildings[1], pos, Quaternion.identity);
                else if (noiseRes < 6) Instantiate(buildings[2], pos, Quaternion.identity);
                else if (noiseRes < 8) Instantiate(buildings[3], pos, Quaternion.identity);
                else if (noiseRes < 10) Instantiate(buildings[4], pos, Quaternion.identity);
            }
        }
    }
}
