using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    // Speed of bullet
    [SerializeField]
    private float speed;

    // Variables regarding flight time
    [SerializeField]
    private float lifespan;
    private float deathTime;

    // Direction of flight
    [SerializeField]
    private Vector3 direction;

    // Booleon that is true when the bullet is active and causes time to tick towards the bullet's demise
    private bool ticking = false;

    // Function used to direct the bullet in the direction the ship is pointing
    public void SetDirection( Vector3 dir )
    {
        direction = dir;
    }

    // Called every time the bullet gameobject is enabled in the hierarchy
    void OnEnable()
    {
        ticking = true;
        // Calculate new time at which the bullet will self-deactivate
        deathTime = Time.time + lifespan;
    }

    void LateUpdate()
    {
        // Apply movement
        transform.position += direction.normalized * speed * Time.deltaTime;

        // Check whether the time has come to deactivate at which point it will be available for picking again when the ship fires next
        if( ticking )
        {
            if( Time.time > deathTime )
            {
                gameObject.SetActive( false );
            }
        }
    }

    void OnTriggerEnter2D( Collider2D col )
    {
        if( col.gameObject.tag == "Asteroid" )
        {
            Debug.Log( "Shot" );

            // Bye bye bullet
            if( col.gameObject.GetComponent<AsteroidLogic>().GetSpeed() > 0 )
            {
                gameObject.SetActive( false );
            }
        }
    }
}
