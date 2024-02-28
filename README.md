# OctreeGeneration

This is an upload of an older project prototype I had made to sort model triangles into an octree.

The generator takes the bounding box of a given model and decides if it should subdivide into a new layer depending on the number of triangles within each octant.
By default it considers an octant to be a leaf node if it contains 100 triangles or less. 
Triangles are considered to be within an octant if the octant contains one of the vertices and triangles with vertices in multiple octants will be contained in the list of each.

Originally I had planned on adding more functionality such as addition/removal of entries within the tree as well as optimizing it for runtime use by taking advantage of multithreading or converting it to be generated in compute shaders. I've moved on to other projects since and so won't be making those changes at this time
