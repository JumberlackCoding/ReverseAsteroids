using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLogic : MonoBehaviour
{
    // Movement related variables to be set in the inspector
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float maxRotationSpeed;
    [SerializeField]
    private float rotationValue;

    [SerializeField]
    private Vector3 direction;

    void Start()
    {
        // Apply a random spin to the asteroid
        // Get rate of spin
        rotationValue = Random.Range( 15f, maxRotationSpeed );
        // Get directon of spin
        rotationValue = Random.Range( 0, 2 ) % 2 == 0 ? -rotationValue : rotationValue;
    }

    void LateUpdate()
    {
        // Apply movement changes
        transform.position += direction.normalized * speed * Time.deltaTime;
        transform.Rotate( new Vector3( 0f, 0f, rotationValue * Time.deltaTime ), Space.World );
    }

    public void SetDirection( Vector3 dir )
    {
        Debug.Log( "Direction = " + dir );
        direction = dir;
    }

    public void SetSpeed( float s )
    {
        speed = s;
    }
}
