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
    [SerializeField]
    private float maxRange = 0f;
    SpriteRenderer sr;
    [SerializeField] float fowardsPercentage = 0.5f;
    int strafeDir = 1;

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

    private float strafeTimer;
    [SerializeField] float strafeMinTime, strafeMaxTime, strafeCurTime;
    
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform lineTarget;
    [SerializeField] SpriteRenderer targetRenderer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        manager = GetComponent<EnemyManager>();
        sr = GetComponent<SpriteRenderer>();

        strafeCurTime = Random.Range(strafeMinTime, strafeMaxTime);
        lineRenderer.enabled = false;
        targetRenderer.enabled = false;
    }

    // Keep dist from player, circle around
    void FixedUpdate()
    {
        if (!manager.stunned && !isAttacking)
        {
            Vector2 dir = (manager.target.position - firingPoint.position).normalized;

            float distToPlayer = Vector2.Distance(manager.target.position, transform.position);

            //Forward/backward movement
            Vector2 forwardVelo = Vector2.zero;

            if (distToPlayer < minRange) //too close, away from player
            {
                forwardVelo = -dir * fowardsPercentage;
            }
            else if (distToPlayer > maxRange) //too far, closer to player
            {
                forwardVelo = dir * fowardsPercentage;
            }
            else
            {
                forwardVelo = Vector2.zero;
            }

            //Sideways movement
            Vector2 sidewaysDir = new Vector2(-dir.y, dir.x);
            Vector2 sidewaysVelo = Vector2.zero;

            strafeTimer += Time.fixedDeltaTime;
            
            if (strafeTimer > strafeCurTime)
            {
                strafeTimer = 0;
                strafeCurTime = Random.Range(strafeMinTime, strafeMaxTime);
                strafeDir *= -1;
            }

            sidewaysVelo = sidewaysDir * strafeDir * (1 - fowardsPercentage);

            Vector2 totalVelo = (forwardVelo + sidewaysVelo).normalized * speed;
            rb.velocity = totalVelo;

            if (dir.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (dir.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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
                    
                    lineRenderer.enabled = true;
                    targetRenderer.enabled = true;

                    Aim();
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

                        lineRenderer.enabled = false;
                        targetRenderer.enabled = false;
                    }
                }
                else
                {
                    if (!shotEgg)
                    {
                        //Aim
                        Aim();
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

    void Aim()
    {
        Vector2 dir = (manager.target.position - firingPoint.position).normalized;

        int layerMask = LayerMask.GetMask("Player", "Walls", "OuterWalls", "Door");
        RaycastHit2D rayHit = Physics2D.Raycast(firingPoint.position, dir, Mathf.Infinity, layerMask);
        
        lineTarget.transform.position = rayHit.point;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, firingPoint.position);
        lineRenderer.SetPosition(1, lineTarget.transform.position);
    }
}
