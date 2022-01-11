using System.Collections;
using System.Collections.Generic;
using System;
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
    private int directionalSluggishness;

    [SerializeField]
    private float currentRotation;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private Vector3 currentDirection;

    [SerializeField]
    private GameObject bulletPreFab;
    [SerializeField]
    private GameObject[] bulletBank;
    [SerializeField]
    private int bulletBankCount;
    [SerializeField]
    private float fireDelay;
    [SerializeField]
    private float fireLast;

    void Start()
    {
        bulletBank = new GameObject[bulletBankCount];

        for( int i = 0; i < bulletBankCount; i++ )
        {
            bulletBank[i] = Instantiate( bulletPreFab, new Vector3( 100, 100, 0 ), Quaternion.identity );
            bulletBank[i].SetActive( false );
        }
    }

    void Update()
    {
        if( playerControlled )
        {
            if( Input.GetKey( forward ) )
            {
                currentSpeed = Mathf.Clamp( currentSpeed + moveRate, -maxSpeed, maxSpeed );
                currentDirection = ( ( currentDirection * directionalSluggishness ) + transform.up ).normalized;
            }
            if( Input.GetKey( backward ) )
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
        try
        {
            GameObject bullet = GetBullet();
            bullet.transform.position = transform.position;
            bullet.GetComponent<BulletLogic>().SetDirection( transform.up );
            bullet.SetActive( true );
        }
        catch( NullReferenceException e )
        {
            Debug.LogWarning( "No bullets in bank" );
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

    void OnTriggerEnter2D( Collider2D col )
    {
        if( col.gameObject.tag == "Border" )
        {
            edgeLogic.ResetEdgeUse();

            foreach( GameObject go in bulletBank )
            {
                Destroy( go );
            }

            Destroy( gameObject );
        }
    }
}
