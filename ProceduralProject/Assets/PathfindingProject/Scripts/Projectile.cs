using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;

    public Vector3 vToTarget;

    float speed = 7;

    float destroyCountdown = 0;

    private void Start()
    {
        destroyCountdown = 2;
        vToTarget = (target.position - transform.position).normalized;
    }

    private void Update()
    {
        transform.position += vToTarget * speed * Time.deltaTime;
        DestroyAfterTime();
    }

    private void OnTriggerEnter(Collider other)
    {
        UnitMovement target = other.GetComponent<UnitMovement>();

        if (target) // Overlapping a targetablething
        {
            HealthSystem targetHealth = target.GetComponent<HealthSystem>();
            if (targetHealth)
            {
                targetHealth.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }

    void DestroyAfterTime()
    {
        destroyCountdown -= Time.deltaTime;
        if (destroyCountdown <= 0) Destroy(gameObject);
    }
}
