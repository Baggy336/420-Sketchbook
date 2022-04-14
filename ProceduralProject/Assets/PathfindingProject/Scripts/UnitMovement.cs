using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public int currentStep = 0;

    public Vector3 endPos;

    public float calcCooldown = 0;

    public List<Pathfinder.Square> pathList = new List<Pathfinder.Square>();

    private void Start()
    {       
        endPos = new Vector3(18, 0, 18);
        CalcPath();
    }

    private void Update()
    {
        calcCooldown -= Time.deltaTime;

        if (calcCooldown <= 0) CalcPath();
        WalkPath();
    }

    private void EmptyList() 
    {
        pathList = null;
    }

    private void CalcPath()
    {

            Pathfinder.Square start = GenerateMap.instance.LookupTile(transform.position);
            Pathfinder.Square end = GenerateMap.instance.LookupTile(endPos);

            if (start == null || end == null || start == end)
            {
                pathList.Clear();

                return;
            }

            pathList = Pathfinder.FindPath(start, end);
            Vector3[] steps = new Vector3[pathList.Count];

            for (int i = 0; i < pathList.Count; i++)
            {
                steps[i] = pathList[i].pos;
            }
        
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public void WalkPath()
    {
        if (pathList == null) return;
        if (pathList.Count < 2) return;

        Vector3 target = pathList[1].pos;
        transform.position = AnimMath.Lerp(transform.position, target, .005f);

        float dis = (target - transform.position).magnitude;
        if (dis < .2f)
        {
            calcCooldown = 3;
        }
    }
}
