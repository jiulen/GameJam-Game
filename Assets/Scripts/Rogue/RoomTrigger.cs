using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RoomTrigger : MonoBehaviour
{
    RoomManager room;
    DoorManager door;

    bool enteredBefore;
    public GameObject[] enemyPrefabs;
    public Transform[] enemySpawnPoints;
    public bool noBlockBuff;
    private TilemapCollider2D tilemapCollider2D;
    PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        room = GetComponentInParent<RoomManager>();
        door = GetComponentInParent<DoorManager>();
        tilemapCollider2D = null;
        noBlockBuff = false;
        enteredBefore = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setBuffActive(TilemapCollider2D tilemapCollider2D)
    {
        Debug.Log("yuzuruuuuu");
        noBlockBuff = !noBlockBuff;
        Debug.Log("yuzuruuuuu2" + noBlockBuff);
        this.tilemapCollider2D = tilemapCollider2D;
        Debug.Log("yuzuruuuuu3" + this.tilemapCollider2D);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool noBlockBuff = playerStats.getNoBlockBuff();
        GameObject layoutWalls = playerStats.getLayoutWalls();
        TilemapCollider2D tilemapCollider2D = playerStats.getTilemapCollider2D();
        Debug.Log("cOLLIED");
        Debug.Log(noBlockBuff);

        if (noBlockBuff)
        {
            Collider2D collider = collision.collider;
            Collider2D otherCollider = collision.otherCollider;
            Debug.Log(collider); // walls
            Debug.Log(otherCollider); // enemies

            // Access the game objects associated with the colliders
            GameObject gameObjectA = collider.gameObject;
            GameObject gameObjectB = otherCollider.gameObject;

            if (layoutWalls && gameObjectB.CompareTag("Enemy"))
            {
                Debug.Log("Enemy collided with: " + gameObjectB.name);
                // Perform actions specific to enemy colliding with gameObjectB
                tilemapCollider2D.enabled = true;
            }

            if (layoutWalls && gameObjectB.CompareTag("Player"))
            {
                Debug.Log("Enemy collided with: " + gameObjectB.name);
                // Perform actions specific to enemy colliding with gameObjectB
                tilemapCollider2D.enabled = false;
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("cOLLIED");
    //    Debug.Log(noBlockBuff);
    //    if(noBlockBuff)
    //    {
    //        if (collision.gameObject.CompareTag("Player"))
    //        {
    //            // Player collided with something
    //            if (collision.collider.gameObject.CompareTag("Wall"))
    //            {
    //                // Player collided with a wall
    //                Debug.Log("Player collided with a wall");
    //                // Perform actions specific to player colliding with a wall
    //            }
    //            else if (collision.collider.gameObject.CompareTag("Enemy"))
    //            {
    //                // Player collided with an enemy
    //                Debug.Log("Player collided with an enemy");
    //                // Perform actions specific to player colliding with an enemy
    //            }
    //        }
    //        else if (collision.gameObject.CompareTag("Enemy"))
    //        {
    //            // Enemy collided with something
    //            if (collision.collider.gameObject.CompareTag("Wall"))
    //            {
    //                // Enemy collided with a wall
    //                Debug.Log("Enemy collided with a wall");
    //                // Perform actions specific to enemy colliding with a wall
    //            }
    //        }
    //    }

    //}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (enteredBefore == false)
            {
                playerStats = collision.gameObject.GetComponent<PlayerStats>();
                SpawnEnemyPrefabs();
                enteredBefore = true;
                if (playerStats.currentLevel > 0)
                {
                    SpawnAdditionalEnemies();
                }
            }
            room.setPlayerInside(true);
            door.setClosed(true);
        }


        //Debug.Log("cOLLIED");
        //Debug.Log(noBlockBuff);
        //if (noBlockBuff)
        //{
        //    GameObject otherObject = collision.gameObject;

        //    if (otherObject.CompareTag("Player"))
        //    {
        //        Debug.Log("Player collided with walls");
        //        // Perform actions specific to player colliding with walls
        //        tilemapCollider2D.enabled = false;
        //    }

        //    if (otherObject.CompareTag("Enemy"))
        //    {
        //        Debug.Log("Enemy collided with walls");
        //        // Perform actions specific to enemy colliding with walls
        //        tilemapCollider2D.enabled = true;
        //    }
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            room.setPlayerInside(false);
        }
        
    }

    void SpawnEnemyPrefabs()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemyIndex], enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
        }
    }

    void SpawnAdditionalEnemies()
    {
        int additionalEnemies = playerStats.currentLevel;
        if (additionalEnemies > 4)
        {
            additionalEnemies = 4;
        }
        for (int i = 0; i <= additionalEnemies; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemyIndex], enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
        }
    }

}
