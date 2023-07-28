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
    GameObject eggObj;
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

    bool launched = false;

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

            Vector2 totalVelo = (forwardVelo + sidewaysVelo).normalized * speed * manager.slowMultiplier;
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
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            animator.Play("Chicken_Attack");
            
            if (attackTimer > shootTime)
            {
                if (!shotEgg)
                {
                    shotEgg = true;
                    Shoot();
                }
                else
                {
                    if (!launched) Aim();
                }
            }

            if (attackTimer > attackTime)
            {
                isAttacking = false;
                shotEgg = false;
                animator.Play("Chicken_Run", sr.sortingLayerID, 0);
                attackTimer = 0;
            }
        }

        if (!manager.stunned)
        {
            if (!isAttacking)
            {
                animator.Play("Chicken_Run");
                cooldownTimer += Time.deltaTime;

                if (cooldownTimer > attackCooldown)
                {
                    isAttacking = true;              
                    rb.velocity = Vector2.zero;
                    cooldownTimer = 0;

                    animator.Play("Chicken_Attack", sr.sortingLayerID, 0);
                }
            }
        }
    }

    public void DestroyEgg()
    {
        if (eggObj != null) Destroy(eggObj);
    }

    void Shoot()
    {
        Vector2 dir = (manager.target.position - firingPoint.position).normalized;
        eggObj = Instantiate(eggPrefab, firingPoint.position, Quaternion.identity);
        eggObj.GetComponent<ExplodingProjectile>().chickenManager = this;
        eggObj.GetComponent<ExplodingProjectile>().manager = manager;

        eggObj.GetComponent<ExplodingProjectile>().futureVelo = dir * shootSpeed;

        lineRenderer.enabled = true;
        targetRenderer.enabled = true;

        launched = false;
    }

    public void Launch()
    {
        Vector2 dir = (manager.target.position - firingPoint.position).normalized;
        eggObj.GetComponent<ExplodingProjectile>().futureVelo = dir * shootSpeed;
        eggObj = null;

        lineRenderer.enabled = false;
        targetRenderer.enabled = false;

        launched = true;
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

        if (eggObj != null) eggObj.GetComponent<ExplodingProjectile>().futureVelo = dir * shootSpeed;
    }
}
