using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawn : MonoBehaviour
{
    public AgentResource resource;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Instantiate(resource, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)), Quaternion.identity);
        }
    }
}
