using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnits : MonoBehaviour
{
    private float unitSpawnCooldown = 0;

    public Transform unitToSpawn;

    private void Update()
    {
        unitSpawnCooldown -= Time.deltaTime;

        if (unitSpawnCooldown <= 0)
        {
            Instantiate(unitToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
            unitSpawnCooldown = Random.Range(3, 7);
        }
    }
}
