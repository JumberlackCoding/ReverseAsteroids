using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLogic : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3 direction;

    void LateUpdate()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
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
