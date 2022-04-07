
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicAgent : MonoBehaviour
{
    private List<AgentResource> resource = new List<AgentResource>();

    private Rigidbody body;

    public Transform target;
    public Transform theNest;

    [Range(0.5f, 10)]
    public float mass = 1;

    [Range(5, 30)]
    public float maxSpeed = 10;

    public float visDis = 100;
    public float visAngle = 180;
    public float cohesionRad = 100;
    public float alignRad = 40;
    public float separationRad = 10;
    public float cohesionForce = 5;
    public float alignmentForce = .5f;
    public float separationForce = 2;

    private bool hasResource = false;
    private bool wantsResource = true;

    public float cooldownScan;
    public float cooldownSelect;
    public float cooldownWantResource;
    private float harvestTime = 10;
    private float disToTarget;
    private float maxForce;
    private float speed = 10;
    
    private int resourceStorage = 0;

    public Vector3 vel;
    private Vector3 force;

    private void Start()
    {
        FlockController.AddAgent(this);

        maxSpeed = Random.Range(5, 50);
        mass = Random.Range(.5f, 10);
        maxForce = Random.Range(5, 15);

        body = GetComponent<Rigidbody>();

        cooldownWantResource = Random.Range(20, 70);

        if (GameObject.FindWithTag("Nest") != null)
        {
            theNest = GameObject.FindWithTag("Nest").transform;
        }
    }

    void Update()
    {
        // Count down each cooldown 
        cooldownWantResource -= Time.deltaTime;
        if (cooldownWantResource <= 0) cooldownScan -= Time.deltaTime;
        if (cooldownScan <= 0) cooldownSelect -= Time.deltaTime;

        if (target) disToTarget = (target.transform.position - transform.position).sqrMagnitude;

        // if the agent wants a resource and does not have one
        if (wantsResource && !hasResource && cooldownWantResource <= 0)
        {
            if (cooldownScan <= 0 && !target) FindResource();
            if (cooldownSelect <= 0 && !target) PickResource();

            if (target)
            {
                transform.position = AnimMath.Lerp(transform.position, target.position, .25f);
            }

            if (disToTarget <= 2)
            {
                HarvestResource();
            }
        }
        else if (!wantsResource && !hasResource) // If the agent doesn't have a resource and doesn't want one
        {
            // No target, stay with the pack
            EulerStep();
        }
        else if (hasResource)
        {
            // Fly to the nest and deposit
            if (target)
            {
                transform.position = AnimMath.Lerp(transform.position, target.position, .25f);
            }

            if (disToTarget <= 2)
            {
                DepositResource();
            }
        }
    }

    // Euler physics integration
    void EulerStep()
    {
        Vector3 acceleration = force / mass;
        acceleration += vel;
        transform.position += acceleration;
        force *= 0;
    }

    // Check the vision distance and angle compared to the resource to make sure it can be seen
    private bool FindableResource(Transform resources)
    {
        // If there is no resource in the scene
        if (!resources) return false;

        Vector3 vToResource = resources.position - transform.position;

        if (vToResource.sqrMagnitude > visDis * visDis) return false;

        if (Vector3.Angle(transform.forward, vToResource) > visAngle) return false;

        return true;
    }

    // Find the resources in the scene and store them in an array
    void FindResource()
    {
        cooldownScan = 1;

        resource.Clear();
        AgentResource[] resources = GameObject.FindObjectsOfType<AgentResource>();

        foreach(AgentResource thing in resources)
        {
            if (FindableResource(thing.transform))
            {
                resource.Add(thing);
            }
        }
    }

    // Make the target the closest resource to the agent
    void PickResource()
    {
        cooldownSelect = 3;

        target = null;

        float closestDis = 0;

        // Loop through each resource seen and select the closest on the the agent
        foreach(AgentResource ar in resource)
        {
            float disToTarget = (ar.transform.position - transform.position).sqrMagnitude;

            if (disToTarget < closestDis || target == null)
            {
                target = ar.transform;
                closestDis = disToTarget;
            }
        } 
    }

    // Harvest the resource if the agent is close enough to it
    void HarvestResource()
    {
        harvestTime -= Time.deltaTime;

        if (harvestTime <= 0)
        {
            resourceStorage++;
            wantsResource = false;
            hasResource = true;

            var temp = target.GetComponent<AgentResource>();
            temp.resource--;

            harvestTime = 10;

            // Set the target to the nest
            target = theNest;
        }
    }

    // Deposit the resource into the bank.
    void DepositResource()
    {
        harvestTime -= Time.deltaTime;

        if (harvestTime <= 0)
        {
            resourceStorage--;
            hasResource = false;
            wantsResource = true;

            harvestTime = 10;

            Bank.AddResource(1);

            cooldownWantResource = Random.Range(20, 70);

            target = null;
        }
    }

    public void CalcForces(List<BasicAgent> agents)
    {
        int countCohesion = 0;
        Vector3 avgCenter = Vector3.zero;

        // determine alignment, cohesion, and separation forces
        foreach (BasicAgent b in agents)
        {
            // Don't calculate against itself
            if (b == this) continue;

            Vector3 vBetween = b.transform.position - transform.position;

            // Get how long the vector is
            float dis = vBetween.magnitude;

            if (dis < alignRad)
            {

            }
            if (dis < cohesionRad)
            {
                avgCenter += b.transform.position;
                countCohesion++;
            }
            if (dis < separationRad)
            {
                // Normalize the vector and flip it
                Vector3 separation = separationForce * -vBetween / dis / dis * Time.deltaTime;

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
            Vector3 desiredVel = vToCenter.normalized * maxSpeed;

            // Cohesion is the desired velocity - the current rigidbody vel
            Vector3 cohesionVec = desiredVel - body.velocity;

            if (cohesionVec.sqrMagnitude > maxForce * maxForce)
            {
                cohesionVec = cohesionVec.normalized * maxForce;
            }
            // Apply the calculated force, scaled by the cohesion force and delta time
            body.AddForce(cohesionVec * cohesionForce * Time.deltaTime);
        }
    }
}
