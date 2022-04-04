using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int currentStep = 0;

    public float speed = 20f;

    public Vector3 endPos;

    public List<Vector3> pathList = new List<Vector3>();

    private void Start()
    {
        endPos = pathList[pathList.Count - 1];
        Pathfinding1._instance.FindPath(this.transform.position, endPos);
    }

    private void Update()
    {
        WalkPath();
    }

    private void EmptyList() 
    {
        pathList = null;
    }

    private void WalkPath()
    {
        if (pathList != null)
        {
            Vector3 targetPos = pathList[currentStep];
            if (Vector3.Distance(transform.position, targetPos) > 1f)
            {
                Vector3 moveDir = (targetPos - transform.position).normalized;

                float disBefore = Vector3.Distance(transform.position, targetPos);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentStep++;
                if (currentStep >= pathList.Count)
                {
                    EmptyList();
                }
            }
        }
        else
        {
            EmptyList();
        }
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public void SetPosition(Vector3 targetPos)
    {
        currentStep = 0;
        pathList = Pathfinding1._instance.FindPath(GetPos(), targetPos);

        if (pathList != null && pathList.Count > 1)
        {
            pathList.RemoveAt(0);
        }

    }
}
