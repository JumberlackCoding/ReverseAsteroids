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
    private Camera mainCamera;

    [SerializeField]
    private float growthRateDefault;
    [SerializeField]
    private float growthRateDecay;

    [SerializeField]
    private Vector3 startingAsteroidScale;

    private bool haveAsteroid = false;
    private bool doneGrowing = true;

    private int asteroidLevel;

    private float growthRateActual;

    private GameObject heldAsteroid;

    private Vector3 asteroidScale;

    // Start is called before the first frame update
    void Start()
    {
        asteroidScale = startingAsteroidScale;
        growthRateActual = growthRateDefault;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseLocation = mainCamera.ScreenToWorldPoint( Input.mousePosition );
        mouseLocation.z = -0.04f;

        if( Input.GetButton( "Fire1" ) )
        {
            if( !haveAsteroid && doneGrowing )
            {
                heldAsteroid = Instantiate( asteroidLvl0Prefab, mouseLocation, Quaternion.identity );
                heldAsteroid.transform.localScale = asteroidScale;
                asteroidLevel = 0;
                haveAsteroid = true;
                doneGrowing = false;
            }
        }
        else if( Input.GetButtonUp( "Fire1" ) )
        {
            haveAsteroid = false;
        }

        if( haveAsteroid || !doneGrowing )
        {
            heldAsteroid.transform.position = mouseLocation;
            asteroidScale = new Vector3( asteroidScale.x + ( growthRateActual * Time.deltaTime ), asteroidScale.y + ( growthRateActual * Time.deltaTime ), 1f );

            if( ( heldAsteroid.transform.localScale.x >= 1f ) && ( heldAsteroid.transform.localScale.y >= 1f ) )
            {
                switch( asteroidLevel )
                {
                    case 0:
                        Destroy( heldAsteroid );
                        heldAsteroid = Instantiate( asteroid1Lvl1Prefab, mouseLocation, Quaternion.identity );
                        asteroidScale = new Vector3( 0.5f, 0.5f, 1f );
                        asteroidLevel++;
                        growthRateActual *= growthRateDecay;
                        break;
                    case 1:
                        Destroy( heldAsteroid );
                        heldAsteroid = Instantiate( asteroid1Lvl2Prefab, mouseLocation, Quaternion.identity );
                        asteroidScale = new Vector3( 0.5f, 0.5f, 1f );
                        asteroidLevel++;
                        growthRateActual *= growthRateDecay;
                        doneGrowing = true;
                        break;
                    case 2:
                        Destroy( heldAsteroid );
                        heldAsteroid = Instantiate( asteroid1Lvl3Prefab, mouseLocation, Quaternion.identity );
                        asteroidScale = new Vector3( 0.5f, 0.5f, 1f );
                        asteroidLevel++;
                        growthRateActual *= growthRateDecay;
                        break;
                    case 3:
                        asteroidScale = Vector3.one;
                        break;
                    default:
                        Debug.LogWarning( "Asteroid level out of bounds" );
                        break;
                }
            }

            heldAsteroid.transform.localScale = asteroidScale;
        }
        else
        {
            asteroidScale = asteroidScale = startingAsteroidScale;
            asteroidLevel = 0;
            growthRateActual = growthRateDefault;
            heldAsteroid = null;
        }

    }

    void OnDrawGizmos()
    {

    }
}
