using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayer : MonoBehaviour
{
    [SerializeField]
    private bool playerControlled = false;

    [SerializeField]
    private KeyCode forward;
    [SerializeField]
    private KeyCode backward;
    [SerializeField]
    private KeyCode spinLeft;
    [SerializeField]
    private KeyCode spinRight;
    [SerializeField]
    private KeyCode fire;

    [SerializeField]
    private float moveRate;
    [SerializeField]
    private float rotRate;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxRotation;

    private float currentRotation;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private Vector3 currentDirection;

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    void Update()
    {
        if( playerControlled )
        {
            if( Input.GetKey( forward ) )
            {
                currentSpeed = Mathf.Clamp( currentSpeed + ( moveRate * Time.deltaTime ), 0, maxSpeed );
                currentDirection += transform.up * currentSpeed * Time.deltaTime;
            }
            if( Input.GetKey( backward ) )
            {
                currentSpeed = Mathf.Clamp( currentSpeed - ( moveRate * Time.deltaTime ), 0, maxSpeed );
                currentDirection -= currentDirection * currentSpeed * Time.deltaTime;
            }
            if( Input.GetKey( spinLeft ) )
            {
                currentRotation = Mathf.Clamp( currentRotation + ( rotRate * Time.deltaTime ), -maxRotation, maxRotation );
            }
            if( Input.GetKey( spinRight ) )
            {
                currentRotation = Mathf.Clamp( currentRotation - ( rotRate * Time.deltaTime ), -maxRotation, maxRotation );
            }
        }
    }

    void LateUpdate()
    {
        // Apply movement changes
        transform.position += currentDirection;
        transform.Rotate( Vector3.forward * currentRotation, Space.World );
    }
}
