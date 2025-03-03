using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;


public class RoomTrigger : MonoBehaviour
{
    RoomManager room;
    DoorManager door;
    public bool isStartingScene;

    public bool enteredBefore = false;
    public GameObject[] enemyPrefabs;
    public Transform[] enemySpawnPoints;
    [SerializeField]
    private TotemController totemController;
    PlayerStats playerStats;
    public GameObject mushroomEnemy;
    public GameObject chickenEnemy;

    public GameObject spawnMarker;
    // Start is called before the first frame update
    void Start()
    {
        room = GetComponentInParent<RoomManager>();
        if (!enteredBefore)
        {
            door = GetComponentInParent<DoorManager>();
        }
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
                int numberOfEnemiesToSpawn = playerStats.currentLevel + 1;
                door.totalEnemy = numberOfEnemiesToSpawn;
                if (playerStats.currentLevel >= 2)
                {
                    AddNewEnemyPrefab(mushroomEnemy);
                }
                if (playerStats.currentLevel >= 3)
                {
                    AddNewEnemyPrefab(chickenEnemy);
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

            Instantiate(spawnMarker, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(0.8f);

            Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
            door.enemyCount += 1;
            door.enemySpawned += 1;

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void AddNewEnemyPrefab (GameObject newEnemyPrefab)
    {
        GameObject[] newEnemyPrefabs = new GameObject[enemyPrefabs.Length + 1];
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            newEnemyPrefabs[i] = enemyPrefabs[i];
        }
        newEnemyPrefabs[newEnemyPrefabs.Length - 1] = newEnemyPrefab;
        enemyPrefabs = newEnemyPrefabs;
    }
}
