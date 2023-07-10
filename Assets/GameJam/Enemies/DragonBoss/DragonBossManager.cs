using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonBossManager : MonoBehaviour
{
    private Transform playerTarget;
    private DragonBossLaserAttack laserAttack;
    private DragonBossFireballAttack fireballAttack;
    private DragonBossFlyAttack flyAttack;
    private DragonBossTailAttack tailAttack;
    private EnemyManager manager;
    [SerializeField]
    private bool active = false;
    
    public bool attacking = false;
    
    private float attackTimer = 0;
    [SerializeField] private float attackTime = 2.0f;
    [SerializeField] private float waitTime = 1.0f;
    private float waitTimer = 0.0f;
    private Rigidbody2D rb;
    private DragonBossAnimator animator;
    [SerializeField]
    private float walkSpeed = 6.5f;
    public float walkSpeedMultiplier = 1;
    private int phaseNum = 1;
    private bool newPhase = true;
    private bool fireballed = false;
    [SerializeField] Slider healthBar;
    private Vector2 laserMoveStartPos;
    public bool lockedTarget = false;
    public Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;

        laserAttack = GetComponent<DragonBossLaserAttack>();
        fireballAttack = GetComponent<DragonBossFireballAttack>();
        flyAttack = GetComponent<DragonBossFlyAttack>();
        tailAttack = GetComponent<DragonBossTailAttack>();

        manager = GetComponent<EnemyManager>();
        animator = GetComponent<DragonBossAnimator>();
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) {
            return;
        }

        healthBar.value = manager.hp;

        //check phase
        if ((float)manager.hp / (float)manager.startHp <= 0.1f && phaseNum < 4) //10% hp left
        {
            phaseNum = 4;
            newPhase = true;
        }
        else if ((float)manager.hp / (float)manager.startHp <= 0.25f && phaseNum < 3) //25% hp left
        {
            phaseNum = 3;
            newPhase = true;
        }
        else if ((float)manager.hp / (float)manager.startHp <= 0.5f && phaseNum < 2) //50% hp left
        {
            phaseNum = 2;
            newPhase = true;
        }
        
        waitTimer += Time.deltaTime;

        //Walk
        if ((Vector2.Distance(playerTarget.position, transform.position) < 10 && waitTimer < waitTime) || attacking)
        {
            if (attacking && !lockedTarget) //lockedTarget for laser attack 
            { 
                return;
            }

            if (!lockedTarget)
            {
                targetPos = playerTarget.position;
            }

            if (!(lockedTarget && AlmostEqual(targetPos, transform.position, 1e-3f))) //check if reached targetPos for laser atk
            {
                Debug.Log("SUS");
                Vector2 walkDirection = (Vector2)transform.position - targetPos;
                walkDirection.Normalize();
                if (lockedTarget) walkDirection *= -1;
                rb.velocity = walkSpeed * walkSpeedMultiplier * walkDirection;

                //animation
                if (!lockedTarget)
                {
                    if (rb.velocity.x < 0)
                    {
                        animator.AnimationChange(DragonState.MOVE, DragonDirection.LEFT);
                    }
                    else
                    {
                        animator.AnimationChange(DragonState.MOVE, DragonDirection.RIGHT);
                    }
                }

                //Check if exceed target position
                if (Vector2.Distance(transform.position, laserMoveStartPos) > Vector2.Distance(targetPos, laserMoveStartPos) && lockedTarget)      
                {
                    rb.isKinematic = true;
                    transform.position = targetPos;
                    rb.isKinematic = false;
                }
            }
        }
        else {
            if (attacking) 
            { 
                return;
            }
            
            rb.velocity = Vector2.zero;
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackTime)
            {
                attackTimer = 0;
                waitTime = 0;
                chooseAttack();
            }
            else {                
                Debug.Log("Boss is idling");
            }
        }
    }

    public void Activate() 
    {
        active = true;        

        healthBar.gameObject.SetActive(true);
        healthBar.maxValue = manager.startHp;
        healthBar.value = healthBar.maxValue;

        flyAttack.ActivatePatterns();
        chooseAttack();
    }

    public void chooseAttack() {
        //fly attack when enter new phase
        int choose = Random.Range(0, 3);

        if (newPhase)
        {
            choose = 4;
            newPhase = false;
        }
        else if (!fireballed && phaseNum >= 2)
        {
            choose = 3;
        }   

        attacking = true;

        //Priority : Fly->Fireball->Tail/Laser
        //Laser twice as common as Tail
        //Fly happens at start of each phase
        //Fireball happens at start of phase 2

        switch (choose) {
            case 0:
            case 1:
                //Laser attack
                targetPos = laserAttack.StartAttack();
                lockedTarget = true;
                walkSpeedMultiplier = 1 + 0.25f * phaseNum;
                laserMoveStartPos = transform.position;
                break;
            case 2:
                //Tail attack
                tailAttack.StartAttack(4 * phaseNum);
                break;
            case 3:
                //Fireball attack
                fireballed = true;
                fireballAttack.StartAttack();
                break;
            case 4:
                //Fly attack
                flyAttack.StartFlying(Random.Range(0, flyAttack.FlightPatternListSize()));
                break;
        }

    }

    public static bool AlmostEqual(Vector2 v1, Vector2 v2, float tolerance)
    {
        return Mathf.Abs(Vector2.Distance(v1, v2)) <= tolerance;
    }
}
