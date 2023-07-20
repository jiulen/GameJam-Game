using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyManager manager;

    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float minRange = 0f;
    SpriteRenderer sr;

    //For attack
    bool isAttacking = false;
    [SerializeField]
    private GameObject eggPrefab;
    [SerializeField]
    private Transform firingPoint;

    private float attackTimer;
    [SerializeField]
    private float shootTime;
    [SerializeField]
    private float attackTime;
    private float cooldownTimer;
    [SerializeField]
    private float attackCooldown;
    bool shotEgg = false;
    [SerializeField] float shootSpeed;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        manager = GetComponent<EnemyManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    // If player is within range then the enemy will follow the player
    void FixedUpdate()
    {
        if (!manager.stunned && !isAttacking)
        {
            float distToPlayer = Vector2.Distance(manager.target.position, transform.position);

            if (distToPlayer > minRange)
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
        if (!manager.stunned)
        {
            if (!isAttacking)
            {
                //animator.Play("BunnyWalkAnimation");
                cooldownTimer += Time.deltaTime;

                if (cooldownTimer > attackCooldown)
                {
                    isAttacking = true;              
                    rb.velocity = Vector2.zero;    
                    cooldownTimer = 0;
                }
            }
            else
            {
                attackTimer += Time.deltaTime;
                //animator.Play("BunnyKickAnimation");
                
                if (attackTimer > shootTime)
                {
                    if (!shotEgg)
                    {
                        shotEgg = true;
                        Shoot();
                    }
                }
                else
                {
                    if (!shotEgg)
                    {
                        //Aim
                    }
                }

                if (attackTimer > attackTime)
                {
                    isAttacking = false;
                    shotEgg = false;
                    //animator.Play("BunnyWalkAnimation", sr.sortingLayerID, 0);
                    attackTimer = 0;
                }
            }
        }
    }

    void Shoot()
    {
        Vector2 dir = (manager.target.position - firingPoint.position).normalized;

        GameObject egg = Instantiate(eggPrefab, firingPoint.position, Quaternion.identity);
        egg.GetComponent<Rigidbody2D>().velocity = dir * shootSpeed;
    }

    
}
