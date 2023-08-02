using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerWithSpawn : EnemyManager
{
    [SerializeField]
    Transform enemySpawn;
    [SerializeField]
    GameObject enemyToSpawnPrefab;
    [SerializeField]
    Vector3 enemySpawnOffset;
    [SerializeField]
    ChickenManager chickenManager;

    override public void Damage(int p, bool poison = false)
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
                    chickenManager.DestroyEgg();
                    room.getActiveRoom().GetComponent<DoorManager>().enemyCount += 1; //for spawning enemy
                    room.getActiveRoom().GetComponent<DoorManager>().killEnemy();
                }

               
                if (endGame)
                {
                }
                else {
                    if (heartPickup != null)
                    {
                        switch (name)
                        {
                            case "Boar(Clone)":
                            if (!playerMove.boarKilled)
                            {
                                playerMove.boarKilled = true;
                                Instantiate(heartPickup, transform.position, Quaternion.identity);
                            }
                            break;
                            
                            case "Mushroom(Clone)":
                            if (!playerMove.mushroomKilled)
                            {
                                playerMove.mushroomKilled = true;
                                Instantiate(heartPickup, transform.position, Quaternion.identity);
                            }
                            break;
                            
                            case "Bunny(Clone)":
                            if (!playerMove.bunnyKilled)
                            {
                                playerMove.bunnyKilled = true;
                                Instantiate(heartPickup, transform.position, Quaternion.identity);
                            }
                            break;
                            
                            case "Chicken(Clone)":
                            if (!playerMove.chickenKilled)
                            {
                                playerMove.chickenKilled = true;
                                Instantiate(heartPickup, transform.position, Quaternion.identity);
                            }
                            break;
                            
                            case "Egg(Clone)":
                            if (!playerMove.eggKilled)
                            {
                                playerMove.eggKilled = true;
                                Instantiate(heartPickup, transform.position, Quaternion.identity);
                            }
                            break;
                        }
                    }
                    
                    if (lootItem != null)
                    {
                        Instantiate(lootItem, transform.position, Quaternion.identity);
                    }
                    if (!hitPlayer)
                    {
                        Debug.Log(name + " hitless");
                        if (lootItem != null)
                        {
                            Instantiate(lootItem, transform.position, Quaternion.identity);
                        }
                    }
                    else
                    {
                        Debug.Log(name + "hitfull");
                    }
                    Instantiate(enemyToSpawnPrefab, enemySpawn.position + enemySpawnOffset, Quaternion.identity);
                    Destroy(gameObject);
                    playerStats.GainExperience(experienceToGive);
                }
                
            }
        }
    }
}
