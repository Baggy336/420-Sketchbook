using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentResource : MonoBehaviour
{
    private int maxResource = 4;

    public int resource = 4;

    private int cooldown = 0;

    private void Update()
    {
        if (resource < maxResource)
        {
            cooldown--;

            if (cooldown <= 0 && resource < maxResource)
            {
                resource++;

                cooldown = (int)Random.Range(60, 90);
            }
        }
    }
}
