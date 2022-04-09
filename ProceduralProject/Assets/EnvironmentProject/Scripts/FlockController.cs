using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public List<BasicAgent> agents = new List<BasicAgent>();

    public List<Predetor> predetors = new List<Predetor>();

    public BasicAgent agent;
    public Predetor predetor;

    public static FlockController instance { get; private set; }

    private void Start()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        for (int i = 0; i < 10; i++)
        {
            Instantiate(agent, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)), Quaternion.identity);
        }
        for (int p = 0; p < 2; p++)
        {
            Instantiate(predetor, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)), Quaternion.identity);
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

    public List<BasicAgent> GetAgents()
    {
        return agents;
    }

    public static void AddAgent(BasicAgent a)
    {
        instance.agents.Add(a);
    }

    public static void RemoveAgent(BasicAgent a)
    {
        instance.agents.Remove(a);
    }
    public static void AddPredetor(Predetor p)
    {
        instance.predetors.Add(p);
    }
    public static void RemovePredetor(Predetor p)
    {
        instance.predetors.Remove(p);
    }
}
