using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlayer : MonoBehaviour
{
    // Prefab for all spawned asteroids
    [SerializeField]
    private GameObject asteroidBase;

    // Sprites the asteroids will switch to as they grow in size
    [SerializeField]
    private Sprite asteroid1Lvl1Sprite;
    [SerializeField]
    private Sprite asteroid1Lvl2Sprite;
    [SerializeField]
    private Sprite asteroid1Lvl3Sprite;

    // main camera also accessible via "Camera.main" but this is more consistent in form and defensive in case multiple cameras are added
    // For example if we were to include a minimap or something
    [SerializeField]
    private Camera mainCamera;

    // Variables for determining speed at which held asteroids increase in size
    // Growth rate is set to the default for every new asteroid and each grow phase it is reduced by the decay amount multiplicitively
    // For some reason keeping the growth rate constant makes the larger sprites grow at an exponentially faster rate
    [SerializeField]
    private float growthRateDefault;
    [SerializeField]
    private float growthRateDecay;
    private float growthRateActual;

    // Starting size of all asteroids when spawned
    [SerializeField]
    private Vector3 startingAsteroidScale;

    // Various bools for handling when the player is holding onto/releases an asteroid and when it has reached minimum size
    private bool haveAsteroid = false;
    private bool hadAsteroid = false;
    private bool doneGrowing = true;

    // Counter to keep track of growth level for asteroid, determines next sprite to switch to
    private int asteroidLevel;

    // Reference for asteroid created and maintained while the left mouse button is held
    private GameObject heldAsteroid;

    // Reference for the script of the held asteroid for setting direction and speed when launched
    private AsteroidLogic asteroidLogic;

    // Size of held asteroid as it increases each frame
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
        // Grab the mouse position every frame and lock the Z coord to -0.04f
        Vector3 mouseLocation = mainCamera.ScreenToWorldPoint( Input.mousePosition );
        mouseLocation.z = -0.04f;

        // When left clicking
        if( Input.GetButton( "Fire1" ) )
        {
            // If we can spawn a new asteroid
            // Meaning we aren't holding one and the previous one has reached minimum size meaning it was let go
            if( !haveAsteroid && doneGrowing )
            {
                // Spawn new asteroid, scale it and initialize key variables
                heldAsteroid = Instantiate( asteroidBase, mouseLocation, Quaternion.identity );
                heldAsteroid.transform.localScale = asteroidScale;
                asteroidLogic = heldAsteroid.GetComponent<AsteroidLogic>();
                asteroidLevel = 0;
                haveAsteroid = true;
                hadAsteroid = true;
                doneGrowing = false;
            }
        }
        // When releasing left click, flip the flag saying let go of the asteroid once the minimum reqs are met
        else if( Input.GetButtonUp( "Fire1" ) )
        {
            haveAsteroid = false;
        }

        // Asteroid grows and follows mouse until minimum size is reached
        if( haveAsteroid || !doneGrowing )
        {
            heldAsteroid.transform.position = mouseLocation;

            // It's **SUPER IMPORTANT** to **ALWAYS** multiply by Time.deltaTime when doing any kind of adjustment to speed/size/position/rotation/etc
            // Time.deltaTime is the amount of time since the previous frame.  By multiplying by that, you're equation will not be dependent on frame rate
            // If you do not multiply by Time.deltaTime, then having a higher frame rate will mean greater acceleration/speed/growth/spin/etc
            // And having a lower frame rate will respectively mean slower acceleration/growth/speed/spin/etc
            asteroidScale = new Vector3( asteroidScale.x + ( growthRateActual * Time.deltaTime ), asteroidScale.y + ( growthRateActual * Time.deltaTime ), 1f );

            // Once the held asteroid reaches 100% size for the given sprite, time to move up in resolution or reset it
            if( ( heldAsteroid.transform.localScale.x >= 1f ) && ( heldAsteroid.transform.localScale.y >= 1f ) )
            {
                switch( asteroidLevel )
                {
                    // For each case 0-2, it will replace the sprite in the SpriteRenderer with the next highest quality sprite, reset the scale and decay growth rate
                    case 0:
                        heldAsteroid.GetComponent<SpriteRenderer>().sprite = asteroid1Lvl1Sprite;
                        asteroidScale = new Vector3( 0.5f, 0.5f, 1f );
                        asteroidLevel++;
                        growthRateActual *= growthRateDecay;
                        break;
                    case 1:
                        heldAsteroid.GetComponent<SpriteRenderer>().sprite = asteroid1Lvl2Sprite;
                        asteroidScale = new Vector3( 0.5f, 0.5f, 1f );
                        asteroidLevel++;
                        growthRateActual *= growthRateDecay;
                        // Minimum size has been reached and will stop growing once left mouse is released
                        doneGrowing = true;
                        break;
                    case 2:
                        heldAsteroid.GetComponent<SpriteRenderer>().sprite = asteroid1Lvl3Sprite;
                        asteroidScale = new Vector3( 0.5f, 0.5f, 1f );
                        asteroidLevel++;
                        growthRateActual *= growthRateDecay;
                        break;
                    // If we've already reached max leve, reset the size to 1 (100%) so the sprite doesn't continue to grow as we've reached max size
                    case 3:
                        asteroidScale = Vector3.one;
                        break;
                    // This should never be hit
                    default:
                        Debug.LogWarning( "Asteroid level out of bounds" );
                        break;
                }
            }

            // Every frame apply the new size of the asteroid
            heldAsteroid.transform.localScale = asteroidScale;
        }
        else
        {
            // Added extra bool so this is only called if an asteroid was previously held and then released
            // This way it doesn't get called every single frame when just not holding an asteroid
            if( hadAsteroid )
            {
                // Reset scale/level/growth rate and bools and null out the pointer to the previously held asteroid
                asteroidScale = startingAsteroidScale;
                asteroidLevel = 0;
                growthRateActual = growthRateDefault;
                heldAsteroid = null;
                hadAsteroid = false;
            }
        }
    }

    // Called for fun and visualization
    void OnDrawGizmos()
    {

    }
}
