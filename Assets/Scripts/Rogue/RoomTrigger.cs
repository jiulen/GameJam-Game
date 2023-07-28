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
    [SerializeField]
    private TotemController totemController;
    PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        room = GetComponentInParent<RoomManager>();
        door = GetComponentInParent<DoorManager>();
        enteredBefore = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "TerrainHitBox")
        {            
            room.setPlayerInside(true);
            
            if (enteredBefore == false)
            {
                playerStats = FindObjectOfType<PlayerStats>();
                int numberOfEnemiesToSpawn = playerStats.currentLevel;
                if (numberOfEnemiesToSpawn < 1)
                {
                    numberOfEnemiesToSpawn = 1; 
                }
                StartCoroutine(SpawnEnemyPrefabs(numberOfEnemiesToSpawn));
                enteredBefore = true;
                door.setClosed(true);
                if(totemController != null)
                {
                    totemController.startTotemHeal();
                    door.enemyCount += 1;
                }
            }          
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "TerrainHitBox")
        {
            room.setPlayerInside(false);
        }
        
    }

    IEnumerator SpawnEnemyPrefabs(int numberOfEnemiesToSpawn)
    {
        Debug.Log("Spawning " + numberOfEnemiesToSpawn + " enemies.");
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            Transform spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)];

            Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
            door.enemyCount += 1;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
