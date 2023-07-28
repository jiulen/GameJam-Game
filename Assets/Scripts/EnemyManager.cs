using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class EnemyManager : MonoBehaviour
{
    public float stunTime;
    public bool stunned = false;
    protected bool isInvincible = false;
    [SerializeField]
    protected bool roomIndependent = false;

    public int hp, startHp = 3;

    public Transform target;

    public GameObject playerObj;

    private DoorManager doors = null;
    protected RoomTemplates room = null;
    [SerializeField] protected GameObject lootItem;
    [SerializeField] protected bool endGame = false;

    public int experienceToGive;
    protected PlayerStats playerStats;
    public SpriteRenderer spriteRenderer;
    public GameObject winScreen;

    bool fireArrowHit;
    EnemyControl enemyControl;
    FollowEnemy followEnemy;
    public GameObject explosionPrefab;

    public float slowMultiplier = 1;
    Color normalColor = Color.white; //color when not hurt
    bool hurting = false;
    Coroutine slowCoroutine = null;
    Coroutine poisonCoroutine = null;
    public GameObject TotemHealingEffect = null;

    public bool hitPlayer = false;
    public GameObject hitlessDrop;

    void Awake()
    {
        hp = startHp;
    }

    // Start is called before the first frame update
    void Start()
    {
        fireArrowHit = false;

        stunned = false;

        playerObj = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!roomIndependent)
        {
            doors = GetComponentInParent<DoorManager>();
            room = FindObjectOfType<RoomTemplates>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (doors.getClosed())
        //{
        //    //this.gameObject.SetActive(true);
            
        //    Debug.Log("Should spawn");
        //}

        if(stunTime > 0.0f){
            stunned = true;
            stunTime -= Time.deltaTime;
        }
        else   
            stunned = false;
    }

    virtual public void Damage(int p, bool poison = false)
    {
        if (!isInvincible)
        {
            hp -= p;
            if (poison) 
            {
                StartCoroutine(FlashPurple());
            }    
            else 
            {
                if (p > 0)
                {
                    StartCoroutine(FlashRed());
                }
            }
            if (hp <= 0)
            {
                if (!roomIndependent)
                {
                    room.getActiveRoom().GetComponent<DoorManager>().killEnemy();
                }

               
                if (endGame)
                {
                    StartCoroutine(GameOver());
                }
                else {
                    Instantiate(lootItem, transform.position, Quaternion.identity);
                    if (!hitPlayer)
                    {
                        Debug.Log(name + " hitless");
                        if (hitlessDrop != null)
                        {
                            Instantiate(hitlessDrop, transform.position, Quaternion.identity);
                        }                        
                    }
                    else
                    {
                        Debug.Log(name + "hitfull");
                    }
                    Destroy(gameObject);
                    playerStats.GainExperience(experienceToGive);
                }
                
            }
        }
    }

    public void Damage(int dmg, float stun) 
    {
        if (!isInvincible)
        {
            if (stun > stunTime)
            {
                stunTime = stun;
            }

            Damage(dmg);
        }
    }

    public void heal(int currentHp, int startHp, int healValue)
    {
        if (currentHp < startHp && startHp != 500) // check if got damaged, and except heal for boss
        {
            Debug.Log("I am healing up the enemies");
            Debug.Log(currentHp + "current hp before heal");
            if (TotemHealingEffect != null)
            {
                TotemHealingEffect.SetActive(true);
            }

            int afterHealHp = currentHp + healValue;
            if (afterHealHp > startHp)
            {
                hp = startHp;
            }
            else
            {
                hp = afterHealHp;
            }

            Debug.Log(hp + "current hp after heal");
        }
        else
        {
            if (TotemHealingEffect != null)
            {
                TotemHealingEffect.SetActive(false);
            }
        }
    }


public bool IsInvincible() 
    {
        return isInvincible;
    }

    public void TriggerInvincibility()
    {
        isInvincible = true;
    }

    public void StopInvincibility() 
    {
        isInvincible = false;
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3);
        winScreen.SetActive(true);
        //SceneManager.LoadScene("MainMenu1");
    }
    public IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        hurting = true;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = normalColor;
        hurting = false;
    }
    public IEnumerator FlashPurple()
    {
        spriteRenderer.color = new Color(0.5f, 0, 0.5f, 1);
        hurting = true;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = normalColor;
        hurting = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerProjectileFire(Clone)" && fireArrowHit == false)
        {
            fireArrowHit = true;
            spriteRenderer.color = Color.yellow;
            normalColor = Color.yellow;
        }
        else if (other.gameObject.name == "PlayerProjectileFire(Clone)" && fireArrowHit == true)
        {
            fireArrowHit = false;
            spriteRenderer.color = Color.white;
            normalColor = Color.white;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (other.gameObject.name == "PlayerProjectilePoison(Clone)")
        {
            if (poisonCoroutine != null) StopCoroutine(poisonCoroutine);
            poisonCoroutine = StartCoroutine(PoisonDamage());
        }
        if (other.gameObject.name == "PlayerProjectileIce(Clone)")
        {
            if (slowCoroutine != null) StopCoroutine(slowCoroutine);
            slowCoroutine = StartCoroutine(SlowEnemy());
        }
    }
    IEnumerator PoisonDamage()
    {
        float timer = 0f;
        float duration = 3f;
        float tickInterval = 1f;
        int damagePerTick = 2;
        while (timer < duration)
        {
            yield return new WaitForSeconds(tickInterval);
            Damage(damagePerTick, true);
            timer += tickInterval;
        }
    }

    IEnumerator DelayFunction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public int getHp() { return hp; }

    public int getStartHp() { return startHp; }

    IEnumerator SlowEnemy()
    {
        slowMultiplier = 0.5f;
        if (!hurting) spriteRenderer.color = Color.blue;
        normalColor = Color.blue;
        yield return new WaitForSeconds(3f);
        slowMultiplier = 1;     
        if (!hurting) spriteRenderer.color = Color.white;   
        normalColor = Color.white;
    }
}
