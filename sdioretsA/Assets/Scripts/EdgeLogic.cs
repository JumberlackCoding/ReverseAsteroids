using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeLogic : MonoBehaviour
{
    [SerializeField]
    private new string tag;

    [SerializeField]
    private float leftBoundary;
    [SerializeField]
    private float rightBoundary;
    [SerializeField]
    private float topBoundary;
    [SerializeField]
    private float bottomBoundary;

    [SerializeField]
    private bool showDebugLines;

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
        GameObject[] allMoveables = GameObject.FindGameObjectsWithTag( tag );

        foreach( GameObject obj in allMoveables )
        {
            bool left, right, top, bottom;

            if( OnTheEdge( obj, out left, out right, out top, out bottom ) )
            {
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

    public void ResetEdgeUse()
    {
        horizEdgeInUse = false;
        vertEdgeInUse = false;
    }

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
