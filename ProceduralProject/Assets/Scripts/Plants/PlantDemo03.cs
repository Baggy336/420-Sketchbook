using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;


public enum BranchType
{
    Random,
    Opposite,
    Alternate180,
    Alternate1375,
    WhorledTwo,
    WhorledThree
}

public class InstanceCollection {
    public List<CombineInstance> branchInstances = new List<CombineInstance>();
    public List<CombineInstance> leafInstances = new List<CombineInstance>();

    public void AddBranch(Mesh mesh, Matrix4x4 xform)
    {
        branchInstances.Add(new CombineInstance() { mesh = mesh, transform = xform });
    }
    public void AddLeaf(Mesh mesh, Matrix4x4 xform)
    {
        leafInstances.Add(new CombineInstance() { mesh = mesh, transform = xform });
    }
    public Mesh MakeMultiMesh()
    {
        Mesh branchMesh = new Mesh();
        branchMesh.indexFormat = IndexFormat.UInt32;
        branchMesh.CombineMeshes(branchInstances.ToArray());

        Mesh leafMesh = new Mesh();
        leafMesh.indexFormat = IndexFormat.UInt32;
        leafMesh.CombineMeshes(leafInstances.ToArray());

        Mesh finalMesh = new Mesh();
        finalMesh.indexFormat = IndexFormat.UInt32;
        finalMesh.CombineMeshes(new CombineInstance[] {
            new CombineInstance(){ mesh = branchMesh, transform = Matrix4x4.identity },
            new CombineInstance(){ mesh = leafMesh, transform = Matrix4x4.identity }

        }, false);
        return finalMesh;
    }

}


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PlantDemo03 : MonoBehaviour
{
    [Range(0, 100000000)]
    public int seed;

    [Range(2, 30)]
    public int iterations = 5;

    [Range(0, 45)]
    public float turnDegrees = 10;

    [Range(0, 45)]
    public float twistDegrees = 10;

    [Range(0, 1)]
    public float alignWithParent;

    [Range(1, 10)]
    public int branchNodeDis = 2;

    [Range(0, 10)]
    public int branchNodeTrunk = 1;

    public BranchType branchType;

    private System.Random randGen;
    private float Rand()
    {
        return (float)randGen.NextDouble();
    }
    private float RandRange(float min, float max)
    {
        return Rand() * (max - min) + min;
    }
    private float RandBell(float min, float max)
    {
        // Works similar to 2 d6 
        min /= 2;
        max /= 2;

        return RandRange(min, max) + RandRange(min, max);
    }
    private bool RandBool()
    {
        return (Rand() >= .5f);
    }

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
        randGen = new System.Random(seed);

        // Create a list of combine instance objects, create storage
        InstanceCollection instances = new InstanceCollection();

        // Spawn the instances
        Generate(instances, Vector3.zero, Quaternion.identity, new Vector3(.25f, 1, .25f), iterations);


        // Send the stored instances to an array
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter) meshFilter.mesh = instances.MakeMultiMesh();
    }

    void Generate(InstanceCollection instances, Vector3 pos, Quaternion rot, Vector3 scale, int max, int num = 0, float nodeSpin = 0)
    {
        if (num < 0) num = 0;
        if (num >= max) return;

        // Make a cube, add it to the list
        Matrix4x4 xform = Matrix4x4.TRS(pos, rot, scale);
        instances.AddBranch(MeshTools.MakeCube(), xform);

        // Call this function again after adding to the num
        float percentAtEnd = ++num / (float) max;

        Vector3 endPoint = xform.MultiplyPoint(new Vector3(0, 1, 0));

        // If the endpoint is too close, stop recursing
        if ((pos - endPoint).sqrMagnitude < .1f) return;


        bool hasNode = num >= branchNodeTrunk && ((num - branchNodeTrunk - 1) % branchNodeDis == 0);

        if (hasNode)
        {
            if (branchType == BranchType.Alternate180) nodeSpin += 180;
            if (branchType == BranchType.Alternate1375) nodeSpin += 137.5f;

        }

        { // temporary scope

            Quaternion randRot = rot * Quaternion.Euler(turnDegrees, twistDegrees, 0);
            Quaternion upRot = Quaternion.RotateTowards(rot, Quaternion.identity, 45);
            Quaternion newRot = Quaternion.Lerp(randRot, upRot, percentAtEnd);

            Generate(instances, endPoint, newRot, scale * .9f, max, num, nodeSpin);
        }

        if (hasNode)
        {
            // If we are on an odd number
           
            int nodeNum = 0;
            float degreesBetweenNode = 0;

            switch (branchType)
            {
                case BranchType.Random:
                    nodeNum = 1;
                    break;
                case BranchType.Opposite:
                    degreesBetweenNode = 180;
                    nodeNum = 2;
                    break;
                case BranchType.Alternate180:
                    nodeNum = 1;
                    break;
                case BranchType.Alternate1375:
                    nodeNum = 1;
                    break;
                case BranchType.WhorledTwo:
                    degreesBetweenNode = 180;
                    nodeNum = 2;
                    break;
                case BranchType.WhorledThree:
                    degreesBetweenNode = 120;
                    nodeNum = 3;
                    break;
            }

            float lean = Mathf.Lerp(90, 0, alignWithParent);

            for(int i = 0; i < nodeNum; i++)
            {
                float spin = nodeSpin + degreesBetweenNode * i;

                Quaternion newRot = rot * Quaternion.Euler(lean, spin, 0);

                float s = RandBell(.5f, .95f);

                Generate(instances, endPoint, newRot, scale * s, max, num, 90);
            }

            
        }
    }
}
