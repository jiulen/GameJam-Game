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
        if(collision.tag == "Player")
        {
            if (enteredBefore == false)
            {
                playerStats = collision.gameObject.GetComponent<PlayerStats>();
                SpawnEnemyPrefabs();
                enteredBefore = true;
            }
            room.setPlayerInside(true);
            door.setClosed(true);
            if(totemController != null)
            {
                totemController.startTotemHeal();
                door.enemyCount += 1;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            room.setPlayerInside(false);
            if (totemController != null)
            {
                totemController.startTotemHeal();
            }
        }
        
    }

    void SpawnEnemyPrefabs()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemyIndex], enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
            door.enemyCount += 1;
        }
    }
}
