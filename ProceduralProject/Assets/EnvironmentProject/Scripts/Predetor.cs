using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predetor : MonoBehaviour
{
    private BasicAgent target;

    private bool isHungry = false;

    public float hungerCooldown;
    private float disToTarget;
    public float newSpotCooldown;

    Vector3 newPos;

    void Start()
    {
        newPos = new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-9.5f, 9.5f), Random.Range(-9.5f, 9.5f));
        hungerCooldown = Random.Range(10f, 40f);
        newSpotCooldown = Random.Range(5f, 15f);
    }


    void Update()
    {
        newSpotCooldown -= Time.deltaTime;
        hungerCooldown -= Time.deltaTime;

        if (hungerCooldown <= 0) isHungry = true;

        if (isHungry)
        {
            target = FindClosest(FlockController.instance.agents);
        }
        if (target)
        {
            transform.position = AnimMath.Lerp(transform.position, target.transform.position, .01f);
            
            disToTarget = (target.transform.position - transform.position).magnitude;
            if (disToTarget <= 2)
            {
                Destroy(target.gameObject);
                isHungry = false;
                hungerCooldown = Random.Range(30f, 200f);
            }
        }
        else if (!target && newSpotCooldown <= 0)
        {
            newPos = new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-9.5f, 9.5f), Random.Range(-9.5f, 9.5f));

            newSpotCooldown = Random.Range(5f, 15f);
        }
        if (!target) transform.position = AnimMath.Lerp(transform.position, newPos, .01f);
    }

    private BasicAgent FindClosest(List<BasicAgent> agents)
    {
        BasicAgent closest = null;
        float dis;
        float closestDis = 9999;
        foreach (BasicAgent a in agents)
        {
            if (closest == null) closest = a;
            dis = (a.transform.position - transform.position).magnitude;
            if (dis <= closestDis)
            {
                closestDis = dis;
                closest = a;
            }
        }
        return (closest);
    }
}
