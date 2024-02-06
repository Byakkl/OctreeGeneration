using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;


public class OctreeRenderer : MonoBehaviour
{
    /// <summary>
    /// Index of the mesh to be rendered. This value will correspond to the index of the meshes in CPUController
    /// </summary>
    public int meshIndex = 0;

    [Header("Octree Node Visual Controls")]
    /// <summary>
    /// Toggles the display of nodes for the mesh with the name matching the value 
    /// </summary>
    public bool displayNodes = false;

    /// <summary>
    /// Sets the minimum depth to begin rendering, this allows parent nodes to be stripped out to reduce clutter
    /// </summary>
    public int minimumNodeDepth = 0;

    /// <summary>
    /// Toggles the display of child nodes under the set depth value
    /// Setting this to false allows for the view of a single node layer
    /// </summary>
    public bool displayNodesBelowDepth = true;

    public bool showOnlyPopulatedLeafNodes = false;


    private void OnDrawGizmos()
    {
        Node rootNode = null;

        if (displayNodes)
        {
            if (!FetchAndValidateRootNode(meshIndex, ref rootNode))
                return;

            if (minimumNodeDepth == 0)
                if (ShouldDisplay(rootNode))
                {
                    //Draw the root node
                    Gizmos.DrawWireCube(rootNode.bounds.center, rootNode.bounds.size);
                }
            //Draw the child nodes
            DrawChildren(rootNode, 1);
        }
    }

    /// <summary>
    /// Attempts to fetch a root node using the provided index
    /// </summary>
    /// <param name="a_idx">The index of the root node</param>
    /// <param name="a_toPopulate">The reference to be populated with the fetch. Will be NULL on failure</param>
    /// <returns>True: Successfully found a valid root node. False: No valid root node exists</returns>
    private bool FetchAndValidateRootNode(int a_idx, ref Node a_toPopulate)
    {
        //Get the root node reference from the stored array of generated trees using the set index
        a_toPopulate = CPUController.FetchRootNode(meshIndex);

        //Validate the root node
        if (a_toPopulate == null)
        {
            UnityEngine.Debug.Log($"Root node at provided index [{meshIndex}] does not exist. Rendering aborted.");
            return false;
        }

        return true;
    }

    private void DrawChildren(Node a_parentNode, int currentDepth)
    {
        //There are no children, exit
        if (a_parentNode.leaf)
            return;

        for (int idx = 0; idx < 8; idx++)
        {
            //Get the child node data
            Node child = a_parentNode.children[idx];

            //Only draw the children if the current depth matches one of the two conditions
            //1. Current depth is AT LEAST at the minimum node depth and the toggle to display nodes beyond the minimum is TRUE
            //2. Current depth is not beyond the minimum node depth and the toggle to display nodes beyond the minimum is FALSE
            if ((currentDepth >= minimumNodeDepth && displayNodesBelowDepth) || (currentDepth == minimumNodeDepth && !displayNodesBelowDepth))
            {
                if (ShouldDisplay(child))
                {
                    //Set the display color
                    SetGizmoColor(idx);

                    //Draw the node representation
                    Gizmos.DrawWireCube(child.bounds.center, child.bounds.size);
                }
            }
            //Recursively draw children
            DrawChildren(child, currentDepth + 1);
        }
    }

    /// <summary>
    /// Controls the display of nodes based on leaf node control toggle
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private bool ShouldDisplay(Node node)
    {
        if(showOnlyPopulatedLeafNodes)
            if(node.leaf && node.tris.Count > 0)
                return true;
            else
                return false;

        return true;
    }

    /// <summary>
    /// Sets the color of rendered gizmos based on an input index.
    /// This is just a lazy way of having each node on a layer rendered as a different color for clarity
    /// </summary>
    /// <param name="idx"></param>
    private void SetGizmoColor(int idx)
    {
        switch (idx)
        {
            default:
            case 0:
                Gizmos.color = Color.black;
                break;
            case 1:
                Gizmos.color = Color.red;
                break;

            case 2:
                Gizmos.color = Color.green;
                break;

            case 3:
                Gizmos.color = Color.blue;
                break;

            case 4:
                Gizmos.color = Color.yellow;
                break;

            case 5:
                Gizmos.color = Color.magenta;
                break;

            case 6:
                Gizmos.color = Color.cyan;
                break;

            case 7:
                Gizmos.color = Color.white;
                break;
        }
    }
}
