using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyManager : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private EnemyManager manager;    

    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float attackRange = 0f; 
    [SerializeField]
    private float minRange = 0f;
    SpriteRenderer sr;

    //For attack
    bool isAttacking = false;
    [SerializeField]
    private GameObject slashObj;
    private Animator slashAnimator;

    private float attackTimer;
    [SerializeField]
    private float slashTime;
    [SerializeField]
    private float attackTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        manager = GetComponent<EnemyManager>();
        sr = GetComponent<SpriteRenderer>();

        slashAnimator = slashObj.GetComponent<Animator>();
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
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else if (dir.x < 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.stunned)
        {
            float distToPlayer = Vector2.Distance(manager.target.position, transform.position);

            if (!isAttacking)
            {
                animator.Play("BunnyWalkAnimation");

                if (distToPlayer < attackRange)
                {
                    isAttacking = true;                    
                }
            }
            else
            {
                attackTimer += Time.deltaTime;
                animator.Play("BunnyKickAnimation");
                
                if (attackTimer > slashTime)
                {
                    if (!slashObj.activeSelf)
                    {
                        slashObj.SetActive(true);
                    }

                    slashAnimator.Play("Slash3");
                }

                if (attackTimer > attackTime)
                {
                    isAttacking = false;
                    animator.Play("BunnyWalkAnimation", sr.sortingLayerID, 0);
                }
            }
        }
    }
}
