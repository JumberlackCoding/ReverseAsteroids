using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShipPlayer : MonoBehaviour
{
    // Bool to determine if the ship should be player or AI driven
    [SerializeField]
    private bool playerControlled = false;

    // EdgeLogic script reference for handling looping of the ship around edges
    [SerializeField]
    private EdgeLogic edgeLogic;

    // List of KeyCodes for input buttons if the ship is player controlled
    [SerializeField]
    private KeyCode forward;
    [SerializeField]
    private KeyCode backward;
    [SerializeField]
    private bool allowedToMoveBackward;
    [SerializeField]
    private KeyCode spinLeft;
    [SerializeField]
    private KeyCode spinRight;
    [SerializeField]
    private KeyCode brake;
    [SerializeField]
    private KeyCode fire;

    // Various movement related variables
    // Translational acceleration
    [SerializeField]
    private float moveRate;
    // Rotational acceleration
    [SerializeField]
    private float rotRate;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxRotation;
    // This one makes it so if you're flying in one direction and aiming in another, pressing forward doesn't instantly change your direction
    // to 100% your new forward and instead slowly corrects in that direction.  The greater this value, the slower the course correction is
    [SerializeField]
    private int directionalSluggishness;

    // All the current movement values
    [SerializeField]
    private float currentRotation;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private Vector3 currentDirection;

    // Bullet related variables
    // Basic bullet prefab
    [SerializeField]
    private GameObject bulletPreFab;
    // Bullet pool where bullets are pulled from when fired
    [SerializeField]
    private GameObject[] bulletBank;
    // Size of the bullet pool
    [SerializeField]
    private int bulletBankCount;
    // These two prevent the ship from being able to fire a bullet every single frame.  Adds a time based delay between firing bullets
    [SerializeField]
    private float fireDelay;
    [SerializeField]
    private float fireLast;

    void Start()
    {
        // Initialize the bullet pool and disable all of them in the hierarchy
        bulletBank = new GameObject[bulletBankCount];

        for( int i = 0; i < bulletBankCount; i++ )
        {
            bulletBank[i] = Instantiate( bulletPreFab, new Vector3( 100, 100, 0 ), Quaternion.identity );
            bulletBank[i].SetActive( false );
        }
    }

    void Update()
    {
        // When the ship is player driven
        if( playerControlled )
        {
            // Watch for any input keys set in the inspector
            if( Input.GetKey( forward ) )
            {
                // Mathf.Clamp will take the value of the first argument and clamp it within the minimum and maximum values set by the 2nd and 3rd params respectively
                currentSpeed = Mathf.Clamp( currentSpeed + moveRate, -maxSpeed, maxSpeed );
                // Direction is always normalized since we've split out direction and speed to a Vector3 and a float instead of having the magnitude of the Vector3 be the speed
                currentDirection = ( ( currentDirection * directionalSluggishness ) + transform.up ).normalized;
            }
            if( Input.GetKey( backward ) && allowedToMoveBackward )
            {
                currentSpeed = Mathf.Clamp( currentSpeed - moveRate, -maxSpeed, maxSpeed );
                currentDirection = ( ( currentDirection * directionalSluggishness ) + transform.up ).normalized;
            }
            if( Input.GetKey( spinLeft ) )
            {
                currentRotation = Mathf.Clamp( currentRotation + rotRate, -maxRotation, maxRotation );
            }
            if( Input.GetKey( spinRight ) )
            {
                currentRotation = Mathf.Clamp( currentRotation - rotRate, -maxRotation, maxRotation );
            }
            if( Input.GetKey( brake ) )
            {
                if( currentSpeed > 0 )
                {
                    currentSpeed -= moveRate;
                }
                else if( currentSpeed < 0 )
                {
                    currentSpeed += moveRate;
                }

                if( currentRotation > 0 )
                {
                    currentRotation -= rotRate;
                }
                else if( currentRotation < 0 )
                {
                    currentRotation += rotRate;
                }

                // if the current speed is sufficiently slow, just set it to 0 so we don't drift forever at a super duper slow rate
                if( Mathf.Abs( currentSpeed ) < 0.0001f )
                {
                    currentSpeed = 0f;
                    currentDirection = Vector3.zero;
                }

                if( Mathf.Abs( currentRotation ) < 0.0001f )
                {
                    currentRotation = 0f;
                }
            }
            if( Input.GetKey( fire ) )
            {
                // Check if it's been long enough to fire again
                if( Time.time >= fireLast + fireDelay )
                {
                    FireBullet();
                    fireLast = Time.time;
                }
            }
        }
    }

    void LateUpdate()
    {
        // Apply movement changes
        transform.position += currentDirection.normalized * currentSpeed * Time.deltaTime;
        transform.Rotate( Vector3.forward * currentRotation * Time.deltaTime, Space.World );
    }

    void FireBullet()
    {
        // Attempt to get a bullet from the pool and don't blow up if they're all taken
        try
        {
            GameObject bullet = GetBullet();
            bullet.transform.position = transform.position;
            bullet.GetComponent<BulletLogic>().SetDirection( transform.up );
            bullet.SetActive( true );
        }
        catch( NullReferenceException e )
        {
            Debug.LogWarning( "No bullets in bank " + e.Message );
        }
    }

    GameObject GetBullet()
    {
        GameObject result = null;

        foreach( GameObject go in bulletBank )
        {
            if( !go.activeInHierarchy )
            {
                result = go;
                break;
            }
        }

        return result;
    }

    // When the ship collider + rigidbody collides with another collider that has 'IsTrigger = true'
    void OnTriggerEnter2D( Collider2D col )
    {
        // Make sure the game object we collided with has a tag of "Border" meaning it's reached far enough off screen that there's a clone already
        // running around in that the player is using as the main ship.  Time to destroy this oen to prevent weird stuff from happening
        if( col.gameObject.tag == "Border" )
        {
            // Reset edge logic so we can now loop again
            edgeLogic.ResetEdgeUse();

            // Don't forget to clean up the bullet bank until we move this to the GameManager so we don't have to spawn a new one of these everytime
            // we spawn a new ship when we loop around the edges
            foreach( GameObject go in bulletBank )
            {
                Destroy( go );
            }

            // Bye bye ship
            Destroy( gameObject );
        }
    }
}
