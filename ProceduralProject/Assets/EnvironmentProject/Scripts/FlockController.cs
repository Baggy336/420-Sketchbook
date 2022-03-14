using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public List<BasicAgent> agents = new List<BasicAgent>();

    [Range(0.5f, 2)]
    public float mass = 1;

    [Range(5, 30)]
    public float maxSpeed = 10;

    private float maxForce;
    private float speed = 10;
    public float cohesionRad = 50;
    public float alignRad = 40;
    public float separationRad = 10;
    public float cohesionForce = 1;
    public float alignmentForce = .5f;
    public float separationForce = 2;

    public Vector3 vel;
    private Vector3 force;

    private void Start()
    {
        agents.Add(GetComponent<BasicAgent>());

        transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        maxSpeed = Random.Range(5, 50);
        mass = Random.Range(.5f, 10);
        maxForce = Random.Range(5, 15);
    }

    private void Update()
    {
        CalculateFlock();
        EulerStep();
    }

    // Euler physics integration
    void EulerStep()
    {
        Vector3 acceleration = force / mass;
        acceleration += vel;
        transform.position += acceleration;
        force *= 0;
        Debug.Log(acceleration);
    }

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
                avgAlign += vel;

                numAlign++;
            }
        }

        if (numCohesion > 0)
        {
            groupCenter /= numCohesion;

            Vector3 dirToCenter = groupCenter - transform.position;
            dirToCenter *= speed;

            Vector3 forceCohesion = dirToCenter - vel;

            Vector3.ClampMagnitude(forceCohesion, cohesionForce);

            force += forceCohesion;
        }
        if (numAlign > 0)
        {
            avgAlign /= numAlign;
            avgAlign *= speed;

            Vector3 alignForce = avgAlign - vel;

            Vector3.ClampMagnitude(alignForce, alignmentForce);

            force += alignForce;
        }
    }
}
