using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// This class is the monobehaviour that controls the generation process
/// </summary>
public class CPUController : MonoBehaviour
{
    //List of meshes to generate octrees for
    public List<Mesh> meshes;
    
    void Start()
    {
        foreach(Mesh mesh in meshes)
        {
            GenerateOctree(mesh);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateOctree(Mesh a_mesh)
    {
        Stopwatch sw = new Stopwatch();
        sw.Reset();
        sw.Start();

        //Create the triangle storage list
        List<Triangle> triangles = new List<Triangle>();

        //Get the triangles from the mesh
        OctreeUtility.PullTrianglesFromMesh(a_mesh, ref triangles);

        int numNodes = 0;

        //Generate the octree using the bounds and trianglese of the mesh
        Node rootNode = new Node(a_mesh.bounds, triangles, ref numNodes);

        rootNode.numNodes = numNodes;

        sw.Stop();
        UnityEngine.Debug.Log($"Octree Generated." + 
        $"Mesh Name: {a_mesh.name}\n" +
        $"Triangle Count: {triangles.Count}\n" +
        $"Elapsed Time: {sw.ElapsedMilliseconds}ms\n" +
        $"Node Count: {numNodes}");
    }
}