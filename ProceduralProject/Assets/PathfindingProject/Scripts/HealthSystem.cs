using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health { get; private set; }

    public float healthMax = 100;

    private void Start()
    {
        health = healthMax;
    }

    private void Update()
    {
        
    }

    public void TakeDamage(float amt)
    {
        if (amt <= 0) return;
        health -= amt;

        if (health <= 0) Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}
