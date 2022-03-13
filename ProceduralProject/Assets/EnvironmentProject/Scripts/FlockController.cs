using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public List<BasicAgent> agents = new List<BasicAgent>();

    private Vector3 force;

    private float speed = 10;

    public float cohesionRad = 50;
    public float alignRad = 40;
    public float separationRad = 10;
    public float cohesionForce = 1;
    public float alignmentForce = .5f;
    public float separationForce = 2;

    void CalculateFlock()
    {
        Vector3 groupCenter = new Vector3(0, 0, 0);
        Vector3 avgAlign = new Vector3(0, 0, 0);
        int numCohesion = 0;
        int numAlign = 0;

        foreach(BasicAgent a in agents)
        {
            if (a == this) continue;

            float dis = (a.transform.position - transform.position).sqrMagnitude;

            if (dis < cohesionRad)
            {
                groupCenter += a.transform.position;
                numCohesion++;
            }
            if (dis < separationRad)
            {
                Vector3 push = new Vector3(transform.position.x - dis, transform.position.y - dis, transform.position.z - dis);
                push *= separationForce / dis;

                force += push;
            }
            if (dis < alignRad)
            {
                avgAlign += a.vel;

                numAlign++;
            }
        }

        if (numCohesion > 0)
        {
            groupCenter /= numCohesion;

            Vector3 dirToCenter = groupCenter - transform.position;
            dirToCenter *= speed;

            cohesionForce = dirToCenter - agents.vel;
        }
    }
}
