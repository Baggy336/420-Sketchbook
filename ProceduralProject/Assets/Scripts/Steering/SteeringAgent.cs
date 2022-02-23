using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 force;
    private Vector3 vel;
    public Vector3 target;

    public SteeringTarget steeringTarget;

    [Range(.5f, 10)]
    public float mass = 1;

    [Range(5, 50)]
    public float maxSpeed = 10;

    public float maxForce = 10;
    private float targetAngle = 0;
    private float targetRadius = 100;

    private void Start()
    {
        pos = new Vector3(Random.Range(-10 , 10), Random.Range(-10, 10), Random.Range(-10, 10));
        maxSpeed = Random.Range(5, 50);
        mass = Random.Range(.5f, 10);
        maxForce = Random.Range(5, 15);
        targetAngle = Random.Range(-Mathf.PI, Mathf.PI);
        targetRadius = Random.Range(50, 150);

        transform.position = pos;
    }
    private void Update()
    {
        pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        target = new Vector3(steeringTarget.transform.position.x, steeringTarget.transform.position.y, steeringTarget.transform.position.z);

        target.x += (targetRadius * Mathf.Cos(targetAngle));
        target.z += (targetRadius * Mathf.Sin(targetAngle));
        target.y += (targetRadius * Mathf.Tan(targetAngle));

        Steer();
        Euler();

        transform.position = Vector3.Lerp(pos, target, .05f);
    }

    void Euler()
    {
        Vector3 acceleration = force / mass;
        acceleration += vel;
        pos += vel;
        force *= 0;
    }

    void Steer()
    {
        Vector3 targetVel = target - pos;
        targetVel.Normalize();
        targetVel *= maxSpeed;


        Vector3 steering = targetVel - vel;
        steering.x = Mathf.Clamp(steering.x, 0, maxForce);
        steering.y = Mathf.Clamp(steering.y, 0, maxForce);
        steering.z = Mathf.Clamp(steering.z, 0, maxForce);

        force += steering;
    }
}
