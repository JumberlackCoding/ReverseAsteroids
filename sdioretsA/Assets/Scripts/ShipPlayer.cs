using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ShipPlayer : MonoBehaviour
{
    // EdgeLogic script reference for handling looping of the ship around edges
    [SerializeField]
    private EdgeLogic edgeLogic;

    [SerializeField]
    private ShipManager shipManager;

    // Bullet manager 
    [SerializeField]
    private BulletManager bulletManager;

    // System for controlling the fire on engine thrust
    [SerializeField]
    private ParticleSystem[] ThrusterParticles;

    private bool engineOn = false;

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

    void Start()
    {

    }

    void Update()
    {
        // When the ship is player driven
        if( shipManager.playerControlled )
        {
            // Watch for any input keys set in the inspector
            if( Input.GetKey( forward ) )
            {
                // Mathf.Clamp will take the value of the first argument and clamp it within the minimum and maximum values set by the 2nd and 3rd params respectively
                currentSpeed = Mathf.Clamp( currentSpeed + moveRate, -maxSpeed, maxSpeed );
                // Direction is always normalized since we've split out direction and speed to a Vector3 and a float instead of having the magnitude of the Vector3 be the speed
                currentDirection = ( ( currentDirection * directionalSluggishness ) + transform.up ).normalized;

                if( !engineOn )
                {
                    foreach( ParticleSystem ps in ThrusterParticles )
                    {
                        ps.Play();
                    }

                    engineOn = true;
                }
            }
            if( Input.GetKeyUp( forward ) )
            {
                if( engineOn )
                {
                    foreach( ParticleSystem ps in ThrusterParticles )
                    {
                        ps.Stop();
                    }

                    engineOn = false;
                }
            }
            if( Input.GetKey( backward ) && allowedToMoveBackward )
            {
                currentSpeed = Mathf.Clamp( currentSpeed - moveRate, -maxSpeed, maxSpeed );
                currentDirection = ( ( currentDirection * directionalSluggishness ) + transform.up ).normalized;
            }
            else if( Input.GetKey( backward ) )
            {
                if( currentSpeed > 0 )
                {
                    currentSpeed -= moveRate;
                }
                else if( currentSpeed < 0 )
                {
                    currentSpeed += moveRate;
                }

                if( Mathf.Abs( currentSpeed ) < 0.0001f )
                {
                    currentSpeed = 0f;
                    currentDirection = Vector3.zero;
                }
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
                // Fire the bullet
                bulletManager.FireBullet( transform );
            }
        }
    }

    void LateUpdate()
    {
        if( shipManager.playerControlled )
        {
            // Apply movement changes
            transform.position += currentDirection.normalized * currentSpeed * Time.deltaTime;
            transform.Rotate( Vector3.forward * currentRotation * Time.deltaTime, Space.World );
        }
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

            // Bye bye ship
            Destroy( gameObject );
        }
    }
}
