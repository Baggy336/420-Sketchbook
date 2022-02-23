using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringTarget : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 target;
    private Vector3 target2;

    private int cooldown = 0;

    private void Update()
    {
        if (--cooldown <= 0)
        {
            target = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
            cooldown = (int)Random.Range(20, 60);
        }

        target2.x = Mathf.Lerp(target.x, target2.x, .05f);
        target2.y = Mathf.Lerp(target.y, target2.y, .05f);
        target2.z = Mathf.Lerp(target.z, target2.z, .05f);

        pos.x = Mathf.Lerp(target2.x, pos.x, .03f);
        pos.y = Mathf.Lerp(target2.y, pos.y, .03f);
        pos.z = Mathf.Lerp(target2.z, pos.z, .03f);
    }
}
