using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAI : MonoBehaviour
{
    // ----- EdgeLogic, Ship Manager, and Bullet Manager -----
    [SerializeField]
    private EdgeLogic edgeLogic;

    [SerializeField]
    private ShipManager shipManager;

    [SerializeField]
    private BulletManager bulletManager;

    // ----- Movement Buttons -----
    [SerializeField]
    private KeyCode forward;

     [SerializeField]
    private KeyCode left;

     [SerializeField]
    private KeyCode right;

    [SerializeField]
    private KeyCode autoStop;

    [SerializeField]
    private Rigidbody2D playerBody;

    // ----- Various movement related variables -----
    [SerializeField]
    private float forwardThrust;     // Translational acceleration (m/s^2)
    [SerializeField]
    private float turnThrust;        // Rotational acceleration (rad/sec^2)
    [SerializeField]
    private float maxSpeed;          // Max Velocty (m/s^2)
    [SerializeField]
    private float maxRotation;       // Max Rotation (rad/s^2)


    // Non Serialized Variables
    private Vector2 stopDirection;
    private float stopAngle;        // radians


    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }


    // Update With Physics Engine
    void FixedUpdate()
    {

            // ----- Apply Thrust -----
            if( Input.GetKey( forward ) & playerBody.velocity.magnitude < maxSpeed)
            {
                playerBody.AddForce(transform.up * forwardThrust, ForceMode2D.Force);
                Debug.Log( transform.up * forwardThrust);
            }

            // ----- Turn Left -----
            if( Input.GetKey( left ) & playerBody.angularVelocity < maxRotation)
            {
                playerBody.AddTorque(playerBody.inertia * turnThrust);
                Debug.Log(playerBody.inertia * turnThrust);
            }

            // ----- Turn Right -----
            if( Input.GetKey( right ) & playerBody.angularVelocity > (-1 * maxRotation))
            {
                playerBody.AddTorque(playerBody.inertia * turnThrust * -1);
                Debug.Log(playerBody.inertia * turnThrust * -1);
            }

            // ----- Auto Stop The Ship -----
            if( Input.GetKey( autoStop ) )
            {
                // First we need find which way the ship is moving and set the desired diretion as the opposite
                stopDirection = -1 * playerBody.velocity.normalized;
                stopAngle = Mathf.Atan2(stopDirection.y,stopDirection.x);
                Debug.Log(stopAngle);
                Debug.Log(playerBody.rotation);
                Debug.Log(playerBody.rotation * Mathf.Deg2Rad);
                

                // If we are spinning right fast enough that we cannot stop in under half a rotation, slow down the rotation
                if (playerBody.angularVelocity > 11111) 
                {

                }

                // If we are spinning left fast enough that we cannot stop in under half a rotation, slow down the rotation
                else if (playerBody.angularVelocity < -11111)
                {

                }

                // Otherwise we are spinning slow enough. Use state error feedback controller to stop rotation in desired direction
                else
                {
                    playerBody.AddTorque(playerBody.inertia * (stopAngle - playerBody.rotation * Mathf.Deg2Rad));
                    Debug.Log(stopAngle - playerBody.rotation * Mathf.Deg2Rad);
                }
                
                
                // Once we are pointing in the right direction and not spinning, start thrusting until speed is zero
                //if( playerBody.velocity.magnitude > 0 )
                //{
                //    playerBody.AddForce(transform.up * forwardThrust, ForceMode2D.Force);
                //    Debug.Log( transform.up * forwardThrust);
                //D}


            }

    }



    void LateUpdate()
    {
        
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
