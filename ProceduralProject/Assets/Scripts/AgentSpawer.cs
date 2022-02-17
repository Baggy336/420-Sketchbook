using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawer : MonoBehaviour
{
    public GameObject[] spheres;

    Vector3 pos;

    [Range(5, 20)]
    public int numOfSpheres = 10;

    private void Start()
    {
        for (int i = 0; i < numOfSpheres; i++)
        {
            SpawnSpheres();
        }
    }

    private void SpawnSpheres()
    {
        int range = Random.Range(0, spheres.Length);

        pos.x = Random.Range(0, 50);
        pos.y = Random.Range(0, 50);
        pos.z = Random.Range(0, 50);

        Instantiate(spheres[range], pos, transform.rotation);
    }
}
