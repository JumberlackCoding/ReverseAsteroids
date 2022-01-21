using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeLogic : MonoBehaviour
{
    // Tag to search for for objects that can loop around the edges
    [SerializeField]
    private new string tag;

    // Numerical +/-X/Y values for where points at which a clone ship should be spawned
    [SerializeField]
    private float leftBoundary;
    [SerializeField]
    private float rightBoundary;
    [SerializeField]
    private float topBoundary;
    [SerializeField]
    private float bottomBoundary;

    // Boolean triggering the gizmo lines to be drawn or not
    [SerializeField]
    private bool showDebugLines;

    // Separate bools for spawning clones for vertical and horizontal looping
    // These are used to prevent a bajillion clones from being spawned when approaching an edge
    private bool horizEdgeInUse;
    private bool vertEdgeInUse;

    // Start is called before the first frame update
    void Start()
    {
        horizEdgeInUse = false;
        vertEdgeInUse = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Find all loopable objects i.e. the ship currently
        GameObject[] allMoveables = GameObject.FindGameObjectsWithTag( tag );

        foreach( GameObject obj in allMoveables )
        {
            bool left, right, top, bottom;

            // Check if the ship is near and edge and determine which one(s)
            if( OnTheEdge( obj, out left, out right, out top, out bottom ) )
            {
                // Grab the ship's current position and invert the X or Y values based on which edge it's near and spawn a clone on the other side
                // Then lock down that edge so no more clones can spawn
                Vector3 spawnPos = obj.transform.position;

                if( ( left || right ) && !horizEdgeInUse )
                {
                    spawnPos.x *= -1;
                    Instantiate( obj, spawnPos, obj.transform.rotation );
                    horizEdgeInUse = true;
                }

                if( ( top || bottom ) && !vertEdgeInUse )
                {
                    spawnPos.y *= -1;
                    Instantiate( obj, spawnPos, obj.transform.rotation );
                    vertEdgeInUse = true;
                }
            }
        }
    }

    // Function makes use of the cool "out" feature of C# functions to return which edge(s) the ship is near
    bool OnTheEdge( GameObject obj, out bool left, out bool right, out bool top, out bool bottom )
    {
        bool nearEdge = false;
        left = false;
        right = false;
        top = false;
        bottom = false;

        if( obj.transform.position.x < leftBoundary )
        {
            nearEdge = true;
            left = true;
        }
        else if( obj.transform.position.x > rightBoundary )
        {
            nearEdge = true;
            right = true;
        }

        if( obj.transform.position.y > topBoundary )
        {
            nearEdge = true;
            top = true;
        }
        else if( obj.transform.position.y < bottomBoundary )
        {
            nearEdge = true;
            bottom = true;
        }

        return nearEdge;
    }

    // Function called by ships when they self-destruct thus enabling more ships to be spawned
    // This is not a perfect solution - if the spawn walls and the destruction walls are too far apart, you can get some funky business
    // where there can be numerous ships since this function does not specifiy which edge is available again.  Therefore, it is possible
    // to run off a corner, spawn 2 ships, kill 1 and enable the spawning of 2 more.  Unlikely to occur unless the user is explicitly
    // attempting to do so
    public void ResetEdgeUse()
    {
        horizEdgeInUse = false;
        vertEdgeInUse = false;
    }

    // Allows us to visualize where the spawn boundaries are
    void OnDrawGizmos()
    {
        if( showDebugLines )
        {
            Gizmos.DrawLine( new Vector3( leftBoundary, -10, -0.05f ), new Vector3( leftBoundary, 10, -0.05f ) );
            Gizmos.DrawLine( new Vector3( rightBoundary, -10, -0.05f ), new Vector3( rightBoundary, 10, -0.05f ) );
            Gizmos.DrawLine( new Vector3( -10, topBoundary, -0.05f ), new Vector3( 10, topBoundary, -0.05f ) );
            Gizmos.DrawLine( new Vector3( -10, bottomBoundary, -0.05f ), new Vector3( 10, bottomBoundary, -0.05f ) );
        }
    }
}
