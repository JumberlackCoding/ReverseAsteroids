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
    private bool slingshotting = false;

    // Counter to keep track of growth level for asteroid, determines next sprite to switch to
    private int asteroidLevel;

    // Reference for asteroid created and maintained while the left mouse button is held
    private GameObject heldAsteroid;

    // Reference for the script of the held asteroid for setting direction and speed when launched
    private AsteroidLogic asteroidLogic;

    // Size of held asteroid as it increases each frame
    private Vector3 asteroidScale;

    // Prefab for the line between the asteroid and mouse when slingshotting
    [SerializeField]
    private GameObject slingShotLinePreFab;
    // Variable for tracking slingshot line
    private GameObject slingshotLine;
    // Starting mouse position when slingshotting asteroid
    private Vector3 slingshotStart;
    private Vector3 slingshotStartRaw;

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
        mouseLocation.z = -0.08f;

        // When left clicking
        if( Input.GetButton( "Fire1" ) )
        {
            // If we can spawn a new asteroid
            // Meaning we aren't holding one and the previous one has reached minimum size meaning it was let go
            if( !haveAsteroid && doneGrowing && !slingshotting )
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

            // Try to grab an asteroid that isn't moving when you right click
            if( Input.GetButtonDown( "Fire2" ) )
            {
                // Get what you right clicked on
                // You must use Physics2D if you want to collide with 2D colliders which is exactly what we're using everywhere
                // But a 2D raycast can't travel in the Z direction, so we can't fire from the camera to the mouse and see what it hits
                // Instead we start the raycast on the mouse and then have it go nowhere, so it'll just be a point instead of a ray
                // And whatever you're directly on top of will be hit
                RaycastHit2D hit = Physics2D.Raycast( mouseLocation, Vector3.zero );

                if( hit.collider != null )
                {
                    if( hit.collider.gameObject.tag == "Asteroid" )
                    {
                        asteroidLogic = hit.collider.gameObject.GetComponent<AsteroidLogic>();

                        if( asteroidLogic.GetSpeed() == 0f )
                        {
                            // Grab asteroid and slingshot
                            slingshotStart = mouseLocation;
                            slingshotStart.z = -0.04f;
                            slingshotting = true;

                            // Spawn in the line
                            slingshotLine = Instantiate( slingShotLinePreFab, slingshotStart, Quaternion.identity );
                        }
                    }
                }
            }
            else if( Input.GetButtonUp( "Fire2" ) && slingshotting )
            {
                // Calc slingshot trajectory and launch
                Vector3 mouseLocation4Calc = mouseLocation;
                mouseLocation4Calc.z = -0.04f;
                asteroidLogic.SetDirection( ( slingshotStart - mouseLocation4Calc ).normalized );
                asteroidLogic.SetSpeed( ( slingshotStart - mouseLocation4Calc ).magnitude );
                // Cleanup
                asteroidLogic = null;
                slingshotting = false;
                Destroy( slingshotLine );
            }
        }

        // If you're slingshotting, update the line position, rotation and scale so it connects the mouse to the asteroid
        if( slingshotting && ( slingshotLine != null ) )
        {
            // Scale the line in the X direction to match the distance between the mouse and asteroid
            float maxLength = asteroidLogic.GetMaxSpeed();
            Vector3 mouseLocation4Calc = mouseLocation;
            mouseLocation4Calc.z = -0.04f;
            // Since we pass in the magnitude of the vector difference between where our mouse started and where it is when we release as the speed for the asteroid when launched,
            // We will base our line's length on the same metric
            float length = Vector3.Distance( slingshotStart, mouseLocation4Calc ) > maxLength ? maxLength : Vector3.Distance( slingshotStart, mouseLocation4Calc );
            slingshotLine.transform.localScale = new Vector3( length, slingshotLine.transform.lossyScale.y, 1f );

            // Position the line so the one end is always touching the start position and the other will touch the mouse up until the maximum length is reached
            slingshotLine.transform.position = slingshotStart + ( mouseLocation4Calc - slingshotStart ).normalized * length / 2;

            // Make the line point to the asteroid and the mouse
            float zRot = Mathf.Rad2Deg * Mathf.Atan( ( slingshotStart.y - mouseLocation4Calc.y ) / ( slingshotStart.x - mouseLocation4Calc.x ) );
            // When you first right click, the angle is nonexistent since your mouse is on the same point that you right clicked cuz you haven't moved yet
            zRot = float.IsNaN( zRot ) ? 0 : zRot;
            // I prefer to do euler angle rotations rather than playing with quaternion rotations since you can easily say rotate around this single axis only
            // We also use Quaternion.Euler instead of transform.Rotate because the former sets the rotation, the latter adds that rotation
            // Thus if we used transform.Rotate, it'd spin in circles hyper fast
            slingshotLine.transform.rotation = Quaternion.Euler( slingshotLine.transform.rotation.eulerAngles.x, slingshotLine.transform.rotation.eulerAngles.y, zRot );

            // Play with the color
            float percent = length / maxLength;
            Color newColor;
            // Lerp is short for Linear Interpolation meaning it will interpolate the color between color a and b based on the percent
            // THe higher the percent, the close to color b, lower percent mean closer to color a
            newColor = Color.Lerp( Color.green, Color.red, percent );
            slingshotLine.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    // Called for fun and visualization
    void OnDrawGizmos()
    {
    }
}
