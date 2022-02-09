
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Plant02 : MonoBehaviour
{
    public int iterations = 4;

    public int sproutDegrees = 20;
    private void Start()
    {
        Build();
    }

    private void OnValidate()
    {
        Build();
    }

    private void Build()
    {
        // create a storage for combined instance meshes
        List<CombineInstance> instances = new List<CombineInstance>();

        // Spawn the combined instances
        Generate(instances, Vector3.zero, Quaternion.identity, new Vector3(.25f, .75f, .25f), iterations, 0);

        // Store the spawned combined instances in an array
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(instances.ToArray());

        // Get the mesh filter component required by script
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        // If there is a meshfilter, set it to the mesh
        if (meshFilter) meshFilter.mesh = mesh;
    }

    void Generate(List<CombineInstance> instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num)
    {
        // if num is less than 0, num is 0
        if (num < 0) num = 0;
        // if num is larger than maximum iterations, stop iterating
        if (num >= max) return;

        // Make a cube, add it to the list
        CombineInstance inst = new CombineInstance();
        inst.mesh = MeshTools.MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, scale);
        instances.Add(inst);

        // Call this function again after adding to the num
        float percentAtEnd = ++num / (float)max;

        Vector3 endPos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        // If the endpoint is too close, stop recursing
        if ((pos - endPos).sqrMagnitude < .1f) return;

        Quaternion randRot = rot * Quaternion.Euler(sproutDegrees, Random.Range(-180, 180f), 0);

        Generate(instances, endPos, randRot, scale * .6f, max, num);

        if (num > 1)
        {
            if (num % 2 == 1)
            {
                float degrees = Random.Range(-1, 2) * 90;

                Quaternion newRot = Quaternion.LookRotation(endPos - pos) * Quaternion.Euler(0, 0, degrees);

                Generate(instances,
                    endPos,
                    newRot,
                    scale * .9f,
                    max,
                    num);
            }
        }
    }
}
