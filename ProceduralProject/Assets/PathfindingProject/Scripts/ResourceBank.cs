using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBank : MonoBehaviour
{
    public static ResourceBank _instance;
    public static ResourceBank instance { get; private set; }

    public int resources = 30;

    private float resourceGain = 0.25f;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        resourceGain -= Time.deltaTime;
        if (resourceGain <= 0)
        {
            resources++;
            resourceGain = Random.Range(0.25f, .75f);
        }
    }

    public void PayCost(int x)
    {
        resources -= x;
    }
}
