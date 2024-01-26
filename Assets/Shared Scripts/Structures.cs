using UnityEngine;

/// <summary>
/// Representatino of a mesh vertex
/// </summary>
public struct Vertex
{
    //Index for this vertex in a mesh object
    public int index;
    
    //Position of the vertex in model space
    public Vector3 position;

    //Normal of the vertex
    public Vector3 normal;

    public Vertex(int a_index = -1, Vector3 a_position = default, Vector3 a_normal = default)
    {
        index = a_index;
        position = a_position;
        normal = a_normal;
    }
}

/// <summary>
/// Representation of a triangle mesh polygon
/// </summary>
public struct Triangle
{
    public Vertex v1;

    public Vertex v2;
    
    public Vertex v3;

    //The normal of the triangle
    public Vector3 normal;

    //The center of the triangle
    public Vector3 centroid;

    public Triangle(Vertex a_v1, Vertex a_v2, Vertex a_v3)
    {
        //Assign the verts
        v1 = a_v1;
        v2 = a_v2;
        v3 = a_v3;

        //Generate the normal
        normal = OctreeUtility.CalculateTriangleNormal(v1,v2,v3);

        //Generate the centroid
        centroid = OctreeUtility.CalculateTriangleCentroid(v1,v2,v3);
    }
}
