using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private float visDis = 8;

    public Projectile prefabBullet;

    public Transform target;

    public List<UnitMovement> units = new List<UnitMovement>();

    private float scanCD = 2;
    private float pickCD = 2;
    private float attackCD = 3f;

    private void Update()
    {
        scanCD -= Time.deltaTime;
        if (scanCD <= 0) ScanForUnits();
        pickCD -= Time.deltaTime;
        if (pickCD <= 0) PickATarget();
        attackCD -= Time.deltaTime;
        if (attackCD <= 0 && target)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Projectile projectile;
        if (target == null) return;

        projectile = Instantiate(prefabBullet, transform.position, Quaternion.identity);
        projectile.target = target;

    }

    private bool CanSeeUnit(Transform u)
    {
        if (!u) return false;

        Vector3 vectorToThing = u.position - transform.position;

        // Check distance
        if (vectorToThing.sqrMagnitude > visDis * visDis) return false; // Too far away to see.

        return true;

    }

    void ScanForUnits()
    {
        scanCD = 1;
        units.Clear();

        UnitMovement[] temp = GameObject.FindObjectsOfType<UnitMovement>();

        foreach (UnitMovement u in temp)
        {
            if (CanSeeUnit(u.transform))
            {
                units.Add(u);
            }
        }
    }

    void PickATarget()
    {
        pickCD = .5f;

        //if (target) return; // We already have a target
        target = null;

        float closestDisSoFar = 0;

        // Loop throught the targets, and find the closest TargetableThing
        foreach (UnitMovement um in units)
        {
            float dd = (um.transform.position - transform.position).sqrMagnitude;

            if (dd < closestDisSoFar || target == null)
            {
                target = um.transform;
                closestDisSoFar = dd;
            }
        }
    }
}
