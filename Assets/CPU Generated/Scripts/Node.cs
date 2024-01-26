using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;


public class Node
{
    public int numNodes;
    public Bounds bounds;
    public Vector3[] min_max_bounds;
    public Node[] children;
    public bool leaf;
    public List<Triangle> tris;

    /// <summary>
    /// Recursive generation of octree nodes
    /// </summary>
    /// <param name="in_bounds"></param>
    /// <param name="triangles"></param>
    /// <param name="nodeCount"></param>
    public Node(Bounds in_bounds, List<Triangle> triangles, ref int nodeCount)
    {
        nodeCount++;
        numNodes = 0;
        bounds = in_bounds;
        //Store the bounds min and max point
        min_max_bounds = new Vector3[2];
        min_max_bounds[0] = in_bounds.min;
        min_max_bounds[1] = in_bounds.max;

        tris = new List<Triangle>();
        children = new Node[8];

        //Grab the extrents (half size) of the box
        Vector3 extents = in_bounds.extents;
        //If the extents are greater than a threshold, subdivide
        if (triangles.Count > OctreeControls.MAX_TRIS)
        {
            //If we are subdividing it isn't a leaf, this can be optimized based on number of tris contained (0 shoould stop and be a leaf)
            leaf = false;
            //Dividing the center by two gives the bottom left center (closest to origin)
            Vector3 octCenter = in_bounds.center;
            //We need to organize the boxes
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int z = 0; z < 2; z++)
                    {
                        //The new center is the same as the bottom left we determined + offsets based on which quadrant we are building
                        Vector3 offset = new Vector3(in_bounds.size.x * (x - 0.5f), in_bounds.size.y * (y - 0.5f), in_bounds.size.z * (z - 0.5f));
                        Vector3 newCenter = octCenter + offset / 2;
                        //Create a new bounds, we know the size is half in all dimensions so we can just use the extents as it's pregenerated
                        Bounds newBounds = new Bounds(newCenter, extents);

                        //Determine which triangles in this node are contained in each child node
                        List<Triangle> nodeTris = new List<Triangle>();
                        OctreeUtility.FindContainedTriangles(newBounds, triangles, ref nodeTris);
                        //Store the oct nodes based on coordinates;
                        //0: Bottom, Front, Left
                        //1: Bottom, Front, Right
                        //2: Top, Front, Left
                        //3: Top, Front, Right
                        //4: Bottom, Back, Left
                        //5: Bottom, Back, Right
                        //6: Top, Back, Left
                        //7: Top, Back, Right
                        children[x + y * 2 + z * 4] = new Node(newBounds, nodeTris, ref nodeCount);
                    }
                }
            }
        }
        else
        {
            //We are a leaf node
            leaf = true;
            tris = triangles;
        }
    }
}
