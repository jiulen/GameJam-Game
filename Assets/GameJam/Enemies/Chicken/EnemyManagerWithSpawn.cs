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
                    Instantiate(enemyToSpawnPrefab, enemySpawn.position + enemySpawnOffset, Quaternion.identity);
                    Destroy(gameObject);
                    playerStats.GainExperience(experienceToGive);
                }
                
            }
        }
    }
}
