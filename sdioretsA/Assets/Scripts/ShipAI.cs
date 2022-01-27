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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( !shipManager.playerControlled )
        {

            // Set Rotation

            // Set Speed

        }
    }

    void LateUpdate()
    {
        if( !shipManager.playerControlled )
        {
            // Apply movement changes
            transform.position += currentDirection.normalized * currentSpeed * Time.deltaTime;
            transform.Rotate( Vector3.forward * currentRotation * Time.deltaTime, Space.World );
        }
    }
}
