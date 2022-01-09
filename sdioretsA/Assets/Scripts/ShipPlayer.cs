using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPlayer : MonoBehaviour
{
    [SerializeField]
    private bool playerControlled = false;

    [SerializeField]
    private EdgeLogic edgeLogic;

    [SerializeField]
    private KeyCode forward;
    [SerializeField]
    private KeyCode backward;
    [SerializeField]
    private KeyCode spinLeft;
    [SerializeField]
    private KeyCode spinRight;
    [SerializeField]
    private KeyCode brake;
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

    [SerializeField]
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
                currentDirection = ( currentDirection + ( transform.up * Time.deltaTime * 5 ) ).normalized;
            }
            if( Input.GetKey( backward ) )
            {
                currentSpeed = Mathf.Clamp( currentSpeed - ( moveRate * Time.deltaTime ), -maxSpeed, maxSpeed );
                currentDirection = ( currentDirection + ( transform.up * Time.deltaTime * 5 ) ).normalized;
            }
            if( Input.GetKey( spinLeft ) )
            {
                currentRotation = Mathf.Clamp( currentRotation + ( rotRate * Time.deltaTime ), -maxRotation, maxRotation );
            }
            if( Input.GetKey( spinRight ) )
            {
                currentRotation = Mathf.Clamp( currentRotation - ( rotRate * Time.deltaTime ), -maxRotation, maxRotation );
            }
            if( Input.GetKey( brake ) )
            {
                if( currentSpeed > 0 )
                {
                    currentSpeed -= ( moveRate * Time.deltaTime );
                }
                else if( currentSpeed < 0 )
                {
                    currentSpeed += ( moveRate * Time.deltaTime );
                }

                if( currentRotation > 0 )
                {
                    currentRotation -= ( rotRate * Time.deltaTime );
                }
                else if( currentRotation < 0 )
                {
                    currentRotation += ( rotRate * Time.deltaTime );
                }

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
        }
    }

    void LateUpdate()
    {
        // Apply movement changes
        transform.position += currentDirection.normalized * currentSpeed;
        transform.Rotate( Vector3.forward * currentRotation, Space.World );
    }

    void OnTriggerEnter2D( Collider2D col )
    {
        if( col.gameObject.tag == "Border" )
        {
            edgeLogic.ResetEdgeUse();

            Destroy( gameObject );
        }
    }
}
