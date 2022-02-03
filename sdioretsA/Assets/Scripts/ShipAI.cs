using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAI : MonoBehaviour
{
    // EdgeLogic script reference for handling looping of the ship around edges
    [SerializeField]
    private EdgeLogic edgeLogic;

    [SerializeField]
    private ShipManager shipManager;

    [SerializeField]
    private BulletManager bulletManager;

    [SerializeField]
    private KeyCode forward;

     [SerializeField]
    private KeyCode left;

     [SerializeField]
    private KeyCode right;

    [SerializeField]
    private Rigidbody2D playerBody;

    // Various movement related variables
    // Translational acceleration
    [SerializeField]
    private float forwardThrust;
    // Rotational acceleration
    [SerializeField]
    private float turnThrust;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxRotation;

    // All the current movement values
    [SerializeField]
    private float currentRotation;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private Vector2 currentDirection;


    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        

            // Apply Thrust
            if( Input.GetKey( forward ) )
            {
                // Mathf.Clamp will take the value of the first argument and clamp it within the minimum and maximum values set by the 2nd and 3rd params respectively
                playerBody.AddForce(transform.up*forwardThrust, ForceMode2D.Force);
                Debug.Log( transform.up*forwardThrust);
            }
            // Turn Left
            if( Input.GetKey( left ) )
            {
                // Mathf.Clamp will take the value of the first argument and clamp it within the minimum and maximum values set by the 2nd and 3rd params respectively
                playerBody.AddTorque((-5 * Mathf.Deg2Rad) * playerBody.inertia);
            }
            // Turn Right
            if( Input.GetKey( left ) )
            {
                // Mathf.Clamp will take the value of the first argument and clamp it within the minimum and maximum values set by the 2nd and 3rd params respectively
                playerBody.AddTorque((5 * Mathf.Deg2Rad) * playerBody.inertia);
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
