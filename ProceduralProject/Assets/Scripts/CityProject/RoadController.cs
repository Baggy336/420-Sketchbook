using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public Transform[] roadPreFab;
    public float roadSize = 5;

    public void SpawnRoad(BuildingType type)
    {
        Transform preFabRoad = roadPreFab[0];

        // Pick positions
        float dis = roadSize / 2 - .25f;

        Vector3 posN = new Vector3(0, 0, +dis) + this.transform.position;
        Vector3 posS = new Vector3(0, 0, -dis) + this.transform.position;
        Vector3 posE = new Vector3(+dis, 0, 0) + this.transform.position;
        Vector3 posW = new Vector3(-dis, 0, 0) + this.transform.position;

        Instantiate(preFabRoad, posN, Quaternion.identity, transform);
    }
}
