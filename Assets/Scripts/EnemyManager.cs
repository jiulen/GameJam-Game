using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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

    // Start is called before the first frame update
    void Start()
    {
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

    public void heal(int currentHp, int startHp, int healValue)
    {
        if (currentHp < startHp && startHp != 500) // check if got damaged, and except heal for boss
        {
            Debug.Log("I am healing up the enemies");
            Debug.Log(currentHp + "current hp before heal");

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


    public int getHp() { return hp; }

    public int getStartHp() { return startHp; }
}
