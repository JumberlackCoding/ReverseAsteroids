using System;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // Bullet related variables
    // Basic bullet prefab
    [SerializeField]
    private GameObject bulletPreFab;
    // Bullet pool where bullets are pulled from when fired
    [SerializeField]
    public GameObject[] bulletBank;
    // Size of the bullet pool
    [SerializeField]
    private int bulletBankCount;
    // These two prevent the ship from being able to fire a bullet every single frame.  Adds a time based delay between firing bullets
    [SerializeField]
    private float fireDelay;
    private float fireLast = 0;

    // Components required for playing sounds
    // Sound clip
    [SerializeField]
    private AudioClip shipFire1;
    // Thing that will play the sound
    [SerializeField]
    private AudioSource audioPlayer;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the bullet pool and disable all of them in the hierarchy
        bulletBank = new GameObject[bulletBankCount];

        for( int i = 0; i < bulletBankCount; i++ )
        {
            bulletBank[i] = Instantiate( bulletPreFab, new Vector3( 100, 100, 0 ), Quaternion.identity );
            bulletBank[i].SetActive( false );
        }
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    public void FireBullet( Transform shipTransform )
    {
        // Attempt to get a bullet from the pool and don't blow up if they're all taken
        try
        {
            // Check if it's been long enough to fire again
            if( Time.time >= fireLast + fireDelay )
            {
                GameObject bullet = GetBullet();
                bullet.transform.position = shipTransform.position;
                bullet.GetComponent<BulletLogic>().SetDirection( shipTransform.up );
                bullet.SetActive( true );
                fireLast = Time.time;
                audioPlayer.PlayOneShot( shipFire1 );
            }
        }
        catch( NullReferenceException e )
        {
            Debug.LogWarning( "No bullets in bank " + e.Message );
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
}
