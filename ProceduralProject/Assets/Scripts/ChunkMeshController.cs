using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))] // Tells the engine to require a meshfilter on any object with this script
public class ChunkMeshController : MonoBehaviour
{
    [Range(4, 40)]
    public int resolution = 10;

    [Range(5, 50)]
    public float zoom = 10;

    [Range(0, 1)]
    public float densityThreshold = 0.5f;

    private MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void OnValidate()
    {
        BuildMesh();
    }

    void BuildMesh()
    {
        // Sample the noise field to determine if blocks are solid
        bool[,,] voxels = new bool[resolution, resolution, resolution];

        for (int x = 0; x < voxels.GetLength(0); x++) {
            for (int y = 0; y < voxels.GetLength(1); y++) {
                for (int z = 0; z < voxels.GetLength(2); z++) {
                    Vector3 pos = new Vector3(x, y, z);
                    float density = Noise.Perlin(pos / zoom);

                    voxels[x, y, z] = (density > densityThreshold);
                }
            }
        }

        // Make storage for geometry data
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        // Generate the geometry

        for (int x = 0; x < voxels.GetLength(0); x++) {
            for (int y = 0; y < voxels.GetLength(1); y++) {
                for (int z = 0; z < voxels.GetLength(2); z++) {
                    if(voxels[x, y, z])
                    {
                        byte sides = 0;

                        if (!Lookup(voxels, x, y + 1, z)) sides |= 01;
                        if (!Lookup(voxels, x, y - 1, z)) sides |= 02;
                        if (!Lookup(voxels, x + 1, y, z)) sides |= 04;
                        if (!Lookup(voxels, x - 1, y, z)) sides |= 08;
                        if (!Lookup(voxels, x, y, z + 1)) sides |= 16;
                        if (!Lookup(voxels, x, y, z - 1)) sides |= 32;

                        AddCube(new Vector3(x, y, z), sides, verts, tris, normals, uvs);
                    }
                }
            }
        }

        // Make mesh from geometry
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        if (!meshFilter) meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    bool Lookup(bool[,,] arr, int x, int y, int z)
    {
        // Spot falls outside of the index bounds
        if (x < 0) return false;
        if (y < 0) return false;
        if (z < 0) return false;
        if (x >= arr.GetLength(0)) return false;
        if (y >= arr.GetLength(1)) return false;
        if (z >= arr.GetLength(2)) return false;


        return arr[x, y, z];
    }

    void AddCube(Vector3 position, 
        byte sides,
        List<Vector3> verts, 
        List<int> tris, 
        List<Vector3> norms, 
        List<Vector2> uvs)
    {
        if((sides & 01) > 0) AddQuad(position + new Vector3(0, +0.5f, 0), Quaternion.Euler(0, 0, 000), verts, tris, norms, uvs);
        if((sides & 02) > 0) AddQuad(position + new Vector3(0, -0.5f, 0), Quaternion.Euler(0, 0, 180), verts, tris, norms, uvs);
        if((sides & 04) > 0) AddQuad(position + new Vector3(+0.5f, 0, 0), Quaternion.Euler(0, 0, -90), verts, tris, norms, uvs);
        if((sides & 08) > 0) AddQuad(position + new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 0, +90), verts, tris, norms, uvs);
        if((sides & 16) > 0) AddQuad(position + new Vector3(0, 0, +0.5f), Quaternion.Euler(+90, 0, 0), verts, tris, norms, uvs);
        if((sides & 32) > 0) AddQuad(position + new Vector3(0, 0, -0.5f), Quaternion.Euler(-90, 0, 0), verts, tris, norms, uvs);
    }

    /// <summary>
    /// This function requires the parameters from the buildmesh function. postion will be the x,y,z center of the quad
    /// </summary>
    /// <param name="verts"></param>
    /// <param name="tris"></param>
    /// <param name="norms"></param>
    /// <param name="uvs"></param>
    private void AddQuad(Vector3 position, 
        Quaternion rotation, 
        List<Vector3> verts, 
        List<int> tris, 
        List<Vector3> norms, 
        List<Vector2> uvs)
    {
        int num = verts.Count;

        verts.Add(position + rotation * new Vector3(0.5f, 0, 0.5f));
        verts.Add(position + rotation * new Vector3(0.5f, 0, -0.5f));
        verts.Add(position + rotation * new Vector3(-0.5f, 0, -0.5f));
        verts.Add(position + rotation * new Vector3(-0.5f, 0, 0.5f));

        tris.Add(num + 0);
        tris.Add(num + 1);
        tris.Add(num + 3);
        tris.Add(num + 1);
        tris.Add(num + 2);
        tris.Add(num + 3);

        norms.Add(rotation * new Vector3(0, 1, 0));
        norms.Add(rotation * new Vector3(0, 1, 0));
        norms.Add(rotation * new Vector3(0, 1, 0));
        norms.Add(rotation * new Vector3(0, 1, 0));

        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(0, 0));


    }

    private void BuildMeshQuad()
    {
        // Make a mesh
        Mesh mesh = new Mesh();

        // Set mesh vertices
        Vector3[] verts = new Vector3[] {
            new Vector3(0.5f, 0, 0.5f),
            new Vector3(0.5f, 0, -0.5f),
            new Vector3(-0.5f, 0, -0.5f),
            new Vector3(-0.5f, 0, 0.5f)
        };
        // Set mesh triangles
        int[] tris = new int[] {
            0, 1, 3,
            1, 2, 3
        };

        // Set mesh normals
        Vector3[] norms = new Vector3[]
        {
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 0)
        };

        // Set mesh UVs
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1),
            new Vector2(0, 0)
        };

        // Set mesh color

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = norms;
        mesh.uv = uvs;

        // Set mesh filter property
        meshFilter.mesh = mesh;

    }

    
    
}
