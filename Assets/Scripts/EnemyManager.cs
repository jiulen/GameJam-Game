using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public float stunTime;
    public bool stunned = false;
    private bool isInvincible = false;
    [SerializeField]
    private bool roomIndependent = false;

    public int hp, startHp = 3;

    public Transform target;

    private DoorManager doors = null;
    private RoomTemplates room = null;
    [SerializeField] private GameObject lootItem;
    [SerializeField] private bool endGame = false;

    public int experienceToGive;
    PlayerStats playerStats;
    SpriteRenderer spriteRenderer;
    public GameObject winScreen;

    bool fireArrowHit;
    EnemyControl enemyControl;
    FollowEnemy followEnemy;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        fireArrowHit = false;

        stunned = false;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = GameObject.FindObjectOfType<PlayerStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!roomIndependent)
        {
            doors = GetComponentInParent<DoorManager>();
            room = FindObjectOfType<RoomTemplates>();
        }
        //this.gameObject.SetActive(false);
        //this.enabled = false;

        hp = startHp;
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

    public void Damage(int p)
    {
        if (!isInvincible)
        {
            hp -= p;
            StartCoroutine(FlashRed());
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
                    Instantiate(lootItem, transform.position, transform.rotation);
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
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "PlayerProjectileFire(Clone)" && fireArrowHit == false)
        {
            fireArrowHit = true;
        }
        else if (other.gameObject.name == "PlayerProjectileFire(Clone)" && fireArrowHit == true)
        {
            fireArrowHit = false;
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (other.gameObject.name == "PlayerProjectilePoison(Clone)")
        {
            StartCoroutine(PoisonDamage());
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
            Damage(damagePerTick);
            timer += tickInterval;
        }
    }

    IEnumerator DelayFunction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
