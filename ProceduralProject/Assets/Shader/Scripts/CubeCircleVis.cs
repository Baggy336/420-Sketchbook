using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCircleVis : MonoBehaviour
{
    public GameObject cube;
    GameObject[] cubes = new GameObject[512];
    public float scale;

    private void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject sample = (GameObject)Instantiate(cube);
            sample.transform.position = transform.position;
            sample.transform.parent = transform;
            transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            sample.transform.position = Vector3.forward * 100;
            cubes[i] = sample;
        }
    }

    private void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (cubes != null)
            {
                cubes[i].transform.localScale = new Vector3(10, (MusicData.data.freqBands[i] * scale) + 2, 10);
            }
        }
    }
}
