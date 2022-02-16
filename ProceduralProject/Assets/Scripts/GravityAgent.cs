using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAgent : MonoBehaviour
{
    static float G = 1;
    static float MAXFORCE = 5;
    static List<GravityAgent> agents = new List<GravityAgent>();
    static void FindGravForce(GravityAgent a, GravityAgent b)
    {
        if (a == b) return;

        // if either value has calculated gravity already
        if (a.isDone) return;
        if (b.isDone) return;

        Vector3 vToB = b.pos - a.pos;
        float gravity = G * (a.mass * b.mass) / vToB.sqrMagnitude;

        if (gravity > MAXFORCE) gravity = MAXFORCE;

        vToB.Normalize();

        a.AddForce(vToB * gravity);
        b.AddForce(-vToB * gravity);
    }

    Vector3 pos;
    Vector3 force;
    Vector3 vel;

    float mass;
    bool isDone = false;

    void Start()
    {
        pos = transform.position;
        mass = Random.Range(100, 1000);

        agents.Add(this);
    }
    private void OnDestroy()
    {
        agents.Remove(this);
    }
    public void AddForce(Vector3 f)
    {
        // does not happen across frames, no dt
        force += f;
    }
    void Update()
    {
        // calc grav to all other agents
        foreach(GravityAgent a in agents)
        {
            FindGravForce(this, a);
        }
        isDone = true;

        // euler physics
        Vector3 acceleration = force / mass;

        vel += acceleration * Time.deltaTime;
        pos += vel * Time.deltaTime;

        transform.position = pos;
    }
    private void LateUpdate()
    {
        // resets every object after the frame to false
        isDone = false;
        force *= 0;
    }
}
