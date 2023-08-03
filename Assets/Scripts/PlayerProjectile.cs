using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour //Player ranged projectile
{

    public float speed;
    public Rigidbody2D rb;

    public Vector2 v;

    private PlayerManager t;

    public int angle;

    public int power;

    public float cosineTest, sineTest;
    private PlayerStats stats;
    bool hit = false;

    [SerializeField]
    bool getPowerFromPlayer = true;
    Animator myAnim;

    AudioSource audioSource;
    public AudioClip[] gunShot;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        int gunShotIndex = Random.Range(0, gunShot.Length);
        audioSource.clip = gunShot[gunShotIndex];
        audioSource.Play();

        myAnim = GetComponent<Animator>();
        stats = FindObjectOfType<PlayerStats>();

        if (gameObject.name == "PlayerProjectile(Clone)")
        {
            myAnim.Play("NormalAttack");
        }

        if (gameObject.name == "PlayerProjectileFire(Clone)")
        {
            myAnim.Play("FireAttack");
        }

        if (gameObject.name == "PlayerProjectileIce(Clone)")
        {
            myAnim.Play("IceAttack");
        }

        if (gameObject.name == "PlayerProjectilePoison(Clone)")
        {
            myAnim.Play("PoisonAttack");
        }

        if (getPowerFromPlayer)
        {
            power = stats.getRangedDamage();
        }

        rb = GetComponent<Rigidbody2D>();
        

        t = FindObjectOfType<PlayerManager>();

        // angle = t.angle;
        

        // v = new Vector2(Mathf.Cos(angle * Mathf.PI/180),Mathf.Sin(angle* Mathf.PI/180));

        // rb.velocity = speed * v; 
        
    }


    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        if(hitInfo.tag == "Enemy" || hitInfo.name == "Walls" || hitInfo.tag == "Door" || hitInfo.name == "Layout Walls" || hitInfo.name == "Chest" || hitInfo.name == "Totem") // test wall collider 
        {
            if (!hit)
            {
                hit = true;

                Destroy(gameObject);

                if(hitInfo.tag == "Enemy")
                {
                    hitInfo.GetComponent<EnemyManager>().Damage(power);
                }

                if (hitInfo.tag == "Chest") {
                    hitInfo.GetComponent<ChestManager>().Damage();
                }

                if (hitInfo.tag == "Totem")
                {
                    hitInfo.GetComponent<TotemController>().Damage(power);
                }
            }
        }
    }
    
}
