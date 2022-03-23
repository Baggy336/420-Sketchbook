using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonController : MonoBehaviour
{
    public Transform moveTarget;

    private List<Pathfinding.Node> pathToTarget = new List<Pathfinding.Node>();

    private bool shouldCheck = true;

    private float checkCooldown = 0;

    private LineRenderer line;

    void Start()
    {
        if (shouldCheck) FindPath();

        line = GetComponent<LineRenderer>();
    }
    void Update()
    {
        checkCooldown -= Time.deltaTime;
        if (checkCooldown <= 0)
        {
            shouldCheck = true;
            checkCooldown = 1;
        }

        FindPath();
        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        if (pathToTarget == null) return;
        if (pathToTarget.Count < 2) return;

        // Get the first node and move to it
        Vector3 target = pathToTarget[1].pos;
        target.y += 1;
        transform.position = AnimMath.Lerp(transform.position, target, .03f);


        float d = (target - transform.position).magnitude;
        if (d < .25f)
        {
            shouldCheck = true;

        }

        // Recalc the path

    }

    private void FindPath()
    {
        shouldCheck = false;

        if (moveTarget && GridController.singleton)
        {
            Pathfinding.Node start = GridController.singleton.Lookup(transform.position);
            Pathfinding.Node end = GridController.singleton.Lookup(moveTarget.position);

            if (start == null || end == null || start == end)
            {
                // clear the array so that it cannot be used
                pathToTarget.Clear();

                return;
            }

            pathToTarget = Pathfinding.Solve(start, end);

            Vector3[] positions = new Vector3[pathToTarget.Count];

            for (int i = 0; i < pathToTarget.Count; i++)
            {
                positions[i] = pathToTarget[i].pos + new Vector3(0, .5f, 0);
            }
            line.positionCount = positions.Length;
            line.SetPositions(positions);

            // Use the path for movement
        }
    }
}
