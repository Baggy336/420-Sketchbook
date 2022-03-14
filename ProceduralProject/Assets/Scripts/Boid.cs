using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoidType
{
    small,
    large
}

[RequireComponent(typeof(Rigidbody))]
public class Boid : MonoBehaviour
{
    public BoidType type;

    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        BoidManager.AddBoid(this);
    }
    void LateUpdate()
    {
        Vector3 dir = body.velocity;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void OnDestroy()
    {
        BoidManager.RemoveBoid(this);
    }

    public void CalcForces(Boid[] boids)
    {
        BoidSettings settings = BoidManager.GetSettings(type);

        int countCohesion = 0;
        Vector3 avgCenter = Vector3.zero;

        // determine alignment, cohesion, and separation forces
        foreach(Boid b in boids)
        {
            // Don't calculate against itself
            if (b == this) continue;

            Vector3 vBetween = b.transform.position - transform.position;

            // Get how long the vector is
            float dis = vBetween.magnitude;       
            
            if (dis < settings.radiusAlign)
            {

            }
            if (dis < settings.radiusCohesion)
            {
                avgCenter += b.transform.position;
                countCohesion++;
            }
            if (dis < settings.radiusSeparation)
            {
                // Normalize the vector and flip it
                Vector3 separation = settings.forceSeparation * -vBetween / dis / dis * Time.deltaTime;

                body.AddForce(separation);
            }
        }
        // Apply forces
        if (countCohesion > 0)
        {
            avgCenter /= countCohesion;

            // Vector to the center of the group
            Vector3 vToCenter = avgCenter - transform.position;

            // The unit vector scaled by speed
            Vector3 desiredVel = vToCenter.normalized * settings.maxSpeed;

            // Cohesion is the desired velocity - the current rigidbody vel
            Vector3 cohesionVec = desiredVel - body.velocity;

            if(cohesionVec.sqrMagnitude > settings.maxForce * settings.maxForce)
            {
                cohesionVec = cohesionVec.normalized * settings.maxForce;
            }
            // Apply the calculated force, scaled by the cohesion force and delta time
            body.AddForce(cohesionVec * settings.forceCohesion * Time.deltaTime);
        }
    }
}
