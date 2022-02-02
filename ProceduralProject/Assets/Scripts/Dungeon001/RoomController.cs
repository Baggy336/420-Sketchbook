using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public Transform[] wallPrefabs;
    public float roomSize = 5;

    public void InitRoom(RoomType type)
    {
        // Spawn walls
        // Pick a random wall type
        Transform prefabN = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        Transform prefabS = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        Transform prefabE = wallPrefabs[Random.Range(0, wallPrefabs.Length)];
        Transform prefabW = wallPrefabs[Random.Range(0, wallPrefabs.Length)];

        // Pick positions
        float dis = roomSize / 2 - .25f;

        Vector3 posN = new Vector3(0, 0, +dis) + this.transform.position;
        Vector3 posS = new Vector3(0, 0, -dis) + this.transform.position;
        Vector3 posE = new Vector3(+dis, 0, 0) + this.transform.position;
        Vector3 posW = new Vector3(-dis, 0, 0) + this.transform.position;


        Quaternion rotN = Quaternion.Euler(-90, ( (Random.Range(0, 100) < 50) ? 0 : 180), 0);
        Quaternion rotS = Quaternion.Euler(-90, ( (Random.Range(0, 100) < 50) ? 0 : 180), 0);
        Quaternion rotE = Quaternion.Euler(-90, ( (Random.Range(0, 100) < 50) ? -90 : 90), 0);
        Quaternion rotW = Quaternion.Euler(-90, ( (Random.Range(0, 100) < 50) ? -90 : 90), 0);

        Instantiate(prefabN, posN, rotN, transform);
        Instantiate(prefabS, posS, rotS, transform);
        Instantiate(prefabE, posE, rotE, transform);
        Instantiate(prefabW, posW, rotW, transform);

    }
}
