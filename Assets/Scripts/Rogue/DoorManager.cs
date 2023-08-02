using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private RoomManager room;
    public bool isClosed;
    public int enemyCount;
    public int totalEnemy = 0;
    public int enemySpawned = 0;
    public GameObject heartPickUp;

    //public GameObject doors;
    // Start is called before the first frame update
    void Start()
    {
        room = GetComponentInParent<RoomManager>();
        //doors.SetActive(false);
        isClosed = false;
    }

    // Closes all doors in the room if player is currently fighting enemies
    // Opens doors if all enemies are defeated
    void Update()
    {
        //if (isClosed)
        //{
        //    //this.enabled = true;
        //    doors.SetActive(true);
        //}
        //else
        //{
        //    //this.enabled = false;
        //    doors.SetActive(false);
        //}
        if(enemyCount <= 0 && enemySpawned >= totalEnemy)
        {
            //Debug.Log(playerMove);

           
            

            setClosed(false);
        }
    }

    public void setClosed(bool closed)
    {
        if (!closed && isClosed)
        {
            Instantiate(heartPickUp, transform.position, Quaternion.identity);
        }
        
        isClosed = closed;
        //Debug.Log("Close door");
    }

    public void setEnemyCount(int enemyCount)
    {
        this.enemyCount = enemyCount;
    }

    public bool getClosed()
    {
        return isClosed;
    }

    public void killEnemy()
    {
        enemyCount--;
    }
}


