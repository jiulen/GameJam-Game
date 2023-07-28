using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyManager manager;
    
    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float attackRange = 0f;
    SpriteRenderer sr;

    [SerializeField]
    Vector3 eggplosionOffset;
    [SerializeField]
    Transform eggplosionPoint;
    [SerializeField]
    GameObject eggplosionPrefab;

    bool isAttacking = false;
    [SerializeField] float spawnImmuneTime;
    float spawnImmuneTimer = 0;
    [SerializeField] float attackTime;
    float attackTimer = 0;

    [SerializeField] float atkLaunchMultiplier = 1;

    bool spawnImmune = true;

    [SerializeField] Material[] materialsList; //first is default, second is white
    bool isFlashing = false;
    [SerializeField] float flashTime;
    float flashTimer = 0;
    [SerializeField] AnimationCurve spawnJumpCurve;
    Vector3 startPos;
    CapsuleCollider2D collider;

    void Awake()
    {
        manager = GetComponent<EnemyManager>();
        manager.TriggerInvincibility();
        spawnImmune = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        startPos = transform.position;

        collider.isTrigger = true;
    }

    // If player is within range then the enemy will follow the player
    void FixedUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (!manager.stunned && !isAttacking && !spawnImmune)
        {
            Vector2 dir = (manager.target.position - transform.position).normalized;
            rb.velocity = dir * speed * manager.slowMultiplier;

            if (dir.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (dir.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnImmune)
        {
            spawnImmuneTimer += Time.deltaTime;
            if (spawnImmuneTimer >= spawnImmuneTime)
            {
                spawnImmuneTimer = spawnImmuneTime;
                manager.StopInvincibility();
                spawnImmune = false;
            }

            transform.position = new Vector3(startPos.x, startPos.y + spawnJumpCurve.Evaluate((spawnImmuneTimer)), startPos.z);
        }
        else
        {
            if (!manager.stunned)
            {
                float distToPlayer = Vector2.Distance(manager.target.position, transform.position);

                if (!isAttacking)
                {
                    if (distToPlayer <= attackRange)
                    {
                        FollowPlayer();

                        isAttacking = true; 
                        rb.velocity *= atkLaunchMultiplier;
                        animator.speed = 0;

                        if (rb.velocity.x > 0)
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                        }
                        else if (rb.velocity.x < 0)
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                        }
                    }
                }
                else
                {
                    flashTimer += Time.deltaTime;

                    if (flashTimer >= flashTime)
                    {
                        if (isFlashing) 
                        {
                            sr.material = materialsList[0]; 
                            flashTime /= 1.1f;
                        }
                        else sr.material = materialsList[1];

                        isFlashing = !isFlashing;
                    }

                    attackTimer += Time.deltaTime;

                    if (attackTimer >= attackTime)
                    {
                        isAttacking = false;
                        attackTimer = 0;

                        //Spawn explosion
                        GameObject explosion = Instantiate(eggplosionPrefab, eggplosionPoint.transform.position + eggplosionOffset, Quaternion.identity);
                        
                        CircleCollider2D explosionCollider = explosion.GetComponent<CircleCollider2D>();
                        CapsuleCollider2D playerCollider = manager.playerObj.GetComponent<CapsuleCollider2D>();

                        //check if player in explosion range
                        if (Physics2D.IsTouching(explosionCollider, playerCollider) && !manager.playerObj.GetComponent<HealthManager>().isInvincible())
                        {
                            manager.hitPlayer = true;
                        }

                        //Destroy egg
                        manager.Damage(1, 0);
                    }
                }
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Walls" || collision.collider.name == "Layout Walls")
        {
            if (isAttacking)
            {
                attackTimer = attackTime;
                Debug.Log("Collided1 : " + collision.collider.name);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox")  || collision.name == "Walls" || collision.name == "Layout Walls")
        {
            if (isAttacking)
            {
                attackTimer = attackTime;
                Debug.Log("Collided2 : " + LayerMask.LayerToName(collision.gameObject.layer));
            }
        }
    }
}
