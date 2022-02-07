using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlantDemo01 : MonoBehaviour
{
    [Range(2, 30)]
    public int iterations = 5;

    [Range(5, 30)]
    public float fanDegrees = 10;
    private void Start()
    {
        Build();
    }
    private void OnValidate()
    {
        Build();
    }
    void Build()
    {
        // Create a list of combine instance objects, create storage
        List<CombineInstance> instances = new List<CombineInstance>();

        // Spawn the instances
        Generate(instances, Vector3.zero, Quaternion.identity, new Vector3(.25f, 1, .25f), iterations);


        // Send the stored instances to an array
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(instances.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter) meshFilter.mesh = mesh;
    }

    void Generate(List<CombineInstance> instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0)
    {
        if (num < 0) num = 0;
        if (num >= max) return;

        // Make a cube, add it to the list
        CombineInstance inst = new CombineInstance();
        inst.mesh = MeshTools.MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, scale);
        instances.Add(inst);

        // Call this function again after adding to the num
        float percentAtEnd = ++num / (float) max;

        Vector3 endPoint = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        // If the endpoint is too close, stop recursing
        if ((pos - endPoint).sqrMagnitude < .1f) return;

        { // temporary scope

            Quaternion randRot = rot * Quaternion.Euler(fanDegrees, Random.Range(-90, 90f), 0);
            Quaternion upRot = Quaternion.RotateTowards(rot, Quaternion.identity, 45);
            Quaternion newRot = Quaternion.Lerp(randRot, upRot, .5f);

            Generate(instances,
                endPoint,
                newRot,
                scale * .9f,
                max,
                num);
        }

        if (num > 1)
        {
            if (num % 2 == 1)
            {
                float degrees = Random.Range(-1, 2) * 90;

                Quaternion newRot = Quaternion.LookRotation(endPoint - pos) * Quaternion.Euler(0, 0, degrees);

                Generate(instances,
                    endPoint,
                    newRot,
                    scale * .9f,
                    max,
                    num);
            }           
        }
    }
}
