using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentResource : MonoBehaviour
{
    private int maxResource = 4;

    public int resource = 4;

    private float cooldown = 0;


    private void Update()
    {
        if (resource < maxResource)
        {
            cooldown -= Time.deltaTime;

            if (cooldown <= 0 && resource < maxResource)
            {
                resource++;

                cooldown = Random.Range(60, 90);
            }
        }
    }

}
