using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifespan;
    private float deathTime;

    [SerializeField]
    private Vector3 direction;

    private bool ticking = false;

    public void SetDirection( Vector3 dir )
    {
        direction = dir;
    }

    void OnEnable()
    {
        ticking = true;
        deathTime = Time.time + lifespan;
    }

    void LateUpdate()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;

        if( ticking )
        {
            if( Time.time > deathTime )
            {
                gameObject.SetActive( false );
            }
        }
    }
}
