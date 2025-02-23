using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBossFlyAttack : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyManager manager;
    private DragonBossAnimator animator;
    [SerializeField]
    private List<BatBossFlyPoint> flightPoints;
    [SerializeField]
    private List<int> flightPatterns;
    [SerializeField]
    private Vector2 localPositionOffset;
    [SerializeField]
    private float attackRadius = 25.0f;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private int damage = 7;
    [SerializeField]
    private float stunTime = 0.25f;
    [SerializeField]
    private float speed = 80.0f;
    private float maxSpeed = 50.0f;
    [SerializeField]
    private float totalStartUpTime = 1.4f;
    private float startUpTimer = 0.0f;
    private int currPatternIndex = 0;
    private int currPointIndex = 0;
    private bool flying = false;
    private bool startingUp = false;
    private bool changingPhases = false;
    private bool patternsActive = false;

    private bool diving = false;
    public float diveTime = 0.5f;
    private float diveTimer = 0;
    public DragonBossShadow shadow;
    private Vector3 divePoint;
    public Vector3 diveOffset;
    //Player position and rigidbody
    public Transform playerTransform;
    public Rigidbody2D playerRb;

    private bool landing = false;
    public float landTime = 0.25f;
    private float landTimer = 0;
    public DragonBossLandingAttack landingAttack;

    public GameObject flyHitboxObj;

    public enum FLYSTATE
    {
        START,
        STOP
    }

    public FLYSTATE flyState = FLYSTATE.START;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        manager = GetComponent<EnemyManager>();
        animator = GetComponent<DragonBossAnimator>();
        flying = false;
        startingUp = false;
        changingPhases = false;
        patternsActive = false;
        diving = false;
    }

    void Update()
    {
        if (flying) 
        {
            if (startingUp) 
            {
                startUpTimer -= Time.deltaTime;

                if(startUpTimer <= 0.0f) 
                {
                    startUpTimer = 0.0f;
                    startingUp = false;
                    changingPhases = true;

                    flyState = FLYSTATE.START;
                }
            }
            else 
            {
                if(flyState == FLYSTATE.STOP) 
                {
                    currPointIndex++;
                    int nextPatternIndex = currPatternIndex + 1;
                    bool lastIndex = nextPatternIndex >= flightPatterns.Count;            
                                 

                    if(currPointIndex >= flightPoints.Count || (!lastIndex && currPointIndex >= flightPatterns[ currPatternIndex + 1])) 
                    {
                        Dive();
                    }
                    else 
                    {
                        startUpTimer = totalStartUpTime;
                        startingUp = true;
                        changingPhases = true;
                    }
                }
            }
        }
        else if (diving)
        {
            diveTimer += Time.deltaTime;
            
            
            //Play landing animation
            animator.AnimationChange(DragonState.STOMP, DragonDirection.LEFT);

            if (diveTimer >= diveTime)
            {
                //Finish dive                
                diving = false;
                diveTimer = 0;
                shadow.fixedShadow = false;

                landTimer = 0;
                landing = true;

                //start landing, show effect and deal damage
                landingAttack.StartAttack();
            }
        }
        else if (landing)
        {
            landTimer += Time.deltaTime;

            //Play landing animation
            animator.AnimationChange(DragonState.STOMP, DragonDirection.LEFT);

            if (landTimer >= landTime)
            {
                //finish landing   
                
                GetComponent<DragonBossManager>().attacking = false;

                landing = false;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (flying)
        {
            if (startingUp && changingPhases)
            {
                rb.velocity = Vector2.zero;
                transform.position = flightPoints[currPointIndex].transform.position;
                UpdateAnimation();
                changingPhases = false;
            }

            if (!startingUp)
            {
                if (changingPhases)
                {
                    rb.velocity = flightPoints[currPointIndex].GetFlyDirection() * speed * manager.slowMultiplier;
                    changingPhases = false;
                }
            }
        }
    }

    public void StartFlying(int patternIndex, float flySpeed, float waitTime) 
    {
        speed = flySpeed;
        totalStartUpTime = waitTime;

        //Shadow stuff
        shadow.shadowSizeTarget = 0.1f;
        shadow.shadowSizeChangeDuration = totalStartUpTime;

        ActivatePatterns();

        if (patternIndex < 0)
        {
            currPatternIndex = 0;
        }
        else if (patternIndex >= flightPatterns.Count) 
        {
            currPatternIndex = flightPatterns.Count - 1;
        }
        else 
        {
            currPatternIndex = patternIndex;
        }

        currPointIndex = flightPatterns[currPatternIndex];
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        startUpTimer = totalStartUpTime;
        startingUp = true;
        changingPhases = true;
        flying = true;

        shadow.ShowShadow(false);
        flyHitboxObj.SetActive(true);
    }

    private void Dive() 
    {
        StopFlying();
        diving = true;
        diveTimer = 0;

        //Find player's position to dive
        Vector3 playerVelocity = playerRb.velocity;
        divePoint = playerTransform.position + diveOffset;
        
        transform.position = divePoint;

        //Shadow stuff
        shadow.fixedShadow = true;
        shadow.transform.position = divePoint + shadow.shadowOffset;
        shadow.shadowSizeTarget = 1;
        shadow.shadowSizeChangeDuration = diveTime / 2;
        shadow.ShowShadow(true);

        landTimer = 0;
        
        flyHitboxObj.SetActive(false);
    }

    public void StopFlying() 
    {
        flying = false;
        startingUp = false;
        changingPhases = false;
        startUpTimer = 0.0f;
        currPatternIndex = 0;
        currPointIndex = 0;
        rb.velocity = Vector2.zero;
        rb.isKinematic = false;
    }

    public bool IsFlying() 
    {
        return flying || diving;
    }

    public int FlightPatternListSize() 
    {
        return flightPatterns.Count;
    }

    public void ActivatePatterns() 
    {
        if (!patternsActive) 
        {   
            foreach (BatBossFlyPoint point in flightPoints) 
            {
                point.Activate();
            }
            
        }

        patternsActive = true;
    }

    private void UpdateAnimation()
    {
        float angle = Mathf.Atan2(flightPoints[currPointIndex].GetFlyDirection().y, flightPoints[currPointIndex].GetFlyDirection().x) * Mathf.Rad2Deg;
        angle += 180; //since sprite is facing left, turn 180 to make face right
        animator.AnimationChange(DragonState.FLY, DragonDirection.ANY, 1, angle);
    }
}
