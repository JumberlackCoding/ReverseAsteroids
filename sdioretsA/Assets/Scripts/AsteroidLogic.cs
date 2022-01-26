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

    [SerializeField]
    private CircleCollider2D collider;

    [SerializeField]
    private float colliderRadiusScaler;
    [SerializeField]
    private float colliderRadiusCompensator;

    [SerializeField]
    private SpriteRenderer sprenderer;

    [SerializeField]
    private bool debug = false;

    private int level = 1;

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

        /* Here comes some dumb math to make the collider scale at the same rate as the sprite
        Since the sprite changes sizes and that resets the scale from 1 -> 0.5, you can't just multiply the collider by the scale
        How would you know if this is the 1st 0.5 -> 1 iteration or the 3rd?
        We manage this by deriving the level of the sprite.  We could simply pull it from the asteroid player script but that'd be boring
        So instead we calculate it by figuring out what 2^x = height of the sprite
        The first sprite has a size of 8x8, the next 16x16 then 32x32 and so on
        So 2 to what power = 8 and 16 and 32 and 64
        Then subtract by 2 so instead of x being 3,4,5,6 it becomes 1,2,3,4
        Once we have the level, we can use that to set the radius of the collider
        We'll take the local scale x (doesn't matter which since x and y are the same) multiply by some scalar because by default it's way big
        And then multiply it by the level squared.  Next up is to finely tune it which we do by subtracting the compensator and then you have
        To divide the entire thing by the local scale x again because otherwise it'll grow exponentially instead of linearly
        Once that's all done, the collider radius should grow roughly at the same rate as the sprite
        */
        float size = sprenderer.sprite.texture.height;
        level = (int)( Mathf.Log( size ) / Mathf.Log( 2 ) ) - 2;
        collider.radius = ( ( transform.lossyScale.x * colliderRadiusScaler * level * level ) - colliderRadiusCompensator ) / transform.lossyScale.x;
    }

    public void SetDirection( Vector3 dir )
    {
        direction = dir;
    }

    public void SetSpeed( float s )
    {
        speed = s > maxSpeed ? maxSpeed : s;
    }

    public float GetSpeed()
    {
        return speed;
    }

    void OnDrawGizmos()
    {
        // This wire sphere gizmo is a useful tool to debug the size of the collider radius.  If enabled, a wire sphere should perfectly overlap the green
        // Collider circle.  The benefit here being you can enable debug on the prefab and watch the debug sphere grow in real-time without having to pause
        // And select the asteroid to see the green circle
        if( debug )
        {
            Gizmos.DrawWireSphere( transform.position, collider.radius * transform.lossyScale.x );
        }
    }
}
