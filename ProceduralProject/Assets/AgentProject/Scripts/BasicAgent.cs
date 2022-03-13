
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAgent : MonoBehaviour
{
    private List<AgentResource> resource = new List<AgentResource>();

    public Transform target;
    public Transform theNest;

    [Range(0.5f, 2)]
    public float mass = 1;

    [Range(5, 30)]
    public float maxSpeed = 10;

    public float visDis = 10;
    public float visAngle = 45;

    private bool hasResource = false;
    private bool wantsResource = true;

    private float cooldownScan;
    private float cooldownSelect;
    private float harvestTime = 10;
    private float disToTarget;
    private float maxForce;

    private int resourceStorage = 0; 

    private Vector3 vel;
    private Vector3 force;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5));
        maxSpeed = Random.Range(5, 50);
        mass = Random.Range(.5f, 10);
        maxForce = Random.Range(5, 15);

        if (GameObject.FindWithTag("Nest") != null)
        {
            theNest = GameObject.FindWithTag("Nest").transform;
        }

        FindResource();
        PickResource();
    }

    void Update()
    {
        // Count down each cooldown
        cooldownScan -= Time.deltaTime;
        cooldownSelect -= Time.deltaTime;

        disToTarget = (target.transform.position - transform.position).sqrMagnitude;

        // if the agent wants a resource and does not have one
        if (wantsResource && !hasResource)
        {
            transform.position = AnimMath.Lerp(transform.position, target.position, 0.5f) * Time.deltaTime;

            if (cooldownScan <= 0 && !target) FindResource();

            if (cooldownSelect <= 0) PickResource();

            if (disToTarget <= 4)
            {
                HarvestResource();
            }
        }
        else if (!wantsResource && !hasResource) // If the agent doesn't have a resource and doesn't want one
        {
            // No target, stay with the pack
        }
        else if (hasResource)
        {
            // Fly to the nest and deposit
            transform.position = AnimMath.Lerp(transform.position, theNest.position, .03f) * Time.deltaTime;

            if (disToTarget <= 4)
            {
                DepositResource();
            }
        }

        EulerStep();
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

        transform.position = transform.position;

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

        transform.position = transform.position;

        if (harvestTime <= 0)
        {
            resourceStorage--;
            wantsResource = true;
            hasResource = false;

            harvestTime = 10;

            Bank.AddResource(1);

            FindResource();
        }
    }
}
