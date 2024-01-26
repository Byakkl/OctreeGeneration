using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeUtility
{
    public static void FindContainedTriangles(Bounds a_bounds, List<Triangle> a_toParse, ref List<Triangle> a_toPopulate)
    {
        //Spin through the triangle list
        foreach (Triangle tri in a_toParse)
        {
            //If any of the triangle's verts are contained by this box consider it being part of this list
            //This means there will be overlapping storage but it will cover cases with triangles crossing bounds
            if (a_bounds.Contains(tri.v1.position) || a_bounds.Contains(tri.v2.position) || a_bounds.Contains(tri.v3.position))
                a_toPopulate.Add(tri);
        }
    }

    /// <summary>
    /// Helper Function - Calculate the centroid of a triangle defined by the provided vertices
    /// </summary>
    /// <param name="a_v1"></param>
    /// <param name="a_v2"></param>
    /// <param name="a_v3"></param>
    /// <returns></returns>
    public static Vector3 CalculateTriangleCentroid(Vertex a_v1, Vertex a_v2, Vertex a_v3)
    {
        return (a_v1.position + a_v2.position + a_v3.position) / 3.0f;
    }


    /// <summary>
    /// Helper Function - Calculate the normal of a triangle defined by the provided vertices
    /// Winding order is determined by the order in which the vertices are passed in
    /// </summary>
    /// <param name="a_v1"></param>
    /// <param name="a_v2"></param>
    /// <param name="a_v3"></param>
    /// <returns></returns>
    public static Vector3 CalculateTriangleNormal(Vertex a_v1, Vertex a_v2, Vertex a_v3)
    {
        //Create a line going from Vertex 1 to Vertex 2
        Vector3 lineV1V2 = new Vector3();
        lineV1V2 = a_v2.position - a_v1.position;

        //Create a line going from Vertex 1 to Vertex 3
        Vector3 lineV1V3 = new Vector3();
        lineV1V3 = a_v3.position - a_v1.position;

        //Take the cross product of the two lines to determine the normal
        Vector3 result = Vector3.Cross(lineV1V2, lineV1V3);

        //Normalize the result to get a usable value
        result.Normalize();

        return result;
    }

    /// <summary>
    /// Generates a triangle list for a given mesh
    /// </summary>
    /// <param name="a_source">The source mesh</param>
    /// <param name="a_toPopulate">The list to populate</param>
    public static void PullTrianglesFromMesh(Mesh a_source, ref List<Triangle> a_toPopulate)
    {
        //Create and populate an array of indices that make up all triangles in the mesh
        int[] tris = a_source.GetIndices(0);

        //Initialize position and normal container lists
        List<Vector3> vertexPositions = new List<Vector3>();
        List<Vector3> vertexNormals = new List<Vector3>();

        //Get the positions of all vertices in the mesh, note that the order of this list matches with the index values of each vertex
        a_source.GetVertices(vertexPositions);

        //Get the normals of all vertices in the mesh, note that the order of this list matches with the index values of each vertex
        a_source.GetNormals(vertexNormals);

        //For each triangle in the generated array, create a triangle structure
        for (int idx = 0; idx < tris.Length; idx += 3)
        {
            //Generate Vertex 1 of the triangle from the previously retrieved data
            Vertex vert1 = new Vertex();
            //Store the index of the vertex in the structure in case more data is needed later
            vert1.index = tris[idx];
            //Store the vertex's normal
            vert1.normal = vertexNormals[vert1.index];
            //Store the veretx's position
            vert1.position = vertexPositions[vert1.index];

            //Generate Vertex 2
            Vertex vert2 = new Vertex();
            vert2.index = tris[idx + 1];
            vert2.normal = vertexNormals[vert2.index];
            vert2.position = vertexPositions[vert2.index];

            //Generate Vertex 3
            Vertex vert3 = new Vertex();
            vert3.index = tris[idx + 2];
            vert3.normal = vertexNormals[vert3.index];
            vert3.position = vertexPositions[vert3.index];

            //Using the generated vertices, create a triangle structure
            Triangle tri = new Triangle(vert1, vert2, vert3);

            //Add the generated triangle to the output list
            a_toPopulate.Add(tri);
        }
    }
}
