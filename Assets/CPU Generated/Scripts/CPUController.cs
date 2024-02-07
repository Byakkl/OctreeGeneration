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
    public List<Mesh> sourceMeshes = new List<Mesh>();

    //Stores all of the root nodes of the generated octrees. The indexing order will match that of the source mesh array
    public static List<Node> generatedTrees = new List<Node>();

    void Start()
    {
        //Generate octrees for each mesh in the list and add them to the tracking dictionary
        foreach (Mesh mesh in sourceMeshes)
        {
            generatedTrees.Add(GenerateOctree(mesh));
        }
    }

    private Node GenerateOctree(Mesh a_mesh)
    {
        Stopwatch sw = new Stopwatch();
        sw.Reset();
        sw.Start();

        //Create the triangle storage list
        List<Triangle> triangles = new List<Triangle>();

        //Get the triangles from the mesh
        OctreeUtility.PullTrianglesFromMesh(a_mesh, ref triangles);

        int numNodes = 0;
        int maxDepth = 0;

        //Generate the octree using the bounds and trianglese of the mesh
        Node rootNode = new Node(a_mesh.bounds, triangles, 0, ref numNodes, ref maxDepth);

        sw.Stop();
        UnityEngine.Debug.Log($"Octree Generated." +
        $"Mesh Name: {a_mesh.name}\n" +
        $"Triangle Count: {triangles.Count}\n" +
        $"Elapsed Time: {sw.ElapsedMilliseconds}ms\n" +
        $"Node Count: {numNodes}\n" +
        $"Max Depth: {maxDepth}");

        return rootNode;
    }

    public static Node FetchRootNode(int a_idx)
    {
        if (a_idx >= generatedTrees.Count)
            return null;
        else
            return generatedTrees[a_idx];
    }
}
