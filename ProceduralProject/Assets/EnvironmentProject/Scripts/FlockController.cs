using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public List<BasicAgent> agents = new List<BasicAgent>();

    public BasicAgent agent;

    public static FlockController instance { get; private set; }

    private void Start()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        for (int i = 0; i < 6; i++)
        {
            Instantiate(agent, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)), Quaternion.identity);
        }
    }

    private void Update()
    {
        foreach (BasicAgent a in agents)
        {
            if (!a.target)
            {
                a.CalcForces(agents);
            }
        }
    }
    public static void AddAgent(BasicAgent a)
    {
        instance.agents.Add(a);
    }
}
