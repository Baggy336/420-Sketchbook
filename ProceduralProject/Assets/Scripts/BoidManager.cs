using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use a separate class to predetermine which settings should each boid have
[System.Serializable]
public class BoidSettings
{
    public BoidType type;

    public Boid prefab;
    public float maxSpeed;
    public float maxForce;

    public float radiusAlign;
    public float radiusCohesion;
    public float radiusSeparation;

    public float forceAlign;
    public float forceCohesion;
    public float forceSeparation;

}

public class BoidManager : MonoBehaviour
{
    public BoidSettings[] boidTypes;

    // Singleton reference
    public static BoidManager _instance;

    private List<Boid> boids = new List<Boid>();

    private void Start()
    {
        // If we already have a singleton
        if (_instance != null) Destroy(gameObject);
        else // If we don't have a manager
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        if (boids.Count < 3)
        {
            if (boidTypes.Length > 0)
            {
                Boid b = Instantiate(boidTypes[0].prefab);
                b.type = boidTypes[0].type;
            }
        }

        // Update the boids
        Boid[] bArray = boids.ToArray();

        foreach(Boid b in boids)
        {
            b.CalcForces(bArray);
        }
    }

    public static BoidSettings GetSettings(BoidType type)
    {
        foreach(BoidSettings bs in _instance.boidTypes)
        {
            if (bs.type == type) return bs;
        }
        return new BoidSettings();
    }

    public static void AddBoid(Boid b)
    {
        // Grab the singleton, and add a boid
        _instance.boids.Add(b);
    }
    public static void RemoveBoid(Boid b)
    {
        // Grab the singleton, and remove a boid
        _instance.boids.Remove(b);
    }
}
