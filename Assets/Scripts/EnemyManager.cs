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
    private bool noBlockBuff = false;
    [SerializeField]
    private bool roomIndependent = false;
    private Collision2D wallCollision;

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


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    noBlockBuff = playerStats.getNoBlockBuff();
    //    GameObject layoutWalls = playerStats.getLayoutWalls();
    //    TilemapCollider2D tilemapCollider2D = playerStats.getTilemapCollider2D();
    //    Debug.Log("cOLLIED----enemy");
    //    Debug.Log(noBlockBuff);

    //    if (noBlockBuff)
    //    {
    //        //Collider2D collider = collision.collider;
    //        Collider2D otherCollider = collision.otherCollider;
    //        //Debug.Log(collider); // walls
    //        //Debug.Log(otherCollider); // enemies

    //        // Access the game objects associated with the colliders
    //        //GameObject gameObjectA = collider.gameObject;
    //        GameObject gameObjectB = otherCollider.gameObject;

    //        if (layoutWalls && gameObjectB.CompareTag("Enemy"))
    //        {
    //            Debug.Log("Enemy collided with: " + gameObjectB.name);
    //            // Perform actions specific to enemy colliding with gameObjectB
    //            tilemapCollider2D.enabled = true;
    //        }
    //    }
    //}



    //// This function is called when a 2D collider exits the trigger collider of the TilemapCollider2D (wall)
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    noBlockBuff = playerStats.getNoBlockBuff();
    //    GameObject layoutWalls = playerStats.getLayoutWalls();
    //    TilemapCollider2D tilemapCollider2D = playerStats.getTilemapCollider2D();

    //    if (noBlockBuff)
    //    {
    //        Debug.Log("cOLLIED - exit");
    //        //Collider2D collider = collision.collider; // walls
    //        Collider2D otherCollider = collision.otherCollider; // enemies
    //        GameObject gameObjectB = otherCollider.gameObject;

    //        // Check if the collider is the player or enemy
    //        if (layoutWalls && gameObjectB.CompareTag("Enemy"))
    //        {
    //            Debug.Log("Enemy collided with: " + gameObjectB.name);
    //            // Handle collision exit between player and wall
    //            // For example, enable the TilemapCollider2D for the player
    //            tilemapCollider2D.enabled = true;
    //        }
    //    }
    //}
}
