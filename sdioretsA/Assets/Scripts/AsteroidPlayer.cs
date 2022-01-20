using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroidLvl0Prefab;
    [SerializeField]
    private GameObject asteroid1Lvl1Prefab;
    [SerializeField]
    private GameObject asteroid1Lvl2Prefab;
    [SerializeField]
    private GameObject asteroid1Lvl3Prefab;

    [SerializeField]
    private float growthRate;
    [SerializeField]
    private float maxSize;

    [SerializeField]
    private Camera mainCamera;

    private bool haveAsteroid = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetButton( "Fire1" ) )
        {
            Physics.Raycast( mainCamera.transform.position, Input.mousePosition, 10f );

            if( !haveAsteroid )
            {

            }

        }
    }

    void OnDrawGizmos()
    {
        Ray r = new Ray( mainCamera.transform.position, Input.mousePosition );
        Gizmos.DrawRay( r );
    }
}
