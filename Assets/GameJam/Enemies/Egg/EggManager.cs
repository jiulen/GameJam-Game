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
    float spawnImmuneTimer;
    [SerializeField] float attackTime;
    float attackTimer;

    [SerializeField] float atkLaunchMultiplier = 1;

    bool spawnImmune = true;

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
    }

    // If player is within range then the enemy will follow the player
    void FixedUpdate()
    {
        if (!manager.stunned && !isAttacking && !spawnImmune)
        {
            float distToPlayer = Vector2.Distance(manager.target.position, transform.position);

            if (distToPlayer > attackRange)
            {
                Vector2 dir = (manager.target.position - transform.position).normalized;
                rb.velocity = dir * speed;

                if (dir.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (dir.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
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
                manager.StopInvincibility();
                spawnImmune = false;
            }
        }
        else
        {
            if (!manager.stunned)
            {
                float distToPlayer = Vector2.Distance(manager.target.position, transform.position);

                if (!isAttacking)
                {
                    //animator.Play("BunnyWalkAnimation");

                    if (distToPlayer <= attackRange)
                    {
                        isAttacking = true; 
                        rb.velocity *= atkLaunchMultiplier;
                    }
                }
                else
                {
                    attackTimer += Time.deltaTime;
                    //animator.Play("BunnyKickAnimation");

                    if (attackTimer >= attackTime)
                    {
                        isAttacking = false;
                        attackTimer = 0;

                        //Spawn explosion
                        Instantiate(eggplosionPrefab, eggplosionPoint.transform.position + eggplosionOffset, Quaternion.identity);

                        //Destroy egg
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
